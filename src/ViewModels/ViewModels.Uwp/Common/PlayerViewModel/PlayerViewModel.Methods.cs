// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using Windows.Media.Playback;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 播放器视图模型.
    /// </summary>
    public partial class PlayerViewModel
    {
        private void Reset()
        {
            _videoDetail = null;
            _pgcDetail = null;
            IsDetailError = false;
            _dashInformation = null;
            CurrentFormat = null;
            CurrentPgcEpisode = null;
            CurrentVideoPart = null;
            Publisher = null;
            HistoryText = string.Empty;
            _initializeProgress = TimeSpan.Zero;
            _lastReportProgress = TimeSpan.Zero;
            IsShowEpisode = false;
            IsShowParts = false;
            IsShowPgcActivityTab = false;
            IsShowSeason = false;
            IsShowRelatedVideos = false;
            IsShowChat = false;
            IsShowReply = true;
            IsShowHistory = false;
            IsCurrentEpisodeInPgcSection = false;
            IsShowEmptyLiveMessage = true;
            CurrentPlayLine = null;
            CurrentLiveQuality = null;
            _audioList.Clear();
            _videoList.Clear();
            ClearPlayer();
            IsPgc = false;
            IsLive = false;

            PgcSectionCollection.Clear();
            VideoPartCollection.Clear();
            RelatedVideoCollection.Clear();
            FormatCollection.Clear();
            EpisodeCollection.Clear();
            SeasonCollection.Clear();
            LiveQualityCollection.Clear();
            LivePlayLineCollection.Clear();
            LiveDanmakuCollection.Clear();
            FavoriteMetaCollection.Clear();

            var preferPlayerMode = _settingsToolkit.ReadLocalSetting(SettingNames.DefaultPlayerDisplayMode, PlayerDisplayMode.Default);
            PlayerDisplayMode = preferPlayerMode;
        }

        private async Task LoadVideoDetailAsync(string videoId)
        {
            if (_videoDetail == null || videoId != AvId)
            {
                Reset();
                IsDetailLoading = true;
                _videoId = Convert.ToInt64(videoId);
                try
                {
                    var detail = await Controller.GetVideoDetailAsync(_videoId);
                    _videoDetail = detail;
                }
                catch (Exception ex)
                {
                    IsDetailError = true;
                    DetailErrorText = _resourceToolkit.GetLocaleString(LanguageNames.RequestVideoFailed) + $"\n{ex.Message}";
                    IsDetailLoading = false;
                    return;
                }

                InitializeVideoDetail();
                IsDetailLoading = false;
            }

            var partId = CurrentVideoPart == null ? 0 : CurrentVideoPart.Page.Cid;
            await ChangeVideoPartAsync(partId);
        }

        private async Task LoadPgcDetailAsync(int episodeId, int seasonId = 0)
        {
            if (_pgcDetail == null ||
                episodeId.ToString() != EpisodeId ||
                seasonId.ToString() != SeasonId)
            {
                Reset();
                IsPgc = true;
                IsDetailLoading = true;
                EpisodeId = episodeId.ToString();
                SeasonId = seasonId.ToString();

                try
                {
                    var detail = await Controller.GetPgcDisplayInformationAsync(episodeId, seasonId);
                    _pgcDetail = detail;
                }
                catch (Exception ex)
                {
                    IsDetailError = true;
                    DetailErrorText = _resourceToolkit.GetLocaleString(LanguageNames.RequestPgcFailed) + $"\n{ex.Message}";
                    IsDetailLoading = false;
                    return;
                }

                InitializePgcDetail();
                IsDetailLoading = false;
            }

            var id = 0;
            if (CurrentPgcEpisode != null)
            {
                id = CurrentPgcEpisode.Id;
            }
            else if (episodeId > 0)
            {
                id = episodeId;
            }
            else if (_pgcDetail.UserStatus?.Progress != null)
            {
                id = _pgcDetail.UserStatus.Progress.LastEpisodeId;
                _initializeProgress = TimeSpan.FromSeconds(_pgcDetail.UserStatus.Progress.LastTime);
            }

            await ChangePgcEpisodeAsync(id);
        }

        private async Task LoadLiveDetailAsync(int roomId)
        {
            if (_liveDetail == null || RoomId != roomId.ToString())
            {
                Reset();
                IsLive = true;
                IsDetailLoading = true;
                IsShowReply = false;
                RoomId = roomId.ToString();

                try
                {
                    var detail = await Controller.GetLiveRoomDetailAsync(roomId);
                    _liveDetail = detail;
                }
                catch (Exception ex)
                {
                    IsDetailError = true;
                    DetailErrorText = _resourceToolkit.GetLocaleString(LanguageNames.RequestLiveFailed) + $"\n{ex.Message}";
                    IsDetailLoading = false;
                    return;
                }

                InitializeLiveDetail();
                IsDetailLoading = false;

                await Controller.ConnectToLiveRoomAsync(roomId);
                await ChangeLiveQualityAsync(4);
                await Controller.SendLiveHeartBeatAsync();
            }
        }

        private void InitializeVideoDetail()
        {
            if (_videoDetail == null)
            {
                return;
            }

            Title = _videoDetail.Arc.Title;
            Subtitle = DateTimeOffset.FromUnixTimeSeconds(_videoDetail.Arc.Pubdate).ToString("yy/MM/dd HH:mm");
            Description = _videoDetail.Arc.Desc;
            var author = _videoDetail.Arc.Author;
            Publisher = new UserViewModel(author.Name, author.Face, Convert.ToInt32(author.Mid));
            AvId = _videoDetail.Arc.Aid.ToString();
            BvId = _videoDetail.Bvid;
            SeasonId = string.Empty;
            EpisodeId = string.Empty;
            RoomId = string.Empty;
            PlayCount = _numberToolkit.GetCountText(_videoDetail.Arc.Stat.View);
            DanmakuCount = _numberToolkit.GetCountText(_videoDetail.Arc.Stat.Danmaku);
            LikeCount = _numberToolkit.GetCountText(_videoDetail.Arc.Stat.Like);
            CoinCount = _numberToolkit.GetCountText(_videoDetail.Arc.Stat.Coin);
            FavoriteCount = _numberToolkit.GetCountText(_videoDetail.Arc.Stat.Fav);
            ShareCount = _numberToolkit.GetCountText(_videoDetail.Arc.Stat.Share);
            ReplyCount = _numberToolkit.GetCountText(_videoDetail.Arc.Stat.Reply);
            ViewerCount = string.Empty;
            CoverUrl = _videoDetail.Arc.Pic;

            IsLikeChecked = _videoDetail.ReqUser.Like == 1;
            IsCoinChecked = _videoDetail.ReqUser.Coin == 1;
            IsFavoriteChecked = _videoDetail.ReqUser.Favorite == 1;

            foreach (var page in _videoDetail.Pages)
            {
                VideoPartCollection.Add(new VideoPartViewModel(page));
            }

            IsShowParts = VideoPartCollection.Count > 1;

            var relates = _videoDetail.Relates.Where(p => p.Goto.Equals(ServiceConstants.Pgc, StringComparison.OrdinalIgnoreCase) || p.Goto.Equals(ServiceConstants.Av, StringComparison.OrdinalIgnoreCase));
            IsShowRelatedVideos = relates.Count() > 0;
            foreach (var video in relates)
            {
                RelatedVideoCollection.Add(new VideoViewModel(video));
            }

            if (_videoDetail.History != null && _videoDetail.History.Progress > 0)
            {
                var title = string.Empty;
                if (IsShowParts)
                {
                    var part = VideoPartCollection.Where(p => p.Data.Page.Cid == _videoDetail.History.Cid).FirstOrDefault();
                    if (part != null)
                    {
                        title = part.Data.Page.Part;
                    }
                }

                var ts = TimeSpan.FromSeconds(_videoDetail.History.Progress);
                HistoryText = $"{_resourceToolkit.GetLocaleString(LanguageNames.PreviousView)}{title} {ts}";
            }
        }

        private void InitializePgcDetail()
        {
            if (_pgcDetail == null)
            {
                return;
            }

            Title = _pgcDetail.Title;
            Subtitle = _pgcDetail.OriginName ?? _pgcDetail.DynamicSubtitle ?? _pgcDetail.BadgeText ?? Subtitle;
            Description = $"{_pgcDetail.TypeDescription}\n" +
                $"{_pgcDetail.PublishInformation.DisplayReleaseDate}\n" +
                $"{_pgcDetail.PublishInformation.DisplayProgress}";
            AvId = string.Empty;
            BvId = string.Empty;
            RoomId = string.Empty;
            SeasonId = _pgcDetail.SeasonId.ToString();
            PlayCount = _numberToolkit.GetCountText(_pgcDetail.InformationStat.PlayCount);
            DanmakuCount = _numberToolkit.GetCountText(_pgcDetail.InformationStat.DanmakuCount);
            LikeCount = _numberToolkit.GetCountText(_pgcDetail.InformationStat.LikeCount);
            CoinCount = _numberToolkit.GetCountText(_pgcDetail.InformationStat.CoinCount);
            FavoriteCount = _numberToolkit.GetCountText(_pgcDetail.InformationStat.FavoriteCount);
            ShareCount = _numberToolkit.GetCountText(_pgcDetail.InformationStat.ShareCount);
            ReplyCount = _numberToolkit.GetCountText(_pgcDetail.InformationStat.ReplyCount);
            ViewerCount = string.Empty;
            CoverUrl = _pgcDetail.Cover;

            BadgeText = _pgcDetail.BadgeText;
            IsShowBadge = !string.IsNullOrEmpty(_pgcDetail.BadgeText);
            DisplayProgress = _pgcDetail.PublishInformation.DisplayProgress;
            PublishDate = _pgcDetail.PublishInformation.DisplayReleaseDate;
            IsShowOriginName = !string.IsNullOrEmpty(_pgcDetail.OriginName);
            OriginName = _pgcDetail.OriginName ?? string.Empty;
            IsShowAlias = !string.IsNullOrEmpty(_pgcDetail.Alias);
            Alias = _pgcDetail.Alias ?? string.Empty;
            IsShowActor = _pgcDetail.Actor != null && !string.IsNullOrEmpty(_pgcDetail.Actor.Information);
            if (IsShowActor)
            {
                ActorTitle = _pgcDetail.Actor.Title;
                ActorInformation = _pgcDetail.Actor.Information;
            }

            IsShowEditor = _pgcDetail.Staff != null && !string.IsNullOrEmpty(_pgcDetail.Staff.Information);
            if (IsShowEditor)
            {
                EditorTitle = _pgcDetail.Staff.Title;
                EditorInformation = _pgcDetail.Staff.Information;
            }

            Evaluate = _pgcDetail.Evaluate;
            PgcTypeName = _pgcDetail.TypeName;
            IsShowRating = _pgcDetail.Rating != null;
            if (IsShowRating)
            {
                Rating = _pgcDetail.Rating.Score;
                RatedCount = _numberToolkit.GetCountText(_pgcDetail.Rating.Count) + _resourceToolkit.GetLocaleString(LanguageNames.PeopleCount);
            }

            if (CelebrityCollection == null)
            {
                CelebrityCollection = new ObservableCollection<PgcCelebrity>();
            }

            CelebrityCollection.Clear();
            IsShowCelebrity = _pgcDetail.Celebrity != null;
            if (IsShowCelebrity)
            {
                _pgcDetail.Celebrity.ForEach(p => CelebrityCollection.Add(p));
            }

            IsShowPgcActivityTab = _pgcDetail.ActivityTab != null;
            if (IsShowPgcActivityTab)
            {
                PgcActivityTab = _pgcDetail.ActivityTab.DisplayName;
            }

            if (_pgcDetail.UserStatus != null)
            {
                IsFollow = _pgcDetail.UserStatus.IsFollow == 1;
            }

            if (_pgcDetail.Modules != null && _pgcDetail.Modules.Count > 0)
            {
                var seasonModule = _pgcDetail.Modules.Where(p => p.Style == ServiceConstants.Season).FirstOrDefault();
                IsShowSeason = seasonModule != null && seasonModule.Data.Seasons.Count > 1;
                if (IsShowSeason)
                {
                    foreach (var item in seasonModule.Data.Seasons)
                    {
                        SeasonCollection.Add(new PgcSeasonViewModel(item, item.SeasonId.ToString() == SeasonId));
                    }
                }

                var episodeModule = _pgcDetail.Modules.Where(p => p.Style == ServiceConstants.Positive).FirstOrDefault();
                IsShowEpisode = episodeModule != null && episodeModule.Data?.Episodes?.Count > 1;

                if (episodeModule != null && episodeModule.Data.Episodes != null)
                {
                    foreach (var item in episodeModule.Data.Episodes)
                    {
                        EpisodeCollection.Add(new PgcEpisodeViewModel(item, false));
                    }
                }

                var partModuleList = _pgcDetail.Modules.Where(p => p.Style == ServiceConstants.Section).ToList();
                IsShowSection = partModuleList.Count > 0;
                if (IsShowSection)
                {
                    foreach (var item in partModuleList)
                    {
                        if (item.Data?.Episodes?.Any() ?? false)
                        {
                            PgcSectionCollection.Add(new PgcSectionViewModel(item));
                        }
                    }
                }
            }
        }

        private void InitializeLiveDetail()
        {
            if (_liveDetail == null)
            {
                return;
            }

            Title = _liveDetail.RoomInformation.Title;
            Subtitle = _liveDetail.RoomInformation.AreaName + " · " + _liveDetail.RoomInformation.ParentAreaName;
            var descRegex = new Regex(@"<[^>]*>");
            var desc = descRegex.Replace(_liveDetail.RoomInformation.Description, string.Empty).Trim();
            Description = WebUtility.HtmlDecode(desc);
            RoomId = _liveDetail.RoomInformation.RoomId.ToString();
            AvId = string.Empty;
            BvId = string.Empty;
            SeasonId = string.Empty;
            EpisodeId = string.Empty;
            PlayCount = string.Empty;
            DanmakuCount = string.Empty;
            LikeCount = string.Empty;
            CoinCount = string.Empty;
            FavoriteCount = string.Empty;
            ShareCount = string.Empty;
            ReplyCount = string.Empty;
            ViewerCount = _numberToolkit.GetCountText(_liveDetail.RoomInformation.ViewerCount);
            CoverUrl = _liveDetail.RoomInformation.Cover ?? _liveDetail.RoomInformation.Keyframe;
            var user = _liveDetail.AnchorInformation.UserBasicInformation;
            Publisher = new UserViewModel(user.UserName, user.Avatar, _liveDetail.RoomInformation.UserId);
            IsShowChat = true;
        }

        private async Task InitializeVideoPlayInformationAsync(PlayerDashInformation videoPlayView)
        {
            _audioList = videoPlayView.VideoInformation.Audio.ToList();
            _videoList = videoPlayView.VideoInformation.Video.ToList();

            _currentAudio = null;
            _currentVideo = null;

            FormatCollection.Clear();
            var isLogin = AccountViewModel.Instance.Mid != null && AccountViewModel.Instance.Mid > 0;
            var isVip = AccountViewModel.Instance.IsVip;
            foreach (var format in videoPlayView.SupportFormats)
            {
                var canAdd = false;
                if (isLogin)
                {
                    if (!(format.Quality >= 112 && !isVip))
                    {
                        canAdd = true;
                    }
                }
                else
                {
                    canAdd = format.Quality <= 64;
                }

                if (canAdd)
                {
                    FormatCollection.Add(new VideoFormatViewModel(format, false));
                }
            }

            var formatId = CurrentFormat == null ?
                _settingsToolkit.ReadLocalSetting(SettingNames.DefaultVideoFormat, 64) :
                CurrentFormat.Quality;

            // 如果用户选择了4K优先，则优先播放4K片源.
            if (_settingsToolkit.ReadLocalSetting(SettingNames.IsPrefer4K, false) &&
                FormatCollection.Any(p => p.Data.Quality == 120))
            {
                formatId = 120;
            }

            await ChangeFormatAsync(formatId);
        }

        private async Task InitializeLivePlayInformationAsync(LivePlayInformation livePlayInfo)
        {
            LiveQualityCollection.Clear();
            LivePlayLineCollection.Clear();
            foreach (var q in livePlayInfo.AcceptQuality)
            {
                var quality = livePlayInfo.QualityDescriptions.Where(p => p.Quality.ToString() == q).FirstOrDefault();
                if (quality != null)
                {
                    LiveQualityCollection.Add(new LiveQualityViewModel(quality, quality.Quality == livePlayInfo.CurrentQuality));
                }
            }

            livePlayInfo.PlayLines.ForEach(p => LivePlayLineCollection.Add(new LivePlayLineViewModel(p)));

            var currentQuality = LiveQualityCollection.Where(p => p.IsSelected).FirstOrDefault();
            if (currentQuality == null)
            {
                currentQuality = LiveQualityCollection.First();
            }

            CurrentLiveQuality = currentQuality.Data;

            if (CurrentPlayLine != null)
            {
                foreach (var item in LivePlayLineCollection)
                {
                    item.IsSelected = item.Data.Order == CurrentPlayLine.Order;
                }

                CurrentPlayLine = LivePlayLineCollection.Where(p => p.IsSelected).FirstOrDefault()?.Data ?? LivePlayLineCollection.First().Data;
            }
            else
            {
                CurrentPlayLine = LivePlayLineCollection.First().Data;
            }

            if (CurrentPlayLine == null)
            {
                IsPlayInformationError = true;
                PlayInformationErrorText = "无法获取正确的播放地址";

                return;
            }

            await InitializeLiveDashAsync(CurrentPlayLine.Url);
        }

        private void InitializeTimer()
        {
            if (_progressTimer == null)
            {
                _progressTimer = new Windows.UI.Xaml.DispatcherTimer();
                _progressTimer.Interval = TimeSpan.FromSeconds(5);
                _progressTimer.Tick += OnProgressTimerTickAsync;
            }

            if (_heartBeatTimer == null)
            {
                _heartBeatTimer = new Windows.UI.Xaml.DispatcherTimer();
                _heartBeatTimer.Interval = TimeSpan.FromSeconds(25);
                _heartBeatTimer.Tick += OnHeartBeatTimerTickAsync;
            }
        }

        private int GetPreferCodecId()
        {
            var id = 7;
            switch (PreferCodec)
            {
                case PreferCodec.H265:
                    id = 12;
                    break;
                default:
                    break;
            }

            return id;
        }

        private void CheckPartSelection()
        {
            foreach (var item in VideoPartCollection)
            {
                item.IsSelected = item.Data.Equals(CurrentVideoPart);
            }
        }

        private void CheckEpisodeSelection()
        {
            foreach (var item in EpisodeCollection)
            {
                item.IsSelected = item.Data.Equals(CurrentPgcEpisode);
            }

            foreach (var section in PgcSectionCollection)
            {
                foreach (var epi in section.Episodes)
                {
                    epi.IsSelected = epi.Data.Equals(CurrentPgcEpisode);
                }
            }

            IsCurrentEpisodeInPgcSection = !EpisodeCollection.Any(p => p.Data.Equals(CurrentPgcEpisode));
        }

        private void CheckFormatSelection()
        {
            foreach (var item in FormatCollection)
            {
                item.IsSelected = item.Data.Equals(CurrentFormat);
            }
        }

        private async Task CheckVideoHistoryAsync()
        {
            var history = _videoDetail.History;
            if (CurrentVideoPart == null || history.Cid != CurrentVideoPart?.Page.Cid)
            {
                await ChangeVideoPartAsync(history.Cid);
                _initializeProgress = TimeSpan.FromSeconds(history.Progress);
            }
            else
            {
                _currentVideoPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(history.Progress);
            }
        }

        private async void OnProgressTimerTickAsync(object sender, object e)
        {
            if (_videoDetail == null || CurrentVideoPart == null)
            {
                return;
            }

            if (_currentVideoPlayer == null || _currentVideoPlayer.PlaybackSession == null)
            {
                return;
            }

            var progress = _currentVideoPlayer.PlaybackSession.Position;
            if (progress != _lastReportProgress)
            {
                var videoId = IsPgc ? 0 : _videoId;
                var partId = IsPgc ? 0 : CurrentVideoPart?.Page?.Cid ?? 0;
                var episodeId = IsPgc ? Convert.ToInt32(EpisodeId) : 0;
                var seasonId = IsPgc ? Convert.ToInt32(SeasonId) : 0;
                await Controller.ReportHistoryAsync(videoId, partId, episodeId, seasonId, _currentVideoPlayer.PlaybackSession.Position);
                _lastReportProgress = progress;
            }
        }

        private async void OnHeartBeatTimerTickAsync(object sender, object e)
        {
            if (_currentVideoPlayer == null || _currentVideoPlayer.PlaybackSession == null)
            {
                return;
            }

            if (_videoType == VideoType.Live)
            {
                await Controller.SendLiveHeartBeatAsync();
            }
        }

        private MediaPlayer InitializeMediaPlayer()
        {
            var player = new MediaPlayer();
            player.CurrentStateChanged += OnMediaPlayerCurrentStateChangedAsync;
            player.MediaEnded += OnMediaPlayerEndedAsync;
            player.AutoPlay = IsAutoPlay;
            player.Volume = Volume;
            player.VolumeChanged += OnMediaPlayerVolumeChangedAsync;

            return player;
        }

        private async void OnMediaPlayerVolumeChangedAsync(MediaPlayer sender, object args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Volume = sender.Volume;
            });
        }

        private async void OnMediaPlayerEndedAsync(MediaPlayer sender, object args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                PlayerStatus = PlayerStatus.End;
                if (IsLive)
                {
                    var currentOrder = CurrentPlayLine == null ? -1 : CurrentPlayLine.Order;
                    if (currentOrder == LivePlayLineCollection.Count - 1)
                    {
                        currentOrder = -1;
                    }

                    await ChangeLivePlayLineAsync(currentOrder + 1);
                }
            });
        }

        private async void OnMediaPlayerCurrentStateChangedAsync(MediaPlayer sender, object args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    switch (sender.PlaybackSession.PlaybackState)
                    {
                        case MediaPlaybackState.None:
                            PlayerStatus = PlayerStatus.End;
                            break;
                        case MediaPlaybackState.Opening:
                            PlayerStatus = PlayerStatus.Playing;
                            break;
                        case MediaPlaybackState.Playing:
                            PlayerStatus = PlayerStatus.Playing;

                            if (!string.IsNullOrEmpty(HistoryText) && _initializeProgress == TimeSpan.Zero)
                            {
                                IsShowHistory = true;
                            }

                            if (sender.PlaybackSession.Position < _initializeProgress)
                            {
                                sender.PlaybackSession.Position = _initializeProgress;
                                _initializeProgress = TimeSpan.Zero;
                            }

                            break;
                        case MediaPlaybackState.Buffering:
                            PlayerStatus = PlayerStatus.Buffering;
                            break;
                        case MediaPlaybackState.Paused:
                            PlayerStatus = PlayerStatus.Pause;
                            break;
                        default:
                            PlayerStatus = PlayerStatus.NotLoad;
                            break;
                    }
                }
                catch (Exception)
                {
                    PlayerStatus = PlayerStatus.NotLoad;
                }
            });
        }
    }
}

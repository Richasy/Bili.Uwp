// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

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
            _initializeProgress = TimeSpan.Zero;
            _lastReportProgress = TimeSpan.Zero;
            IsShowEpisode = false;
            IsShowParts = false;
            IsShowPgcActivityTab = false;
            IsShowSeason = false;
            IsShowRelatedVideos = false;
            IsShowChat = false;
            IsShowReply = true;
            IsCurrentEpisodeInPgcSection = false;
            PgcSectionCollection.Clear();
            VideoPartCollection.Clear();
            RelatedVideoCollection.Clear();
            FormatCollection.Clear();
            EpisodeCollection.Clear();
            SeasonCollection.Clear();
            _audioList.Clear();
            _videoList.Clear();
            ClearPlayer();
            IsPgc = false;
            IsLive = false;
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

        private async Task LoadLiveDetailAsync(int roomId, string h264Url, string h265Url)
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

                var url = string.Empty;
                if (string.IsNullOrEmpty(h264Url) || string.IsNullOrEmpty(h265Url))
                {
                    url = string.IsNullOrEmpty(h264Url) ? h265Url : h264Url;
                }
                else
                {
                    url = PreferCodec == PreferCodec.H264 ? h264Url : h265Url;
                }

                if (string.IsNullOrEmpty(url))
                {
                    IsPlayInformationError = true;
                    PlayInformationErrorText = "无法获取正确的播放地址";
                    return;
                }

                await InitializeLiveDashAsync(url);
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
            Publisher = new PublisherViewModel(_videoDetail.Arc.Author);
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
                IsShowEpisode = episodeModule != null && episodeModule.Data.Episodes.Count > 1;
                if (IsShowEpisode)
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
            Description = _liveDetail.RoomInformation.Description;
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
            ViewerCount = _numberToolkit.GetCountText(_liveDetail.AnchorInformation.RelationInformation.AttentionCount);
            CoverUrl = _liveDetail.RoomInformation.Cover ?? _liveDetail.RoomInformation.Keyframe;
            Publisher = new PublisherViewModel(_liveDetail.AnchorInformation.UserBasicInformation);
            IsShowChat = true;
        }

        private async Task InitializeVideoPlayInformationAsync(PlayerDashInformation videoPlayView)
        {
            _audioList = videoPlayView.VideoInformation.Audio.ToList();
            _videoList = videoPlayView.VideoInformation.Video.ToList();

            _currentAudio = null;
            _currentVideo = null;

            FormatCollection.Clear();
            videoPlayView.SupportFormats.ForEach(p => FormatCollection.Add(new VideoFormatViewModel(p, false)));

            var formatId = CurrentFormat == null ?
                _settingsToolkit.ReadLocalSetting(Models.Enums.SettingNames.DefaultVideoFormat, 64) :
                CurrentFormat.Quality;

            await ChangeFormatAsync(formatId);
        }

        private void InitializeTimer()
        {
            if (_progressTimer == null)
            {
                _progressTimer = new Windows.UI.Xaml.DispatcherTimer();
                _progressTimer.Interval = TimeSpan.FromSeconds(5);
                _progressTimer.Tick += OnProgressTimerTickAsync;
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
    }
}

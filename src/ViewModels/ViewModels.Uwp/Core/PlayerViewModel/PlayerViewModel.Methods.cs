// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bili.Models.App;
using Bili.Models.App.Constants;
using Bili.Models.App.Other;
using Bili.Models.BiliBili;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using Bili.ViewModels.Uwp.Account;
using Bilibili.App.View.V1;
using Splat;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 播放器视图模型.
    /// </summary>
    public partial class PlayerViewModel
    {
        private async void ResetAsync()
        {
            _videoDetail = null;
            _pgcDetail = null;
            IsDetailError = false;
            _playerInformation = null;
            _interactionNodeId = 0;
            _interactionPartId = 0;
            _isFirstShowHistory = true;
            CurrentFormat = null;
            CurrentPgcEpisode = null;
            CurrentVideoPart = null;
            CurrentUgcEpisode = null;
            CurrentUgcSection = null;
            Publisher = null;
            HistoryTipText = string.Empty;
            _initializeProgress = TimeSpan.Zero;
            _lastReportProgress = TimeSpan.Zero;
            IsShowEpisode = false;
            IsShowParts = false;
            IsShowPgcActivityTab = false;
            IsShowSeason = false;
            IsShowRelatedVideos = false;
            IsShowChat = false;
            IsShowReply = true;
            IsShowHistoryTip = false;
            IsShowNextVideoTip = false;
            IsShowUgcSection = false;
            IsShowSection = false;
            IsPlayInformationError = false;
            IsCurrentEpisodeInPgcSection = false;
            IsShowEmptyLiveMessage = true;
            IsLiveMessageAutoScroll = true;
            CurrentPlayUrl = null;
            CurrentSubtitleIndex = null;
            CurrentSubtitle = string.Empty;
            IsShowSubtitle = false;
            IsShowAudioCover = false;
            _audioList.Clear();
            _videoList.Clear();
            _subtitleList.Clear();
            ClearPlayer();
            IsPgc = false;
            IsLive = false;
            IsInteraction = false;
            IsLikeChecked = false;
            IsCoinChecked = false;
            IsFollow = false;
            IsFavoriteChecked = false;
            IsEnableLikeHolding = true;
            IsShowChoice = false;
            IsShowInteractionEnd = false;
            IsShowSwitchEpisodeButton = false;
            IsNextEpisodeButtonEnabled = false;
            IsPreviousEpisodeButtonEnabled = false;

            PgcSectionCollection.Clear();
            VideoPartCollection.Clear();
            RelatedVideoCollection.Clear();
            FormatCollection.Clear();
            EpisodeCollection.Clear();
            SeasonCollection.Clear();
            LiveAppQualityCollection.Clear();
            LiveDanmakuCollection.Clear();
            FavoriteMetaCollection.Clear();
            SubtitleIndexCollection.Clear();
            StaffCollection.Clear();
            ChoiceCollection.Clear();
            TagCollection.Clear();
            UgcEpisodeCollection.Clear();
            UgcSectionCollection.Clear();
            ReplyModuleViewModel.Instance.SetInformation(0, Models.Enums.Bili.CommentType.None);
            Controller.CleanupLiveSocket();
            await ClearInitViewModelAsync();
        }

        private async Task LoadVideoDetailAsync(string videoId, bool isRefresh)
        {
            if (_videoDetail == null || videoId != AvId || isRefresh)
            {
                ResetAsync();
                IsDetailLoading = true;
                try
                {
                    _videoDetail = videoId.StartsWith("bv", StringComparison.OrdinalIgnoreCase)
                        ? await Controller.GetVideoDetailAsync(videoId)
                        : await Controller.GetVideoDetailAsync(Convert.ToInt64(videoId.Replace("av", string.Empty)));
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

            await InitializeUserRelationAsync();

            if (IsInteraction)
            {
                await InitializeInteractionVideoAsync();
            }
            else
            {
                var partId = CurrentVideoPart == null ? 0 : CurrentVideoPart.Page.Cid;
                await ChangeVideoPartAsync(partId);
            }
        }

        private async Task LoadPgcDetailAsync(int episodeId, int seasonId = 0, bool isRefresh = false, string title = "")
        {
            if (_pgcDetail == null ||
                episodeId.ToString() != EpisodeId ||
                seasonId.ToString() != SeasonId ||
                isRefresh)
            {
                ResetAsync();
                IsPgc = true;
                IsDetailLoading = true;
                EpisodeId = episodeId.ToString();
                SeasonId = seasonId.ToString();

                try
                {
                    var proxyPack = GetProxyAndArea(title, false);
                    var detail = await Controller.GetPgcDisplayInformationAsync(episodeId, seasonId, proxyPack.Item1, proxyPack.Item2);
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
            }

            IsDetailLoading = false;
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
                var lastId = _pgcDetail.UserStatus.Progress.LastEpisodeId;

                if (EpisodeCollection.Any(p => p.Data.Id == lastId))
                {
                    id = lastId;
                    _initializeProgress = TimeSpan.FromSeconds(_pgcDetail.UserStatus.Progress.LastTime);
                }
            }

            await ChangePgcEpisodeAsync(id);
        }

        private async Task LoadLiveDetailAsync(int roomId)
        {
            ResetAsync();
            IsLive = true;
            IsDetailLoading = true;
            IsShowReply = false;
            RoomId = roomId.ToString();

            if (PlaybackRate != 1)
            {
                PlaybackRate = 1;
            }

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

            await InitializeUserRelationAsync();
            await Controller.ConnectToLiveRoomAsync(roomId);
            await ChangeLivePlayBehaviorAsync(150);
            await Controller.SendLiveHeartBeatAsync();
        }

        private void InitializeVideoDetail()
        {
            if (_videoDetail == null)
            {
                return;
            }

            _videoId = _videoDetail.Arc.Aid;
            Title = _videoDetail.Arc.Title;
            Subtitle = DateTimeOffset.FromUnixTimeSeconds(_videoDetail.Arc.Pubdate).ToLocalTime().ToString("yy/MM/dd HH:mm");
            Description = _videoDetail.Arc.Desc;
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
            IsInteraction = _videoDetail.Interaction != null;
            IsContentFixed = Splat.Locator.Current.GetService<AccountViewModel>().FixedItemCollection.Any(p => p.Id == AvId);
            _videoDetail.Tag.Select(p => new VideoTag { Id = p.Id.ToString(), Name = p.Name.TrimStart('#'), Uri = p.Uri })
                .ToList()
                .ForEach(p => TagCollection.Add(p));
            ReplyModuleViewModel.Instance.SetInformation(Convert.ToInt32(_videoDetail.Arc.Aid), Models.Enums.Bili.CommentType.Video);

            if (_videoDetail.Staff.Count > 0)
            {
                // 联合创作.
                IsShowStaff = true;
                foreach (var user in _videoDetail.Staff)
                {
                    var vm = new UserViewModel(user);
                    StaffCollection.Add(vm);
                }
            }
            else
            {
                // 独立创作.
                IsShowStaff = false;
                var author = _videoDetail.Arc.Author;
                Publisher = new UserViewModel(author.Name, author.Face, Convert.ToInt32(author.Mid));
            }

            if (IsInteraction)
            {
                if (_videoDetail.Interaction.HistoryNode != null)
                {
                    _interactionPartId = _videoDetail.Interaction.HistoryNode.Cid;
                    _interactionNodeId = _videoDetail.Interaction.HistoryNode.NodeId;
                }
                else
                {
                    _interactionPartId = _videoDetail.Pages.First().Page.Cid;
                }
            }

            IsLikeChecked = _videoDetail.ReqUser.Like == 1;
            IsCoinChecked = _videoDetail.ReqUser.Coin == 1;
            IsFavoriteChecked = _videoDetail.ReqUser.Favorite == 1;

            foreach (var page in _videoDetail.Pages)
            {
                VideoPartCollection.Add(new VideoPartViewModel(page));
            }

            if (_videoDetail.UgcSeason != null && _videoDetail.UgcSeason.Sections?.Count > 0)
            {
                IsShowUgcSection = true;
                foreach (var item in _videoDetail.UgcSeason.Sections)
                {
                    UgcSectionCollection.Add(item);
                }

                CurrentUgcEpisode = _videoDetail.UgcSeason.Sections.SelectMany(p => p.Episodes).FirstOrDefault(j => j.Aid == _videoDetail.Arc.Aid);
                ChangeUgcSection(_videoDetail.UgcSeason.Sections.FirstOrDefault());
            }

            IsShowParts = VideoPartCollection.Count > 1;
            IsShowSwitchEpisodeButton = IsShowParts;

            var relates = _videoDetail.Relates.Where(p => p.Goto.Equals(ServiceConstants.Pgc, StringComparison.OrdinalIgnoreCase) || p.Goto.Equals(ServiceConstants.Av, StringComparison.OrdinalIgnoreCase));
            IsShowRelatedVideos = relates.Count() > 0;
            foreach (var video in relates)
            {
                RelatedVideoCollection.Add(new VideoViewModel(video));
            }

            if (_videoDetail.History != null && _videoDetail.History.Progress > 0 && !IsInteraction)
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
                else if (IsShowUgcSection)
                {
                    var ugcEpisode = GetUgcEpisodeFromCid(_videoDetail.History.Cid);
                    if (ugcEpisode != null)
                    {
                        title = ugcEpisode.Title;
                    }
                }

                var ts = TimeSpan.FromSeconds(_videoDetail.History.Progress);
                HistoryTipText = $"{_resourceToolkit.GetLocaleString(LanguageNames.PreviousView)}{title} {ts}";
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
            IsContentFixed = Splat.Locator.Current.GetService<AccountViewModel>().FixedItemCollection.Any(p => p.Id == SeasonId);
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
                IsShowSwitchEpisodeButton = IsShowEpisode;

                if (episodeModule != null && episodeModule.Data.Episodes != null)
                {
                    foreach (var item in episodeModule.Data.Episodes)
                    {
                        EpisodeCollection.Add(new PgcEpisodeViewModel(item, false));
                    }

                    if (EpisodeCollection.Count == 0)
                    {
                        IsPlayInformationError = true;
                        PlayInformationErrorText = _resourceToolkit.GetLocaleString(LanguageNames.NoEpisode);
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
            if (string.IsNullOrEmpty(Description.Trim()))
            {
                Description = _resourceToolkit.GetLocaleString(LanguageNames.NoRoomDescription);
            }

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
            LivePartition = (_liveDetail.RoomInformation.ParentAreaName ?? "--") + " · " + _liveDetail.RoomInformation.AreaName;
            IsContentFixed = Splat.Locator.Current.GetService<AccountViewModel>().FixedItemCollection.Any(p => p.Id == RoomId.ToString());
            IsShowChat = true;
        }

        private async Task InitializeVideoPlayInformationAsync(PlayerInformation videoPlayView)
        {
            if (videoPlayView.VideoInformation != null)
            {
                _videoList = videoPlayView.VideoInformation.Video.ToList();
                _audioList = videoPlayView.VideoInformation.Audio?.ToList() ?? new List<DashItem>();
            }

            _currentAudio = null;
            _currentVideo = null;

            FormatCollection.Clear();
            var isLogin = Splat.Locator.Current.GetService<AccountViewModel>().Mid != null && Splat.Locator.Current.GetService<AccountViewModel>().Mid > 0;
            var isVip = Splat.Locator.Current.GetService<AccountViewModel>().IsVip;
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

            // 如果用户选择了画质优先，则优先播放高画质片源.
            if (_settingsToolkit.ReadLocalSetting(SettingNames.IsPreferHighQuality, false))
            {
                formatId = FormatCollection.Max(p => p.Data.Quality);
            }

            await ChangeFormatAsync(formatId);

            if (_progressTimer != null && !_progressTimer.IsEnabled)
            {
                _progressTimer.Start();
            }
        }

        private async Task InitializeAppLivePlayInformationAsync(LiveAppPlayUrlInfo livePlayInfo)
        {
            LiveAppQualityCollection.Clear();
            LiveAppPlayLineCollection.Clear();

            var qualities = livePlayInfo.PlayUrl.Descriptions;
            var stream = livePlayInfo.PlayUrl.StreamList.First();
            var format = stream.FormatList.First();
            var codec = format.CodecList.First();

            if (PreferCodec != PreferCodec.Flv && livePlayInfo.PlayUrl.StreamList.Any(p => p.ProtocolName.Equals("http_hls")))
            {
                var hlsStream = livePlayInfo.PlayUrl.StreamList.First(p => p.ProtocolName.Equals("http_hls"));
                var hlsFormat = hlsStream.FormatList.First();
                var isInScope = false;
                if (PreferCodec == PreferCodec.H264 && hlsFormat.CodecList.Any(p => p.CodecName.Equals("avc")))
                {
                    isInScope = true;
                    codec = hlsFormat.CodecList.First(p => p.CodecName.Equals("avc"));
                }
                else if (PreferCodec == PreferCodec.H265 && hlsFormat.CodecList.Any(p => p.CodecName.Equals("hevc")))
                {
                    isInScope = true;
                    codec = hlsFormat.CodecList.First(p => p.CodecName.Equals("hevc"));
                }
                else if (PreferCodec == PreferCodec.Av1 && hlsFormat.CodecList.Any(p => p.CodecName.Equals("av1")))
                {
                    isInScope = true;
                    codec = hlsFormat.CodecList.First(p => p.CodecName.Equals("av1"));
                }

                if (isInScope)
                {
                    stream = hlsStream;
                    format = hlsFormat;
                }
            }

            foreach (var q in codec.AcceptQualities)
            {
                var desc = qualities.First(p => p.Quality == q);
                LiveAppQualityCollection.Add(new LiveAppQualityViewModel(desc, q == codec.CurrentQuality));
            }

            var currentQuality = LiveAppQualityCollection.Where(p => p.IsSelected).FirstOrDefault();
            if (currentQuality == null)
            {
                currentQuality = LiveAppQualityCollection.Where(p => p.Data.Quality == codec.CurrentQuality).FirstOrDefault() ?? LiveAppQualityCollection.First();
            }

            CurrentAppLiveQuality = currentQuality.Data;
            var tempUrlIndex = 0;
            for (var i = 0; i < codec.Urls.Count; i++)
            {
                var data = codec.Urls[i];

                // 直播链接的Host有两种，一种是IP地址，一种是.com结尾的域名。前者解析不出来，只有后者可以正常播放.
                if (data.Host.EndsWith(".com"))
                {
                    tempUrlIndex++;
                    LiveAppPlayLineCollection.Add(new LiveAppPlayLineViewModel(data, codec.BaseUrl, tempUrlIndex));
                }
            }

            if (CurrentPlayUrl != null)
            {
                foreach (var item in LiveAppPlayLineCollection)
                {
                    item.IsSelected = item.Data.Host == CurrentPlayUrl.Data.Host;
                }

                CurrentPlayUrl = LiveAppPlayLineCollection.Where(p => p.IsSelected).FirstOrDefault() ?? LiveAppPlayLineCollection.FirstOrDefault();
            }
            else
            {
                CurrentPlayUrl = LiveAppPlayLineCollection.FirstOrDefault();
            }

            if (CurrentPlayUrl == null)
            {
                IsPlayInformationError = true;
                PlayInformationErrorText = "无法获取正确的播放地址";

                return;
            }

            await InitializeLiveDashAsync(CurrentPlayUrl.Url);
        }

        private void InitializeTimer()
        {
            if (_progressTimer == null)
            {
                _progressTimer = new DispatcherTimer();
                _progressTimer.Interval = TimeSpan.FromSeconds(5);
                _progressTimer.Tick += OnProgressTimerTickAsync;
            }

            if (_heartBeatTimer == null)
            {
                _heartBeatTimer = new DispatcherTimer();
                _heartBeatTimer.Interval = TimeSpan.FromSeconds(25);
                _heartBeatTimer.Tick += OnHeartBeatTimerTickAsync;
            }

            if (_subtitleTimer == null)
            {
                _subtitleTimer = new DispatcherTimer();
                _subtitleTimer.Interval = TimeSpan.FromSeconds(0.5);
                _subtitleTimer.Tick += OnSubtitleTimerTickAsync;
            }
        }

        private string GetPreferCodecId()
        {
            var id = "avc";
            switch (PreferCodec)
            {
                case PreferCodec.H265:
                    id = "hev";
                    break;
                case PreferCodec.Av1:
                    id = "av01";
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

            if (VideoPartCollection.Count > 0 && CurrentVideoPart != null)
            {
                IsPreviousEpisodeButtonEnabled = CurrentVideoPart.Page.Page_ != VideoPartCollection.First().Data.Page.Page_;
                IsNextEpisodeButtonEnabled = CurrentVideoPart.Page.Page_ != VideoPartCollection.Last().Data.Page.Page_;
            }
        }

        private void CheckUgcEpisodeSelection()
        {
            foreach (var item in UgcEpisodeCollection)
            {
                item.IsSelected = item.VideoId.Equals(CurrentUgcEpisode.Aid.ToString());
            }

            if (UgcEpisodeCollection.Count > 0 && CurrentUgcEpisode != null)
            {
                IsPreviousEpisodeButtonEnabled = CurrentUgcEpisode.Aid.ToString() != UgcEpisodeCollection.First().VideoId;
                IsNextEpisodeButtonEnabled = CurrentUgcEpisode.Aid.ToString() != UgcEpisodeCollection.Last().VideoId;
            }
        }

        private long GetCurrentPartId()
        {
            if (IsInteraction)
            {
                return _interactionPartId;
            }
            else if (UgcEpisodeCollection.Count > 0)
            {
                return CurrentUgcEpisode?.Page?.Cid ?? 0;
            }
            else if (VideoPartCollection.Count > 0)
            {
                return CurrentVideoPart?.Page?.Cid ?? 0;
            }

            return 0;
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

            if (EpisodeCollection.Count > 0)
            {
                IsPreviousEpisodeButtonEnabled = CurrentPgcEpisode.Index != EpisodeCollection.First().Data.Index;
                IsNextEpisodeButtonEnabled = CurrentPgcEpisode.Index != EpisodeCollection.Last().Data.Index;
            }
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
            if (IsShowParts)
            {
                if (history.Cid != GetCurrentPartId())
                {
                    await ChangeVideoPartAsync(history.Cid);
                    _initializeProgress = TimeSpan.FromSeconds(history.Progress);
                }
                else
                {
                    _currentVideoPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(history.Progress);
                }
            }
            else if (IsShowUgcSection)
            {
                if (history.Cid != CurrentUgcEpisode?.Cid)
                {
                    var episode = GetUgcEpisodeFromCid(history.Cid);
                    if (episode != null)
                    {
                        await LoadAsync(new VideoViewModel(episode));
                        _initializeProgress = TimeSpan.FromSeconds(history.Progress);
                    }
                }
                else
                {
                    _currentVideoPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(history.Progress);
                }
            }
            else
            {
                _currentVideoPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(history.Progress);
            }
        }

        private async Task CheckPgcHistoryAsync()
        {
            var history = _pgcDetail.UserStatus.Progress;
            if (CurrentPgcEpisode == null || history.LastEpisodeId != CurrentPgcEpisode.Id)
            {
                await ChangePgcEpisodeAsync(history.LastEpisodeId);
                _initializeProgress = TimeSpan.FromSeconds(history.LastTime);
            }
            else
            {
                _currentVideoPlayer.PlaybackSession.Position = TimeSpan.FromSeconds(history.LastTime);
            }
        }

        private async void OnProgressTimerTickAsync(object sender, object e)
        {
            if (_videoDetail == null && CurrentVideoPart == null && _pgcDetail == null && CurrentPgcEpisode == null)
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
                var videoId = IsPgc ? CurrentPgcEpisode.Aid : _videoId;
                var partId = IsPgc ? CurrentPgcEpisode.PartId : GetCurrentPartId();
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

        private void OnSubtitleTimerTickAsync(object sender, object e)
        {
            if (PlayerStatus == PlayerStatus.Playing && _subtitleList.Count > 0 && CanShowSubtitle)
            {
                var progress = _currentVideoPlayer.PlaybackSession.Position;
                var sec = progress.TotalSeconds;
                var subtitle = _subtitleList.Where(p => p.From <= sec && p.To >= sec).FirstOrDefault();
                if (subtitle != null && !string.IsNullOrEmpty(subtitle.Content))
                {
                    IsShowSubtitle = true;
                    var sub = subtitle.Content;
                    if (SubtitleConvertType != Models.Enums.App.SubtitleConvertType.None)
                    {
                        switch (SubtitleConvertType)
                        {
                            case Models.Enums.App.SubtitleConvertType.ToSimplifiedChinese:
                                sub = ToolGood.Words.WordsHelper.ToSimplifiedChinese(sub);
                                break;
                            case Models.Enums.App.SubtitleConvertType.ToTraditionalChinese:
                                sub = ToolGood.Words.WordsHelper.ToTraditionalChinese(sub);
                                break;
                            default:
                                break;
                        }
                    }

                    CurrentSubtitle = sub;
                }
                else
                {
                    IsShowSubtitle = false;
                }
            }
        }

        private MediaPlayer InitializeMediaPlayer()
        {
            var player = new MediaPlayer();
            player.MediaOpened += OnMediaPlayerOpened;
            player.CurrentStateChanged += OnMediaPlayerCurrentStateChangedAsync;
            player.MediaEnded += OnMediaPlayerEndedAsync;
            player.MediaFailed += OnMediaPlayerFailedAsync;
            player.AutoPlay = IsAutoPlay;
            player.Volume = Volume;
            player.VolumeChanged += OnMediaPlayerVolumeChangedAsync;
            player.IsLoopingEnabled = IsInfiniteLoop;

            return player;
        }

        private async Task InitializeUserRelationAsync()
        {
            if (Splat.Locator.Current.GetService<AccountViewModel>().State != AuthorizeState.SignedIn ||
                IsShowStaff ||
                Splat.Locator.Current.GetService<AccountViewModel>().Mid.Value == Publisher.Id)
            {
                return;
            }

            try
            {
                // var relation = await Controller.GetRelationAsync(Publisher.Id);
                // Publisher.IsFollow = relation.IsFollow();
                Publisher.IsFollow = false;
                await Task.CompletedTask;
            }
            catch (Exception)
            {
                _appViewModel.ShowTip(_resourceToolkit.GetLocaleString(LanguageNames.FailedToGetUserRelation), Models.Enums.App.InfoType.Warning);
            }
        }

        private async void OnMediaPlayerVolumeChangedAsync(MediaPlayer sender, object args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                Volume = sender.Volume;
            });
        }

        private void OnMediaPlayerOpened(MediaPlayer sender, object args)
        {
            var session = sender.PlaybackSession;
            if (session != null)
            {
                if (IsLive && _interopMSS != null)
                {
                    _interopMSS.PlaybackSession = session;
                }
                else if (_initializeProgress != TimeSpan.Zero)
                {
                    session.Position = _initializeProgress;
                    _initializeProgress = TimeSpan.Zero;
                }

                session.PlaybackRate = PlaybackRate;
            }
        }

        private async void OnMediaPlayerFailedAsync(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
            if (args.ExtendedErrorCode?.HResult == -1072873851 || args.Error == MediaPlayerError.Unknown)
            {
                // 不处理 Shutdown 造成的错误.
                return;
            }

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                // 在视频未加载时不对报错进行处理.
                if (PlayerStatus == PlayerStatus.NotLoad)
                {
                    return;
                }

                PlayerStatus = PlayerStatus.End;
                IsPlayInformationError = true;
                var message = string.Empty;
                switch (args.Error)
                {
                    case MediaPlayerError.Aborted:
                        message = _resourceToolkit.GetLocaleString(LanguageNames.Aborted);
                        break;
                    case MediaPlayerError.NetworkError:
                        message = _resourceToolkit.GetLocaleString(LanguageNames.NetworkError);
                        break;
                    case MediaPlayerError.DecodingError:
                        message = _resourceToolkit.GetLocaleString(LanguageNames.DecodingError);
                        break;
                    case MediaPlayerError.SourceNotSupported:
                        message = _resourceToolkit.GetLocaleString(LanguageNames.SourceNotSupported);
                        break;
                    default:
                        break;
                }

                PlayInformationErrorText = message;
                _logger.LogError(new Exception($"播放失败: {args.Error} | {args.ErrorMessage} | {args.ExtendedErrorCode}"));
            });
        }

        private async void OnMediaPlayerEndedAsync(MediaPlayer sender, object args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                PlayerStatus = PlayerStatus.End;
                var isContinue = _settingsToolkit.ReadLocalSetting(SettingNames.IsContinusPlay, true);
                if (IsInteraction)
                {
                    if (ChoiceCollection.Count > 0)
                    {
                        if (ChoiceCollection.Count == 1 && string.IsNullOrEmpty(ChoiceCollection.First().Option))
                        {
                            ChangeChoice(ChoiceCollection.First());
                            await InitializeInteractionVideoAsync();
                        }
                        else
                        {
                            IsShowInteractionEnd = false;
                            IsShowChoice = true;
                        }
                    }
                    else
                    {
                        IsShowChoice = false;
                        IsShowInteractionEnd = true;
                    }
                }
                else if (IsPgc)
                {
                    var canContinue = !IsCurrentEpisodeInPgcSection && EpisodeCollection.Count > 1 && CurrentPgcEpisode.Index < EpisodeCollection.Last().Data.Index;
                    if (isContinue && canContinue)
                    {
                        var episode = EpisodeCollection.Where(p => p.Data.Index == CurrentPgcEpisode.Index + 1).FirstOrDefault();
                        if (episode != null)
                        {
                            await ChangePgcEpisodeAsync(episode.Data.Id);
                        }
                    }
                    else
                    {
                        PlayerDisplayMode = PlayerDisplayMode.Default;
                    }
                }
                else
                {
                    // Video
                    var canContinue = (VideoPartCollection.Count > 1 && CurrentVideoPart.Page.Page_ < VideoPartCollection.Last().Data.Page.Page_)
                        || (UgcEpisodeCollection.Count > 1 && CurrentUgcEpisode.Aid.ToString() != UgcEpisodeCollection.Last().VideoId);
                    if (isContinue && canContinue)
                    {
                        if (IsShowParts)
                        {
                            var part = VideoPartCollection.FirstOrDefault(p => p.Data.Page.Page_ == CurrentVideoPart.Page.Page_ + 1);
                            if (part != null)
                            {
                                await ChangeVideoPartAsync(part.Data.Page.Cid);
                            }
                        }
                        else if (IsShowUgcSection)
                        {
                            var index = CurrentUgcSection.Episodes.IndexOf(CurrentUgcEpisode);
                            if (index > -1 && index < CurrentUgcSection.Episodes.Count - 1)
                            {
                                var next = CurrentUgcSection.Episodes[index + 1];
                                await LoadAsync(new VideoViewModel(next));
                            }
                        }
                    }
                    else
                    {
                        if (HasNextVideo() && _settingsToolkit.ReadLocalSetting(SettingNames.IsAutoPlayNextRelatedVideo, false))
                        {
                            IsShowNextVideoTip = true;
                        }
                        else
                        {
                            PlayerDisplayMode = PlayerDisplayMode.Default;
                        }
                    }
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
                            IsPlayInformationError = false;
                            PlayerStatus = PlayerStatus.Playing;
                            break;
                        case MediaPlaybackState.Playing:
                            PlayerStatus = PlayerStatus.Playing;
                            IsPlayInformationError = false;
                            if (!string.IsNullOrEmpty(HistoryTipText) && _initializeProgress == TimeSpan.Zero && _isFirstShowHistory)
                            {
                                IsShowHistoryTip = true;
                                _isFirstShowHistory = false;
                            }

                            if (sender.PlaybackSession.Position < _initializeProgress)
                            {
                                sender.PlaybackSession.Position = _initializeProgress;
                                _initializeProgress = TimeSpan.Zero;
                            }

                            if (IsShowNextVideoTip)
                            {
                                IsShowNextVideoTip = false;
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

        private void OnUserLoggedOut(object sender, EventArgs e)
        {
            IsCoinChecked = false;
            IsLikeChecked = false;
            IsFollow = false;
            IsFavoriteChecked = false;
        }

        private async Task RecordInitViewModelToLocalAsync(string vid, int sid, VideoType type, string title)
        {
            var data = new CurrentPlayingRecord(vid, sid, type);
            await _fileToolkit.WriteLocalDataAsync(AppConstants.LastOpenVideoFileName, data);
            _settingsToolkit.WriteLocalSetting(SettingNames.CanContinuePlay, true);
            _settingsToolkit.WriteLocalSetting(SettingNames.ContinuePlayTitle, title);
        }

        private void InitializePlaybackRateProperties()
        {
            var isEnhancement = _settingsToolkit.ReadLocalSetting(SettingNames.PlaybackRateEnhancement, false);
            MaxPlaybackRate = isEnhancement ? 6d : 3d;
            PlaybackRateStep = isEnhancement ? 0.2 : 0.1;

            PlaybackRateNodeCollection.Clear();
            var defaultList = new List<double> { 0.5, 0.75, 1.0, 1.25, 1.5, 2.0 };
            if (isEnhancement)
            {
                defaultList = defaultList.Union(new List<double> { 2.5, 3.0, 3.5, 5.0 }).ToList();
            }

            defaultList.ForEach(p => PlaybackRateNodeCollection.Add(p));

            var isGlobal = _settingsToolkit.ReadLocalSetting(SettingNames.GlobalPlaybackRate, false);
            if (!isGlobal)
            {
                PlaybackRate = 1d;
            }
        }

        private bool HasNextVideo()
        {
            var result = IsShowViewLater
                ? ViewLaterVideoCollection.FirstOrDefault(p => p.IsSelected) != ViewLaterVideoCollection.Last()
                : !IsInteraction
                && _videoType == VideoType.Video
                && IsShowRelatedVideos
                && RelatedVideoCollection.Count > 0;

            if (result)
            {
                if (IsShowViewLater)
                {
                    var index = ViewLaterVideoCollection.IndexOf(ViewLaterVideoCollection.FirstOrDefault(p => p.IsSelected));
                    if (index != -1)
                    {
                        var nextVideo = ViewLaterVideoCollection[index + 1];
                        NextVideoTipText = nextVideo.Title;
                    }
                    else
                    {
                        result = false;
                    }
                }
                else
                {
                    var nextVideo = RelatedVideoCollection.First();
                    NextVideoTipText = nextVideo.Title;
                }
            }

            return result;
        }

        private Episode GetUgcEpisodeFromCid(long cid)
        {
            if (_videoDetail.UgcSeason?.Sections != null)
            {
                return _videoDetail.UgcSeason.Sections.SelectMany(p => p.Episodes).FirstOrDefault(p => p.Cid == cid);
            }

            return null;
        }

        private Tuple<string, string> GetProxyAndArea(string title, bool isVideo)
        {
            var proxy = string.Empty;
            var area = string.Empty;

            var isOpenRoaming = _settingsToolkit.ReadLocalSetting(SettingNames.IsOpenRoaming, false);
            var localProxy = isVideo
                ? _settingsToolkit.ReadLocalSetting(SettingNames.RoamingVideoAddress, string.Empty)
                : _settingsToolkit.ReadLocalSetting(SettingNames.RoamingViewAddress, string.Empty);
            if (isOpenRoaming && !string.IsNullOrEmpty(localProxy))
            {
                if (!string.IsNullOrEmpty(title))
                {
                    if (Regex.IsMatch(title, @"僅.*港.*地區"))
                    {
                        area = "hk";
                    }
                    else if (Regex.IsMatch(title, @"僅.*台.*地區"))
                    {
                        area = "tw";
                    }
                }

                var isForceProxy = _settingsToolkit.ReadLocalSetting(SettingNames.IsGlobeProxy, false);
                if ((isForceProxy && string.IsNullOrEmpty(area))
                    || !string.IsNullOrEmpty(area))
                {
                    proxy = localProxy;
                }
            }

            return new Tuple<string, string>(proxy, area);
        }

        /// <summary>
        /// 获取当前正在播放的内容Id.
        /// </summary>
        /// <returns>Id.</returns>
        private FixedItem ConvertToFixItem()
        {
            var item = new FixedItem();
            var id = string.Empty;
            switch (_videoType)
            {
                case VideoType.Video:
                    id = AvId;
                    break;
                case VideoType.Pgc:
                    id = SeasonId;
                    break;
                case VideoType.Live:
                    id = RoomId;
                    break;
                default:
                    break;
            }

            var title = Title;
            if (_videoType == VideoType.Live)
            {
                title = string.Format(_resourceToolkit.GetLocaleString(LanguageNames.SomeoneLiveRoom), Publisher.Name);
            }

            var cover = CoverUrl;
            Enum.TryParse(_videoType.ToString(), out FixedType type);
            item.Id = id;
            item.Title = title;
            item.Cover = cover;
            item.Type = type;

            return item;
        }

        private void OnBiliPlayerPointerMoved(object sender, PointerRoutedEventArgs e)
            => IsPointerInMediaElement = true;

        private void OnBiliPlayerPointerExited(object sender, PointerRoutedEventArgs e)
            => IsPointerInMediaElement = false;

        private void OnBiliPlayerPointerEntered(object sender, PointerRoutedEventArgs e)
            => IsPointerInMediaElement = true;
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using FFmpegInterop;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.ViewModels.Uwp.Common;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 播放器视图模型.
    /// </summary>
    public partial class PlayerViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerViewModel"/> class.
        /// </summary>
        public PlayerViewModel()
        {
            RelatedVideoCollection = new ObservableCollection<VideoViewModel>();
            VideoPartCollection = new ObservableCollection<VideoPartViewModel>();
            FormatCollection = new ObservableCollection<VideoFormatViewModel>();
            EpisodeCollection = new ObservableCollection<PgcEpisodeViewModel>();
            SeasonCollection = new ObservableCollection<PgcSeasonViewModel>();
            PgcSectionCollection = new ObservableCollection<PgcSectionViewModel>();
            LivePlayLineCollection = new ObservableCollection<LivePlayLineViewModel>();
            LiveQualityCollection = new ObservableCollection<LiveQualityViewModel>();
            LiveDanmakuCollection = new ObservableCollection<LiveDanmakuMessage>();
            FavoriteMetaCollection = new ObservableCollection<FavoriteMetaViewModel>();
            _audioList = new List<DashItem>();
            _videoList = new List<DashItem>();
            _lastReportProgress = TimeSpan.Zero;

            _liveFFConfig = new FFmpegInteropConfig();
            _liveFFConfig.FFmpegOptions.Add("rtsp_transport", "tcp");
            _liveFFConfig.FFmpegOptions.Add("user_agent", ServiceConstants.DefaultUserAgentString);
            _liveFFConfig.FFmpegOptions.Add("referer", "https://live.bilibili.com/");

            ServiceLocator.Instance.LoadService(out _numberToolkit)
                                   .LoadService(out _resourceToolkit)
                                   .LoadService(out _settingsToolkit)
                                   .LoadService(out _fileToolkit)
                                   .LoadService(out _logger);
            PlayerDisplayMode = _settingsToolkit.ReadLocalSetting(SettingNames.DefaultPlayerDisplayMode, PlayerDisplayMode.Default);
            Volume = _settingsToolkit.ReadLocalSetting(SettingNames.Volume, 100d);
            InitializeTimer();
            this.PropertyChanged += OnPropertyChanged;
            LiveDanmakuCollection.CollectionChanged += OnLiveDanmakuCollectionChanged;
            Controller.LiveMessageReceived += OnLiveMessageReceivedAsync;
            Controller.LoggedOut += OnUserLoggedOut;
        }

        /// <summary>
        /// 媒体播放器数据已更新.
        /// </summary>
        public event EventHandler MediaPlayerUpdated;

        /// <summary>
        /// 数据请求完成时发生.
        /// </summary>
        public event EventHandler Loaded;

        /// <summary>
        /// 保存媒体控件.
        /// </summary>
        /// <param name="playerControl">播放器控件.</param>
        public void ApplyMediaControl(MediaPlayerElement playerControl)
        {
            BiliPlayer = playerControl;
        }

        /// <summary>
        /// 视频加载.
        /// </summary>
        /// <param name="vm">视图模型.</param>
        /// <param name="isRefresh">是否刷新.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task LoadAsync(object vm, bool isRefresh = false)
        {
            var videoId = string.Empty;
            var seasonId = 0;

            if (vm is VideoViewModel videoVM)
            {
                videoId = videoVM.VideoId;
                _videoType = videoVM.VideoType;
            }
            else if (vm is SeasonViewModel seasonVM)
            {
                videoId = seasonVM.EpisodeId.ToString();
                seasonId = seasonVM.SeasonId;
                _videoType = VideoType.Pgc;
            }
            else if (vm is PgcSeason seasonData)
            {
                videoId = "0";
                seasonId = seasonData.SeasonId;
                _videoType = VideoType.Pgc;
            }
            else if (vm is PgcEpisodeDetail episodeData)
            {
                videoId = episodeData.Id.ToString();
                seasonId = 0;
                _videoType = VideoType.Pgc;
            }

            IsDetailCanLoaded = true;
            DanmakuViewModel.Instance.Reset();
            IsPlayInformationError = false;

            switch (_videoType)
            {
                case VideoType.Video:
                    await LoadVideoDetailAsync(videoId, isRefresh);
                    break;
                case VideoType.Pgc:
                    await LoadPgcDetailAsync(Convert.ToInt32(videoId), seasonId, isRefresh);
                    break;
                case VideoType.Live:
                    await LoadLiveDetailAsync(Convert.ToInt32(videoId), isRefresh);
                    break;
                default:
                    break;
            }

            _progressTimer.Start();

            Loaded?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 改变当前分P.
        /// </summary>
        /// <param name="partId">分P Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ChangeVideoPartAsync(long partId)
        {
            if (partId != 0 && VideoPartCollection.Any(p => p.Data.Page.Cid == partId))
            {
                var targetPart = VideoPartCollection.Where(p => p.Data.Page.Cid == partId).FirstOrDefault();
                CurrentVideoPart = targetPart.Data;
            }
            else
            {
                CurrentVideoPart = VideoPartCollection.First().Data;
            }

            CheckPartSelection();

            try
            {
                IsPlayInformationLoading = true;
                var play = await Controller.GetVideoPlayInformationAsync(_videoId, Convert.ToInt64(CurrentVideoPart?.Page.Cid));
                if (play != null)
                {
                    _dashInformation = play;
                }
            }
            catch (Exception ex)
            {
                IsPlayInformationError = true;
                PlayInformationErrorText = _resourceToolkit.GetLocaleString(LanguageNames.RequestVideoFailed) + $"\n{ex.Message}";
            }

            IsPlayInformationLoading = false;

            if (_dashInformation != null)
            {
                ClearPlayer();

                if (_videoDetail.History != null && _videoDetail.History.Cid == partId)
                {
                    _initializeProgress = TimeSpan.FromSeconds(_videoDetail.History.Progress);
                }

                await InitializeVideoPlayInformationAsync(_dashInformation);
                await DanmakuViewModel.Instance.LoadAsync(_videoDetail.Arc.Aid, CurrentVideoPart.Page.Cid);
                ViewerCount = await Controller.GetOnlineViewerCountAsync(Convert.ToInt32(_videoDetail.Arc.Aid), Convert.ToInt32(CurrentVideoPart.Page.Cid));
            }
        }

        /// <summary>
        /// 改变PGC当前分集.
        /// </summary>
        /// <param name="episodeId">分集Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ChangePgcEpisodeAsync(int episodeId)
        {
            if (episodeId != 0 && EpisodeCollection.Any(p => p.Data.Id == episodeId))
            {
                var targetPart = EpisodeCollection.Where(p => p.Data.Id == episodeId).FirstOrDefault();
                CurrentPgcEpisode = targetPart.Data;
            }
            else
            {
                if (PgcSectionCollection.Count > 0)
                {
                    foreach (var section in PgcSectionCollection)
                    {
                        var epi = section.Episodes.FirstOrDefault(p => p.Data.Id == episodeId);
                        if (epi != null)
                        {
                            CurrentPgcEpisode = epi.Data;
                        }
                    }
                }

                if (CurrentPgcEpisode == null)
                {
                    CurrentPgcEpisode = EpisodeCollection.FirstOrDefault()?.Data;
                }
            }

            if (CurrentPgcEpisode == null)
            {
                // 没有分集，弹出警告.
                if (_pgcDetail.Warning != null)
                {
                    IsPlayInformationError = true;
                    PlayInformationErrorText = _pgcDetail.Warning.Message;
                }

                return;
            }

            EpisodeId = CurrentPgcEpisode?.Id.ToString() ?? string.Empty;
            CheckEpisodeSelection();
            ReplyModuleViewModel.Instance.SetInformation(Convert.ToInt32(CurrentPgcEpisode.Aid), Models.Enums.Bili.ReplyType.Video);

            try
            {
                IsPlayInformationLoading = true;
                var play = await Controller.GetPgcPlayInformationAsync(CurrentPgcEpisode.PartId, Convert.ToInt32(CurrentPgcEpisode.Report.SeasonType));
                if (play != null && play.VideoInformation != null)
                {
                    _dashInformation = play;
                }
                else
                {
                    IsPlayInformationError = true;
                    PlayInformationErrorText = _resourceToolkit.GetLocaleString(LanguageNames.RequestPgcFailed);
                }
            }
            catch (Exception ex)
            {
                IsPlayInformationError = true;
                PlayInformationErrorText = _resourceToolkit.GetLocaleString(LanguageNames.RequestPgcFailed) + $"\n{ex.Message}";
            }

            IsPlayInformationLoading = false;

            if (_dashInformation != null)
            {
                ClearPlayer();
                await InitializeVideoPlayInformationAsync(_dashInformation);
                await DanmakuViewModel.Instance.LoadAsync(CurrentPgcEpisode.Aid, CurrentPgcEpisode.PartId);
            }

            try
            {
                ViewerCount = await Controller.GetOnlineViewerCountAsync(CurrentPgcEpisode.Aid, CurrentPgcEpisode.PartId);
                var interaction = await Controller.GetPgcEpisodeInteractionAsync(CurrentPgcEpisode.Id);
                IsLikeChecked = interaction.IsLike != 0;
                IsCoinChecked = interaction.CoinNumber > 0;
                IsFavoriteChecked = interaction.IsFavorite != 0;
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 修改清晰度.
        /// </summary>
        /// <param name="formatId">清晰度Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ChangeFormatAsync(int formatId)
        {
            var preferCodecId = GetPreferCodecId();
            var conditionStreams = _videoList.Where(p => p.Id == formatId).ToList();
            if (conditionStreams.Count == 0)
            {
                var maxQuality = _videoList.Max(p => p.Id);
                _currentVideo = _videoList.Where(p => p.Id == maxQuality).FirstOrDefault();
            }
            else
            {
                var tempVideo = conditionStreams.Where(p => p.CodecId == preferCodecId).FirstOrDefault();
                if (tempVideo == null)
                {
                    tempVideo = conditionStreams.First();
                }

                _currentVideo = tempVideo;
            }

            CurrentFormat = FormatCollection.Where(p => p.Data.Quality == _currentVideo.Id).FirstOrDefault().Data;
            _currentAudio = _audioList.FirstOrDefault();

            CheckFormatSelection();

            await InitializeOnlineDashVideoAsync();
        }

        /// <summary>
        /// 修改直播清晰度.
        /// </summary>
        /// <param name="quality">清晰度.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ChangeLiveQualityAsync(int quality)
        {
            var playInfo = await Controller.GetLivePlayInformationAsync(Convert.ToInt32(RoomId), quality);

            if (playInfo != null)
            {
                await InitializeLivePlayInformationAsync(playInfo);
            }
            else
            {
                IsPlayInformationError = true;
                PlayInformationErrorText = _resourceToolkit.GetLocaleString(LanguageNames.RequestLivePlayInformationFailed);
                CurrentLiveQuality = LiveQualityCollection.FirstOrDefault()?.Data;
            }
        }

        /// <summary>
        /// 修改直播线路.
        /// </summary>
        /// <param name="order">线路序号.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ChangeLivePlayLineAsync(int order)
        {
            var playLine = LivePlayLineCollection.Where(p => p.Data.Order == order).FirstOrDefault()?.Data;
            if (playLine != null)
            {
                CurrentPlayLine = playLine;
                await InitializeLiveDashAsync(CurrentPlayLine.Url);
            }
        }

        /// <summary>
        /// 清理播放数据.
        /// </summary>
        public void ClearPlayer()
        {
            if (BiliPlayer != null)
            {
                BiliPlayer.SetMediaPlayer(null);
            }

            if (_currentVideoPlayer != null)
            {
                if (_currentVideoPlayer.PlaybackSession.CanPause)
                {
                    _currentVideoPlayer.Pause();
                }

                if (_currentPlaybackItem != null)
                {
                    _currentPlaybackItem.Source.Dispose();
                    _currentPlaybackItem = null;
                }

                _currentVideoPlayer.Source = null;
            }

            _lastReportProgress = TimeSpan.Zero;
            _progressTimer.Stop();
            _heartBeatTimer.Stop();

            if (_interopMSS != null)
            {
                _interopMSS.Dispose();
                _interopMSS = null;
            }

            PlayerStatus = PlayerStatus.NotLoad;
        }

        /// <summary>
        /// 切换播放/暂停状态.
        /// </summary>
        public void TogglePlayPause()
        {
            if (_currentVideoPlayer != null)
            {
                if (PlayerStatus == PlayerStatus.Playing)
                {
                    _currentVideoPlayer.Pause();
                }
                else
                {
                    _currentVideoPlayer.Play();
                }
            }
        }

        /// <summary>
        /// 跳转到之前的历史记录.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task JumpToHistoryAsync()
        {
            if (_videoType == VideoType.Video)
            {
                await CheckVideoHistoryAsync();
            }
        }

        /// <summary>
        /// 加载收藏夹列表.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task LoadFavoritesAsync()
        {
            var accVM = AccountViewModel.Instance;
            if (accVM.Status != AccountViewModelStatus.Login || IsRequestingFavorites)
            {
                return;
            }

            try
            {
                IsRequestFavoritesError = false;
                FavoriteMetaCollection.Clear();
                IsRequestingFavorites = true;
                var favorites = await Controller.GetFavoriteListAsync(AccountViewModel.Instance.Mid.Value, Convert.ToInt32(GetAid()));
                if (favorites.Count > 0)
                {
                    favorites.ForEach(p => FavoriteMetaCollection.Add(new FavoriteMetaViewModel(p, p.FavoriteState == 1)));
                }
            }
            catch (Exception)
            {
                IsRequestFavoritesError = true;
            }

            IsRequestingFavorites = false;
        }

        /// <summary>
        /// 使用本机组件分享当前正在播放的内容.
        /// </summary>
        public void Share()
        {
            var dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += OnDataRequested;
            DataTransferManager.ShowShareUI();
        }

        private void OnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var request = args.Request;
            var url = string.Empty;
            switch (_videoType)
            {
                case VideoType.Video:
                    url = _videoDetail.ShortLink;
                    break;
                case VideoType.Pgc:
                    url = CurrentPgcEpisode.Link;
                    break;
                case VideoType.Live:
                    url = $"https://live.bilibili.com/{_liveDetail.RoomInformation.RoomId}";
                    break;
                default:
                    break;
            }

            request.Data.Properties.Title = Title;
            request.Data.Properties.Description = Description;
            request.Data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromUri(new Uri(CoverUrl));
            request.Data.Properties.ContentSourceWebLink = new Uri(url);

            request.Data.SetText(Description);
            request.Data.SetWebLink(new Uri(url));
            request.Data.SetBitmap(RandomAccessStreamReference.CreateFromUri(new Uri(CoverUrl)));
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                case nameof(CurrentFormat):
                    if (CurrentFormat != null)
                    {
                        _settingsToolkit.WriteLocalSetting(SettingNames.DefaultVideoFormat, CurrentFormat.Quality);
                    }

                    break;
                case nameof(Volume):
                    if (Volume > 0)
                    {
                        _settingsToolkit.WriteLocalSetting(SettingNames.Volume, Volume);
                    }

                    break;
                case nameof(IsPlayInformationError):
                case nameof(IsDetailError):
                    PlayerDisplayMode = PlayerDisplayMode.Default;
                    break;
                default:
                    break;
            }
        }

        private void OnLiveDanmakuCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var count = LiveDanmakuCollection.Count;
            IsShowEmptyLiveMessage = count == 0;

            if (count > 0 && IsLiveMessageAutoScroll)
            {
                RequestLiveMessageScrollToBottom?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}

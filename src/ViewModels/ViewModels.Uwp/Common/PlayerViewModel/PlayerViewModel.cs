// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.ViewModels.Uwp.Common;
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
            PartCollection = new ObservableCollection<VideoPartViewModel>();
            FormatCollection = new ObservableCollection<VideoFormatViewModel>();
            _audioList = new List<DashItem>();
            _videoList = new List<DashItem>();
            _lastReportProgress = TimeSpan.Zero;

            ServiceLocator.Instance.LoadService(out _numberToolkit)
                                   .LoadService(out _resourceToolkit)
                                   .LoadService(out _settingsToolkit)
                                   .LoadService(out _fileToolkit);
            PlayerDisplayMode = _settingsToolkit.ReadLocalSetting(SettingNames.DefaultPlayerDisplayMode, PlayerDisplayMode.Default);
            InitializeTimer();
            this.PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        /// 媒体播放器数据已更新.
        /// </summary>
        public event EventHandler MediaPlayerUpdated;

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
        /// <param name="vm">视频视图模型.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task LoadAsync(VideoViewModel vm)
        {
            if (_detail == null || vm.VideoId != AvId)
            {
                IsDetailLoading = true;
                IsDetailError = false;
                _dashInformation = null;
                CurrentFormat = null;
                PartCollection.Clear();
                RelatedVideoCollection.Clear();
                FormatCollection.Clear();
                _audioList.Clear();
                _videoList.Clear();
                ClearPlayer();
                Title = vm.Title;
                _videoId = Convert.ToInt64(vm.VideoId);
                try
                {
                    var detail = await Controller.GetVideoDetailAsync(_videoId);
                    _detail = detail;
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

            var partId = CurrentPart == null ? 0 : CurrentPart.Page.Cid;
            await ChangePartAsync(partId);

            _progressTimer.Start();
        }

        /// <summary>
        /// 改变当前分P.
        /// </summary>
        /// <param name="partId">分P Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ChangePartAsync(long partId)
        {
            if (partId != 0 && PartCollection.Any(p => p.Data.Page.Cid == partId))
            {
                var targetPart = PartCollection.Where(p => p.Data.Page.Cid == partId).FirstOrDefault();
                CurrentPart = targetPart.Data;
            }
            else
            {
                CurrentPart = PartCollection.First().Data;
            }

            CheckPartSelection();

            try
            {
                IsPlayInformationLoading = true;
                var play = await Controller.GetVideoPlayInformationAsync(_videoId, Convert.ToInt64(CurrentPart?.Page.Cid));
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
                await InitializeVideoPlayInformationAsync(_dashInformation);
                await DanmakuViewModel.Instance.LoadAsync(_detail.Arc.Aid, CurrentPart.Page.Cid);
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
        /// 清理播放数据.
        /// </summary>
        public void ClearPlayer()
        {
            BiliPlayer.SetMediaPlayer(null);

            if (_currentVideoPlayer != null)
            {
                if (_currentVideoPlayer.PlaybackSession.CanPause)
                {
                    _currentVideoPlayer.Pause();
                }

                if (_currentVideoPlayer.Source != null)
                {
                    (_currentVideoPlayer.Source as IDisposable).Dispose();
                }

                _currentVideoPlayer.Dispose();
                _currentVideoPlayer = null;
            }

            if (_currentAudioPlayer != null)
            {
                if (_currentAudioPlayer.PlaybackSession.CanPause)
                {
                    _currentAudioPlayer.Pause();
                }

                if (_currentAudioPlayer.Source != null)
                {
                    (_currentAudioPlayer.Source as IDisposable).Dispose();
                }

                _currentAudioPlayer.Dispose();
                _currentAudioPlayer = null;
            }

            _lastReportProgress = TimeSpan.Zero;
            _progressTimer.Stop();
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
                default:
                    break;
            }
        }
    }
}

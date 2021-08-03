// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Bilibili.App.View.V1;
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
            PartCollection = new ObservableCollection<ViewPage>();
            _audioList = new List<DashItem>();
            _videoList = new List<DashItem>();

            ServiceLocator.Instance.LoadService(out _numberToolkit)
                                   .LoadService(out _resourceToolkit)
                                   .LoadService(out _settingsToolkit)
                                   .LoadService(out _fileToolkit);
            CurrentQuality = Convert.ToUInt32(_settingsToolkit.ReadLocalSetting(SettingNames.DefaultVideoQuality, 64));
            PlayerDisplayMode = _settingsToolkit.ReadLocalSetting(SettingNames.DefaultPlayerDisplayMode, PlayerDisplayMode.Default);
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
                PartCollection.Clear();
                RelatedVideoCollection.Clear();
                _audioList.Clear();
                _videoList.Clear();
                ClearPlayer();
                Title = vm.Title;
                try
                {
                    var detail = await Controller.GetVideoDetailAsync(Convert.ToInt64(vm.VideoId));
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

                try
                {
                    IsPlayInformationLoading = true;
                    var play = await Controller.GetVideoPlayInformationAsync(Convert.ToInt64(vm.VideoId), Convert.ToInt64(CurrentPart?.Page.Cid));
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
            }

            if (_dashInformation != null)
            {
                await InitializeVideoPlayInformationAsync(_dashInformation);
                MediaPlayerUpdated?.Invoke(this, EventArgs.Empty);
                await DanmakuViewModel.Instance.LoadAsync(_detail.Arc.Aid, CurrentPart.Page.Cid);
            }
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
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName)
            {
                default:
                    break;
            }
        }
    }
}

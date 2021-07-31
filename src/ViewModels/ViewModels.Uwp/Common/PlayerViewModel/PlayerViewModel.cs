// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Bilibili.App.View.V1;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
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
            _audioList = new System.Collections.Generic.List<DashItem>();
            _streamList = new System.Collections.Generic.List<DashItem>();
            ServiceLocator.Instance.LoadService(out _numberToolkit)
                                   .LoadService(out _resourceToolkit)
                                   .LoadService(out _settingsToolkit)
                                   .LoadService(out _fileToolkit)
                                   .LoadService(out _httpProvider);
            CurrentQuality = Convert.ToUInt32(_settingsToolkit.ReadLocalSetting(SettingNames.DefaultVideoQuality, 64));
            this.PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        /// 保存媒体控件.
        /// </summary>
        /// <param name="playerControl">播放器控件.</param>
        /// <param name="mediaPlayerElement">媒体播放器.</param>
        public void ApplyMediaControl(Control playerControl, MediaPlayerElement mediaPlayerElement)
        {
            MediaPlayerElement = mediaPlayerElement;
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
                PartCollection.Clear();
                RelatedVideoCollection.Clear();
                _audioList.Clear();
                _streamList.Clear();
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
                        await InitializeVideoPlayInformationAsync(play);
                    }
                }
                catch (Exception ex)
                {
                    IsPlayInformationError = true;
                    PlayInformationErrorText = _resourceToolkit.GetLocaleString(LanguageNames.RequestVideoFailed) + $"\n{ex.Message}";
                }

                IsPlayInformationLoading = false;
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

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Bilibili.App.View.V1;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using Windows.Media.Core;

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
                                   .LoadService(out _settingsToolkit);
            CurrentQuality = Convert.ToUInt32(_settingsToolkit.ReadLocalSetting(SettingNames.DefaultVideoQuality, 64));
        }

        /// <summary>
        /// 多媒体源更新.
        /// </summary>
        public event EventHandler<MediaSource> MediaSourceUpdated;

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
            }
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using Windows.System;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// 剧集条目视图模型.
    /// </summary>
    public sealed partial class SeasonItemViewModel : ViewModelBase, IVideoBaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonItemViewModel"/> class.
        /// </summary>
        /// <param name="numberToolkit">数字转换工具.</param>
        /// <param name="navigationViewModel">导航服务.</param>
        public SeasonItemViewModel(
            INumberToolkit numberToolkit,
            NavigationViewModel navigationViewModel)
        {
            _numberToolkit = numberToolkit;
            _navigationViewModel = navigationViewModel;

            OpenInBroswerCommand = ReactiveCommand.CreateFromTask(OpenInBroswerAsync, outputScheduler: RxApp.MainThreadScheduler);
            PlayCommand = ReactiveCommand.Create(Play, outputScheduler: RxApp.MainThreadScheduler);
        }

        /// <summary>
        /// 设置单集信息，并进行视图模型的初始化.
        /// </summary>
        /// <param name="information">单集信息.</param>
        public void SetInformation(IVideoBase information)
        {
            Information = information as SeasonInformation;
            InitializeData();
        }

        private void InitializeData()
        {
            IsShowRating = Information.CommunityInformation?.Score > 0;

            if (Information.CommunityInformation?.TrackCount > 0)
            {
                TrackCountText = _numberToolkit.GetCountText(Information.CommunityInformation.TrackCount);
            }
        }

        private void Play()
            => _navigationViewModel.NavigateToPlayView(Information);

        private async Task OpenInBroswerAsync()
        {
            var uri = $"https://www.bilibili.com/bangumi/play/ss{Information.Identifier.Id}";
            await Launcher.LaunchUriAsync(new Uri(uri));
        }
    }
}

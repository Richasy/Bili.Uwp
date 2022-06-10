// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Models.Data.Live;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using Windows.System;

namespace Bili.ViewModels.Uwp.Live
{
    /// <summary>
    /// 直播条目视图模型.
    /// </summary>
    public sealed partial class LiveItemViewModel : ViewModelBase, IVideoBaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveItemViewModel"/> class.
        /// </summary>
        /// <param name="numberToolkit">数字转换工具.</param>
        /// <param name="navigationViewModel">导航模块.</param>
        public LiveItemViewModel(
            INumberToolkit numberToolkit,
            NavigationViewModel navigationViewModel)
        {
            _numberToolkit = numberToolkit;
            _navigationViewModel = navigationViewModel;

            PlayCommand = ReactiveCommand.Create(Play, outputScheduler: RxApp.MainThreadScheduler);
            OpenInBroswerCommand = ReactiveCommand.CreateFromTask(OpenInBroswerAsync, outputScheduler: RxApp.MainThreadScheduler);
        }

        /// <inheritdoc/>
        public void SetInformation(IVideoBase information)
        {
            Information = information as LiveInformation;
            ViewerCountText = _numberToolkit.GetCountText(Information.ViewerCount);
        }

        private void Play()
            => _navigationViewModel.NavigateToPlayView(new Models.Data.Local.PlaySnapshot(Information.Identifier.Id, default, Models.Enums.VideoType.Live));

        private async Task OpenInBroswerAsync()
        {
            var uri = $"https://live.bilibili.com/{Information.Identifier.Id}";
            await Launcher.LaunchUriAsync(new Uri(uri));
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Models.Data.Live;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Live;
using ReactiveUI;
using Windows.System;

namespace Bili.ViewModels.Uwp.Live
{
    /// <summary>
    /// 直播条目视图模型.
    /// </summary>
    public sealed partial class LiveItemViewModel : ViewModelBase, ILiveItemViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveItemViewModel"/> class.
        /// </summary>
        /// <param name="numberToolkit">数字转换工具.</param>
        /// <param name="navigationViewModel">导航模块.</param>
        public LiveItemViewModel(
            INumberToolkit numberToolkit,
            INavigationViewModel navigationViewModel)
        {
            _numberToolkit = numberToolkit;
            _navigationViewModel = navigationViewModel;

            PlayCommand = new RelayCommand(Play);
            OpenInBroswerCommand = new AsyncRelayCommand(OpenInBroswerAsync);
        }

        /// <inheritdoc/>
        public void InjectData(LiveInformation information)
        {
            Data = information;
            ViewerCountText = _numberToolkit.GetCountText(Data.ViewerCount);
        }

        private void Play()
            => _navigationViewModel.NavigateToPlayView(new Models.Data.Local.PlaySnapshot(Data.Identifier.Id, default, Models.Enums.VideoType.Live));

        private async Task OpenInBroswerAsync()
        {
            var uri = $"https://live.bilibili.com/{Data.Identifier.Id}";
            await Launcher.LaunchUriAsync(new Uri(uri));
        }
    }
}

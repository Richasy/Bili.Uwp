// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using ReactiveUI;
using Windows.System;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// 剧集单集视图模型.
    /// </summary>
    public sealed partial class EpisodeItemViewModel : ViewModelBase, IVideoBaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodeItemViewModel"/> class.
        /// </summary>
        /// <param name="numberToolkit">数字转换工具.</param>
        public EpisodeItemViewModel(
            INumberToolkit numberToolkit)
        {
            _numberToolkit = numberToolkit;

            OpenInBroswerCommand = ReactiveCommand.CreateFromTask(OpenInBroswerAsync, outputScheduler: RxApp.MainThreadScheduler);
            PlayCommand = ReactiveCommand.Create(Play, outputScheduler: RxApp.MainThreadScheduler);
        }

        /// <summary>
        /// 设置单集信息，并进行视图模型的初始化.
        /// </summary>
        /// <param name="information">单集信息.</param>
        public void SetInformation(IVideoBase information)
        {
            Information = information as EpisodeInformation;
            InitializeData();
        }

        private void InitializeData()
        {
            PlayCountText = _numberToolkit.GetCountText(Information.CommunityInformation.PlayCount);
            DanmakuCountText = _numberToolkit.GetCountText(Information.CommunityInformation.DanmakuCount);
            IsShowBadge = !string.IsNullOrEmpty(Information.Highlight);
        }

        private void Play()
            => AppViewModel.Instance.OpenPlayer(Information);

        private async Task OpenInBroswerAsync()
        {
            var uri = $"https://www.bilibili.com/bangumi/play/ep{Information.Identifier.Id}";
            await Launcher.LaunchUriAsync(new Uri(uri));
        }
    }
}

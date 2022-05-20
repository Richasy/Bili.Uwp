// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Reactive;
using Bili.Models.Data.Live;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Live
{
    /// <summary>
    /// 直播间条目视图模型.
    /// </summary>
    public sealed partial class LiveItemViewModel
    {
        private readonly INumberToolkit _numberToolkit;
        private readonly NavigationViewModel _navigationViewModel;

        /// <summary>
        /// 在网页中打开的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> OpenInBroswerCommand { get; }

        /// <summary>
        /// 播放命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> PlayCommand { get; }

        /// <summary>
        /// 视频信息.
        /// </summary>
        [Reactive]
        public LiveInformation Information { get; internal set; }

        /// <summary>
        /// 观看人数的可读文本.
        /// </summary>
        [Reactive]
        public string ViewerCountText { get; internal set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is LiveItemViewModel model && EqualityComparer<LiveInformation>.Default.Equals(Information, model.Information);

        /// <inheritdoc/>
        public override int GetHashCode() => Information.GetHashCode();
    }
}

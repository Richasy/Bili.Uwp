// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Reactive;
using Bili.Models.Data.Pgc;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// 剧集单集视图模型.
    /// </summary>
    public sealed partial class EpisodeItemViewModel
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
        /// 剧集单集信息.
        /// </summary>
        [Reactive]
        public EpisodeInformation Information { get; internal set; }

        /// <summary>
        /// 播放次数的可读文本.
        /// </summary>
        [Reactive]
        public string PlayCountText { get; internal set; }

        /// <summary>
        /// 播放次数的可读文本.
        /// </summary>
        [Reactive]
        public string DanmakuCountText { get; internal set; }

        /// <summary>
        /// 追番/追剧次数的可读文本.
        /// </summary>
        [Reactive]
        public string TrackCountText { get; internal set; }

        /// <summary>
        /// 是否被选中.
        /// </summary>
        [Reactive]
        public bool IsSelected { get; set; }

        /// <summary>
        /// 时长的可读文本.
        /// </summary>
        [Reactive]
        public string DurationText { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is EpisodeItemViewModel model && EqualityComparer<EpisodeInformation>.Default.Equals(Information, model.Information);

        /// <inheritdoc/>
        public override int GetHashCode() => Information.GetHashCode();
    }
}

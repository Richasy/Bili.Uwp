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
    /// 剧集条目视图模型.
    /// </summary>
    public sealed partial class SeasonItemViewModel
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
        public SeasonInformation Information { get; internal set; }

        /// <summary>
        /// 是否被选中.
        /// </summary>
        [Reactive]
        public bool IsSelected { get; set; }

        /// <summary>
        /// 是否显示评分.
        /// </summary>
        [Reactive]
        public bool IsShowRating { get; set; }

        /// <summary>
        /// 追番次数文本.
        /// </summary>
        [Reactive]
        public string TrackCountText { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SeasonItemViewModel model && EqualityComparer<SeasonInformation>.Default.Equals(Information, model.Information);

        /// <inheritdoc/>
        public override int GetHashCode() => Information.GetHashCode();
    }
}

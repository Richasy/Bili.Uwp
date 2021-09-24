// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 热门视图模型的属性集.
    /// </summary>
    public partial class PopularViewModel
    {
        private int _offsetIndex;

        /// <summary>
        /// <see cref="PopularViewModel"/>的静态实例.
        /// </summary>
        public static PopularViewModel Instance { get; } = new Lazy<PopularViewModel>(() => new PopularViewModel()).Value;

        /// <summary>
        /// 视频集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<VideoViewModel> VideoCollection { get; set; }
    }
}

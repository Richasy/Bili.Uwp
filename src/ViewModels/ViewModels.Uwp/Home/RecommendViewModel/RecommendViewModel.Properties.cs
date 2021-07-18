// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 视频推荐视图模型.
    /// </summary>
    public partial class RecommendViewModel
    {
        private int _offsetIndex;

        /// <summary>
        /// <see cref="RecommendViewModel"/>的静态实例.
        /// </summary>
        public static RecommendViewModel Instance { get; } = new Lazy<RecommendViewModel>(() => new RecommendViewModel()).Value;

        /// <summary>
        /// 视频集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<VideoViewModel> VideoCollection { get; set; }
    }
}

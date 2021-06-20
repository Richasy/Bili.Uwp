// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 视频推荐视图模型.
    /// </summary>
    public partial class RecommendViewModel
    {
        private readonly BiliController _controller;
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

        /// <summary>
        /// 是否正在进行初始化加载.
        /// </summary>
        [Reactive]
        public bool IsInitializeLoading { get; set; }

        /// <summary>
        /// 是否正在进行增量加载.
        /// </summary>
        [Reactive]
        public bool IsDeltaLoading { get; set; }

        /// <summary>
        /// 是否显示错误提示.
        /// </summary>
        [Reactive]
        public bool IsError { get; set; }
    }
}

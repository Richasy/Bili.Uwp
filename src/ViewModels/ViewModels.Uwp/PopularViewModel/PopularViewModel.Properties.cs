// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 热门视图模型的属性集.
    /// </summary>
    public partial class PopularViewModel
    {
        private readonly BiliController _controller;
        private readonly IResourceToolkit _resourceToolkit;
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

        /// <summary>
        /// 错误文本.
        /// </summary>
        [Reactive]
        public string ErrorText { get; set; }
    }
}

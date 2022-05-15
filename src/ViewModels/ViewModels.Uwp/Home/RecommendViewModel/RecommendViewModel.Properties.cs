// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 视频推荐视图模型.
    /// </summary>
    public partial class RecommendViewModel
    {
        private readonly IRecommendProvider _recommendProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly ObservableAsPropertyHelper<bool> _isInitializing;
        private readonly ObservableAsPropertyHelper<bool> _isIncrementalLoading;

        /// <summary>
        /// <see cref="RecommendViewModel"/>的静态实例.
        /// </summary>
        public static RecommendViewModel Instance { get; } = new Lazy<RecommendViewModel>(() => new RecommendViewModel()).Value;

        /// <summary>
        /// 初始化命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> InitializeCommand { get; }

        /// <summary>
        /// 增量请求命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> IncrementalCommand { get; }

        /// <summary>
        /// 视频集合.
        /// </summary>
        public ObservableCollection<IVideoBaseViewModel> VideoCollection { get; }

        /// <summary>
        /// 是否正在初始化.
        /// </summary>
        public bool IsInitializing => _isInitializing?.Value ?? false;

        /// <summary>
        /// 是否正在增量加载.
        /// </summary>
        public bool IsIncrementalLoading => _isIncrementalLoading?.Value ?? false;

        /// <summary>
        /// 是否出错.
        /// </summary>
        [Reactive]
        public bool IsError { get; private set; }

        /// <summary>
        /// 错误文本.
        /// </summary>
        [Reactive]
        public string ErrorText { get; set; }
    }
}

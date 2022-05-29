// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Base
{
    /// <summary>
    /// 信息流视图模型基类，支持重载和增量加载.
    /// </summary>
    /// <typeparam name="T">核心数据集合的类型.</typeparam>
    public partial class InformationFlowViewModelBase<T>
    {
        private readonly CoreDispatcher _dispatcher;
        private readonly ObservableAsPropertyHelper<bool> _isReloading;
        private readonly ObservableAsPropertyHelper<bool> _isIncrementalLoading;
        private bool _isNeedLoadAgain;

        /// <summary>
        /// 初始化命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> InitializeCommand { get; }

        /// <summary>
        /// 重新加载命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <summary>
        /// 增量请求命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> IncrementalCommand { get; }

        /// <summary>
        /// 视频集合.
        /// </summary>
        public ObservableCollection<T> Items { get; }

        /// <summary>
        /// 是否正在初始化.
        /// </summary>
        public bool IsReloading => _isReloading?.Value ?? false;

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

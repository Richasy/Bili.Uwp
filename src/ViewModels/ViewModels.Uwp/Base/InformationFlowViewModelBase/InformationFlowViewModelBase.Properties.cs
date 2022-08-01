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
        private bool _isNeedLoadAgain;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> InitializeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> IncrementalCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<T> Items { get; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsReloading { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsIncrementalLoading { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsError { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string ErrorText { get; set; }
    }
}

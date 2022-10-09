// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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

        [ObservableProperty]
        private bool _isReloading;

        [ObservableProperty]
        private bool _isIncrementalLoading;

        [ObservableProperty]
        private bool _isError;

        [ObservableProperty]
        private string _errorText;

        /// <inheritdoc/>
        public IAsyncRelayCommand InitializeCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand IncrementalCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<T> Items { get; }
    }
}

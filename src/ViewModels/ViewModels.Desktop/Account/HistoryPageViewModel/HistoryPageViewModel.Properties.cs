// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Account
{
    /// <summary>
    /// 历史记录页面视图模型.
    /// </summary>
    public sealed partial class HistoryPageViewModel
    {
        private readonly IAccountProvider _accountProvider;
        private readonly IResourceToolkit _resourceToolkit;

        private bool _isEnd;

        [ObservableProperty]
        private bool _isEmpty;

        /// <inheritdoc/>
        public IAsyncRelayCommand ClearCommand { get; }
    }
}

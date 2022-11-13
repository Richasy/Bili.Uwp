// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Account
{
    /// <summary>
    /// 稍后再看页面视图模型.
    /// </summary>
    public sealed partial class ViewLaterPageViewModel
    {
        private readonly IAccountProvider _accountProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly INavigationViewModel _navigationViewModel;

        private bool _isEnd;

        [ObservableProperty]
        private bool _isEmpty;

        [ObservableProperty]
        private bool _isClearing;

        /// <inheritdoc/>
        public IRelayCommand PlayAllCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand ClearCommand { get; }
    }
}

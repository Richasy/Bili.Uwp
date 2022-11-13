// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.WinUI.Connectivity;
using Microsoft.UI.Dispatching;

namespace Bili.ViewModels.Desktop.Core
{
    /// <summary>
    /// <see cref="AppViewModel"/>的属性集.
    /// </summary>
    public sealed partial class AppViewModel
    {
        private readonly IResourceToolkit _resourceToolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly IAppToolkit _appToolkit;
        private readonly IUpdateProvider _updateProvider;
        private readonly ICallerViewModel _callerViewModel;
        private readonly INavigationViewModel _navigationViewModel;
        private readonly NetworkHelper _networkHelper;
        private readonly DispatcherQueue _dispatcherQueue;

        private bool? _isWide;

        [ObservableProperty]
        private bool _isNavigatePaneOpen;

        [ObservableProperty]
        private string _headerText;

        [ObservableProperty]
        private bool _isXbox;

        [ObservableProperty]
        private double _pageHorizontalPadding;

        [ObservableProperty]
        private double _pageTopPadding;

        [ObservableProperty]
        private bool _isShowTitleBar;

        [ObservableProperty]
        private bool _isShowMenuButton;

        [ObservableProperty]
        private bool _isNetworkAvaliable;

        [ObservableProperty]
        private bool _isTraditionalChinese;

        /// <inheritdoc/>
        public object MainWindow { get; set; }

        /// <inheritdoc/>
        public object AppWindow { get; set; }

        /// <inheritdoc/>
        public IntPtr MainWindowHandle { get; set; }

        /// <inheritdoc/>
        public IAsyncRelayCommand CheckUpdateCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand CheckNewDynamicRegistrationCommand { get; }
    }
}

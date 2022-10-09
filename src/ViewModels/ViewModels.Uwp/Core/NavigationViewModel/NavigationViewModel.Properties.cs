// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Bili.Models.App.Args;
using Bili.Models.Enums;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 导航视图模型.
    /// </summary>
    public sealed partial class NavigationViewModel
    {
        private readonly IRecordViewModel _recordViewModel;
        private readonly List<AppBackEventArgs> _backStack;

        [ObservableProperty]
        private bool _isMainViewShown;

        [ObservableProperty]
        private bool _isSecondaryViewShown;

        [ObservableProperty]
        private bool _isPlayViewShown;

        [ObservableProperty]
        private bool _canBack;

        [ObservableProperty]
        private bool _isBackButtonEnabled;

        /// <inheritdoc/>
        public event EventHandler<AppNavigationEventArgs> Navigating;

        /// <inheritdoc/>
        public event EventHandler ExitPlayer;

        /// <inheritdoc/>
        public IRelayCommand BackCommand { get; }

        /// <inheritdoc/>
        public PageIds MainViewId { get; set; }

        /// <inheritdoc/>
        public PageIds SecondaryViewId { get; set; }

        /// <inheritdoc/>
        public PageIds PlayViewId { get; set; }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reactive;
using Bili.Models.App.Args;
using Bili.Models.Enums;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 导航视图模型.
    /// </summary>
    public sealed partial class NavigationViewModel
    {
        private readonly IRecordViewModel _recordViewModel;
        private readonly List<AppBackEventArgs> _backStack;

        /// <inheritdoc/>
        public event EventHandler<AppNavigationEventArgs> Navigating;

        /// <inheritdoc/>
        public event EventHandler ExitPlayer;

        /// <inheritdoc/>
        public IRelayCommand BackCommand { get; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsMainViewShown { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsSecondaryViewShown { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsPlayViewShown { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool CanBack { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsBackButtonEnabled { get; set; }

        /// <inheritdoc/>
        public PageIds MainViewId { get; set; }

        /// <inheritdoc/>
        public PageIds SecondaryViewId { get; set; }

        /// <inheritdoc/>
        public PageIds PlayViewId { get; set; }
    }
}

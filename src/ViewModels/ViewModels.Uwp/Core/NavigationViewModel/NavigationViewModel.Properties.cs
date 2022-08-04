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
        public ReactiveCommand<Unit, Unit> BackCommand { get; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsMainViewShown { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsSecondaryViewShown { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsPlayViewShown { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool CanBack { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsBackButtonEnabled { get; set; }

        /// <inheritdoc/>
        public PageIds MainViewId { get; set; }

        /// <inheritdoc/>
        public PageIds SecondaryViewId { get; set; }

        /// <inheritdoc/>
        public PageIds PlayViewId { get; set; }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reactive;
using Bili.Models.App.Args;
using Bili.Models.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Common
{
    /// <summary>
    /// 导航视图模型.
    /// </summary>
    public sealed partial class NavigationViewModel
    {
        private readonly List<AppBackEventArgs> _backStack;

        /// <inheritdoc/>
        public event EventHandler<AppNavigationEventArgs> Navigating;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> BackCommand { get; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsMainViewShown { get; internal set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsSecondaryViewShown { get; internal set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsPlayViewShown { get; internal set; }

        /// <inheritdoc/>
        [Reactive]
        public bool CanBack { get; set; }

        /// <inheritdoc/>
        public PageIds MainViewId { get; private set; }

        /// <inheritdoc/>
        public PageIds SecondaryViewId { get; private set; }
    }
}

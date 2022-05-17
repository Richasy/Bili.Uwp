// Copyright (c) Richasy. All rights reserved.

using System;
using System.Reactive;
using Bili.Models.App.Args;
using DynamicData;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Common
{
    /// <summary>
    /// 导航视图模型.
    /// </summary>
    public sealed partial class NavigationViewModel
    {
        private readonly SourceList<AppNavigationEventArgs> _history;

        private readonly ObservableAsPropertyHelper<bool> _canBack;

        /// <inheritdoc/>
        public event EventHandler<AppNavigationEventArgs> Navigating;

        /// <inheritdoc/>
        public event EventHandler<AppBackEventArgs> Backing;

        /// <inheritdoc/>
        public ReactiveCommand<object, Unit> BackCommand { get; }

        /// <summary>
        /// 是否显示主视图.
        /// </summary>
        [Reactive]
        public bool IsMainViewShown { get; internal set; }

        /// <summary>
        /// 是否显示二级页面.
        /// </summary>
        [Reactive]
        public bool IsSecondaryViewShown { get; internal set; }

        /// <summary>
        /// 是否显示播放页面.
        /// </summary>
        [Reactive]
        public bool IsPlayViewShown { get; internal set; }

        /// <inheritdoc/>
        public bool CanBack => _canBack.Value;
    }
}

// Copyright (c) Richasy. All rights reserved.

using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Account
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

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> PlayAllCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsEmpty { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsClearing { get; set; }
    }
}

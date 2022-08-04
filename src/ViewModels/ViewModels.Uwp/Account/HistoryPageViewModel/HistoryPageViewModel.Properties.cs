// Copyright (c) Richasy. All rights reserved.

using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Account
{
    /// <summary>
    /// 历史记录页面视图模型.
    /// </summary>
    public sealed partial class HistoryPageViewModel
    {
        private readonly IAccountProvider _accountProvider;
        private readonly IResourceToolkit _resourceToolkit;

        private bool _isEnd;

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

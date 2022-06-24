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
        private readonly ObservableAsPropertyHelper<bool> _isClearing;

        private bool _isEnd;

        /// <summary>
        /// 清空全部命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }

        /// <summary>
        /// 稍后再看列表是否为空.
        /// </summary>
        [Reactive]
        public bool IsEmpty { get; set; }

        /// <summary>
        /// 是否正在清空内容.
        /// </summary>
        public bool IsClearing => _isClearing.Value;
    }
}

// Copyright (c) Richasy. All rights reserved.

using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using Bili.ViewModels.Uwp.Video;
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
        private readonly NavigationViewModel _navigationViewModel;

        private readonly ObservableAsPropertyHelper<bool> _isClearing;

        private bool _isEnd;

        /// <summary>
        /// 播放全部命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> PlayAllCommand { get; }

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

// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 消息页面视图模型.
    /// </summary>
    public sealed partial class MessagePageViewModel
    {
        private readonly IAccountProvider _accountProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly AppViewModel _appViewModel;
        private readonly AccountViewModel _accountViewModel;
        private readonly Dictionary<MessageType, (IEnumerable<MessageInformation> Items, bool IsEnd)> _caches;

        private bool _isEnd;
        private bool _shouldClearCache;

        /// <summary>
        /// 消息类型集合.
        /// </summary>
        public ObservableCollection<MessageHeaderViewModel> MessageTypes { get; }

        /// <summary>
        /// 当前选中的消息类型.
        /// </summary>
        [Reactive]
        public MessageHeaderViewModel CurrentType { get; set; }

        /// <summary>
        /// 选择消息类型命令.
        /// </summary>
        public ReactiveCommand<MessageHeaderViewModel, Unit> SelectTypeCommand { get; }

        /// <summary>
        /// 是否为空.
        /// </summary>
        [Reactive]
        public bool IsEmpty { get; set; }
    }
}

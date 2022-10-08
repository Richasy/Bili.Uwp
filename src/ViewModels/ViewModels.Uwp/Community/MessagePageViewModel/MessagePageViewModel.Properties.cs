// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Community;
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
        private readonly IAccountViewModel _accountViewModel;
        private readonly Dictionary<MessageType, (IEnumerable<MessageInformation> Items, bool IsEnd)> _caches;

        private bool _isEnd;
        private bool _shouldClearCache;

        /// <summary>
        /// 选择消息类型命令.
        /// </summary>
        public IRelayCommand<IMessageHeaderViewModel> SelectTypeCommand { get; }

        /// <summary>
        /// 消息类型集合.
        /// </summary>
        public ObservableCollection<IMessageHeaderViewModel> MessageTypes { get; }

        /// <summary>
        /// 当前选中的消息类型.
        /// </summary>
        [ObservableProperty]
        public IMessageHeaderViewModel CurrentType { get; set; }

        /// <summary>
        /// 是否为空.
        /// </summary>
        [ObservableProperty]
        public bool IsEmpty { get; set; }
    }
}

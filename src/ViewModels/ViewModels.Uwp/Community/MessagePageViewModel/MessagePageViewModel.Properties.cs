// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Community;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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
        /// 当前选中的消息类型.
        /// </summary>
        [ObservableProperty]
        private IMessageHeaderViewModel _currentType;

        /// <summary>
        /// 是否为空.
        /// </summary>
        [ObservableProperty]
        private bool _isEmpty;

        /// <summary>
        /// 选择消息类型命令.
        /// </summary>
        public IAsyncRelayCommand<IMessageHeaderViewModel> SelectTypeCommand { get; }

        /// <summary>
        /// 消息类型集合.
        /// </summary>
        public ObservableCollection<IMessageHeaderViewModel> MessageTypes { get; }
    }
}

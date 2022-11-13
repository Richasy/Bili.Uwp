// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Data.Local;
using Bili.Models.Data.User;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;

namespace Bili.ViewModels.Desktop.Account
{
    /// <summary>
    /// 用户视图模型属性集.
    /// </summary>
    public sealed partial class AccountViewModel
    {
        private readonly IResourceToolkit _resourceToolkit;
        private readonly INumberToolkit _numberToolkit;
        private readonly IFileToolkit _fileToolkit;
        private readonly IAuthorizeProvider _authorizeProvider;
        private readonly IAccountProvider _accountProvider;
        private readonly DispatcherQueue _dispatcherQueue;

        private bool _isRequestLogout = false;

        /// <summary>
        /// 当前视图模型状态.
        /// </summary>
        [ObservableProperty]
        private AuthorizeState _state;

        /// <summary>
        /// 头像.
        /// </summary>
        [ObservableProperty]
        private string _avatar;

        /// <summary>
        /// 显示名称.
        /// </summary>
        [ObservableProperty]
        private string _displayName;

        /// <summary>
        /// 等级.
        /// </summary>
        [ObservableProperty]
        private int _level;

        /// <summary>
        /// 工具提示及自动化文本.
        /// </summary>
        [ObservableProperty]
        private string _tipText;

        /// <summary>
        /// 是否为大会员.
        /// </summary>
        [ObservableProperty]
        private bool _isVip;

        /// <summary>
        /// 动态数.
        /// </summary>
        [ObservableProperty]
        private string _dynamicCount;

        /// <summary>
        /// 粉丝数.
        /// </summary>
        [ObservableProperty]
        private string _followerCount;

        /// <summary>
        /// 关注人数.
        /// </summary>
        [ObservableProperty]
        private string _followCount;

        /// <summary>
        /// 是否已经完成了与账户的连接.
        /// </summary>
        [ObservableProperty]
        private bool _isConnected;

        /// <summary>
        /// 未读提及数.
        /// </summary>
        [ObservableProperty]
        private UnreadInformation _unreadInformation;

        /// <summary>
        /// 是否显示未读消息数.
        /// </summary>
        [ObservableProperty]
        private bool _isShowUnreadMessage;

        /// <summary>
        /// 是否显示固定的内容.
        /// </summary>
        [ObservableProperty]
        private bool _isShowFixedItem;

        /// <summary>
        /// 用户信息.
        /// </summary>
        public AccountInformation AccountInformation { get; internal set; }

        /// <summary>
        /// 登录用户Id.
        /// </summary>
        public int? Mid => Convert.ToInt32(AccountInformation?.User.Id);

        /// <summary>
        /// 固定条目集合.
        /// </summary>
        public ObservableCollection<FixedItem> FixedItemCollection { get; }

        /// <summary>
        /// 尝试登录的命令.
        /// </summary>
        public IAsyncRelayCommand<bool> TrySignInCommand { get; }

        /// <summary>
        /// 登出命令.
        /// </summary>
        public IAsyncRelayCommand SignOutCommand { get; }

        /// <summary>
        /// 加载个人资料的命令.
        /// </summary>
        public IAsyncRelayCommand LoadMyProfileCommand { get; }

        /// <summary>
        /// 初始化社区信息的命令.
        /// </summary>
        public IAsyncRelayCommand InitializeCommunityCommand { get; }

        /// <summary>
        /// 初始化未读消息的命令.
        /// </summary>
        public IAsyncRelayCommand InitializeUnreadCommand { get; }

        /// <summary>
        /// 添加固定条目的命令.
        /// </summary>
        public IAsyncRelayCommand<FixedItem> AddFixedItemCommand { get; }

        /// <summary>
        /// 移除固定条目的命令.
        /// </summary>
        public IAsyncRelayCommand<string> RemoveFixedItemCommand { get; }
    }
}

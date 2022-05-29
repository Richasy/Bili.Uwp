// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.App;
using Bili.Models.Data.Community;
using Bili.Models.Data.User;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Account
{
    /// <summary>
    /// 用户试图模型属性集.
    /// </summary>
    public partial class AccountViewModel
    {
        private readonly IResourceToolkit _resourceToolkit;
        private readonly INumberToolkit _numberToolkit;
        private readonly IFileToolkit _fileToolkit;
        private readonly IAuthorizeProvider _authorizeProvider;
        private readonly IAccountProvider _accountProvider;
        private readonly AppViewModel _appViewModel;

        private AccountInformation _accountInformation;
        private bool _isRequestLogout = false;

        /// <summary>
        /// 登录用户Id.
        /// </summary>
        public int? Mid => Convert.ToInt32(_accountInformation?.User.Id);

        /// <summary>
        /// 固定条目集合.
        /// </summary>
        public ObservableCollection<FixedItem> FixedItemCollection { get; }

        /// <summary>
        /// 当前视图模型状态.
        /// </summary>
        [Reactive]
        public AuthorizeState State { get; set; }

        /// <summary>
        /// 头像.
        /// </summary>
        [Reactive]
        public string Avatar { get; set; }

        /// <summary>
        /// 显示名称.
        /// </summary>
        [Reactive]
        public string DisplayName { get; set; }

        /// <summary>
        /// 等级.
        /// </summary>
        [Reactive]
        public int Level { get; set; }

        /// <summary>
        /// 工具提示及自动化文本.
        /// </summary>
        [Reactive]
        public string TipText { get; set; }

        /// <summary>
        /// 是否为大会员.
        /// </summary>
        [Reactive]
        public bool IsVip { get; set; }

        /// <summary>
        /// 动态数.
        /// </summary>
        [Reactive]
        public string DynamicCount { get; set; }

        /// <summary>
        /// 粉丝数.
        /// </summary>
        [Reactive]
        public string FollowerCount { get; set; }

        /// <summary>
        /// 关注人数.
        /// </summary>
        [Reactive]
        public string FollowCount { get; set; }

        /// <summary>
        /// 是否已经完成了与账户的连接.
        /// </summary>
        [Reactive]
        public bool IsConnected { get; set; }

        /// <summary>
        /// 未读提及数.
        /// </summary>
        [Reactive]
        public UnreadInformation UnreadInformation { get; set; }

        /// <summary>
        /// 是否显示未读消息数.
        /// </summary>
        [Reactive]
        public bool IsShowUnreadMessage { get; set; }

        /// <summary>
        /// 是否显示固定的内容.
        /// </summary>
        [Reactive]
        public bool IsShowFixedItem { get; set; }
    }
}

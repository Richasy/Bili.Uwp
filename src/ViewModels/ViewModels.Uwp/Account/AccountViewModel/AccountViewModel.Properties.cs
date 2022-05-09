// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Controller.Uwp;
using Bili.Models.App;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 当前用户视图模型状态.
    /// </summary>
    public enum AccountViewModelStatus
    {
        /// <summary>
        /// 用户已登出.
        /// </summary>
        Logout,

        /// <summary>
        /// 用户已登录.
        /// </summary>
        Login,

        /// <summary>
        /// 用户正在登录.
        /// </summary>
        Logging,
    }

    /// <summary>
    /// 用户试图模型属性集.
    /// </summary>
    public partial class AccountViewModel
    {
        private readonly BiliController _controller;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly INumberToolkit _numberToolkit;
        private readonly IFileToolkit _fileToolkit;
        private AccountInformation _accountInformation;

        /// <summary>
        /// <see cref="AccountViewModel"/>的实例.
        /// </summary>
        public static AccountViewModel Instance { get; } = new Lazy<AccountViewModel>(() => new AccountViewModel()).Value;

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
        public AccountViewModelStatus Status { get; set; }

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
        /// 未读消息数.
        /// </summary>
        [Reactive]
        public int UnreadMessageCount { get; set; }

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

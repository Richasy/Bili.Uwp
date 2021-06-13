// Copyright (c) Richasy. All rights reserved.

using System;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.ViewModels.Uwp
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

        /// <summary>
        /// <see cref="AccountViewModel"/>的实例.
        /// </summary>
        public static AccountViewModel Instance { get; } = new Lazy<AccountViewModel>(() => new AccountViewModel()).Value;

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
        /// 工具提示及自动化文本.
        /// </summary>
        [Reactive]
        public string TipText { get; set; }
    }
}

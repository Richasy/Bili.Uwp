// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.ComponentModel;
using Bili.Models.Data.Community;
using Bili.Models.Data.Local;
using Bili.Models.Data.User;
using Bili.Models.Enums;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Account
{
    /// <summary>
    /// 登录用户的视图模型接口定义.
    /// </summary>
    public interface IAccountViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 用户信息.
        /// </summary>
        AccountInformation AccountInformation { get; }

        /// <summary>
        /// 登录用户Id.
        /// </summary>
        int? Mid { get; }

        /// <summary>
        /// 固定条目集合.
        /// </summary>
        ObservableCollection<FixedItem> FixedItemCollection { get; }

        /// <summary>
        /// 尝试登录的命令.
        /// </summary>
        IAsyncRelayCommand<bool> TrySignInCommand { get; }

        /// <summary>
        /// 登出命令.
        /// </summary>
        IAsyncRelayCommand SignOutCommand { get; }

        /// <summary>
        /// 加载个人资料的命令.
        /// </summary>
        IAsyncRelayCommand LoadMyProfileCommand { get; }

        /// <summary>
        /// 初始化社区信息的命令.
        /// </summary>
        IAsyncRelayCommand InitializeCommunityCommand { get; }

        /// <summary>
        /// 初始化未读消息的命令.
        /// </summary>
        IAsyncRelayCommand InitializeUnreadCommand { get; }

        /// <summary>
        /// 添加固定条目的命令.
        /// </summary>
        IAsyncRelayCommand<FixedItem> AddFixedItemCommand { get; }

        /// <summary>
        /// 移除固定条目的命令.
        /// </summary>
        IAsyncRelayCommand<string> RemoveFixedItemCommand { get; }

        /// <summary>
        /// 当前视图模型状态.
        /// </summary>
        AuthorizeState State { get; }

        /// <summary>
        /// 头像.
        /// </summary>
        string Avatar { get; }

        /// <summary>
        /// 显示名称.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// 等级.
        /// </summary>
        int Level { get; }

        /// <summary>
        /// 工具提示及自动化文本.
        /// </summary>
        string TipText { get; }

        /// <summary>
        /// 是否为大会员.
        /// </summary>
        bool IsVip { get; }

        /// <summary>
        /// 动态数.
        /// </summary>
        string DynamicCount { get; }

        /// <summary>
        /// 粉丝数.
        /// </summary>
        string FollowerCount { get; }

        /// <summary>
        /// 关注人数.
        /// </summary>
        string FollowCount { get; }

        /// <summary>
        /// 是否已经完成了与账户的连接.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// 未读提及数.
        /// </summary>
        UnreadInformation UnreadInformation { get; }

        /// <summary>
        /// 是否显示未读消息数.
        /// </summary>
        bool IsShowUnreadMessage { get; }

        /// <summary>
        /// 是否显示固定的内容.
        /// </summary>
        bool IsShowFixedItem { get; }
    }
}

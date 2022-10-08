// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Bili.Models.Data.User;
using Bili.Models.Enums.Community;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Account
{
    /// <summary>
    /// 用户视图模型的接口定义.
    /// </summary>
    public interface IUserItemViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 切换关系命令（关注或取消关注）.
        /// </summary>
        IAsyncRelayCommand ToggleRelationCommand { get; }

        /// <summary>
        /// 初始化关系命令.
        /// </summary>
        IAsyncRelayCommand InitializeRelationCommand { get; }

        /// <summary>
        /// 显示用户资料详情的命令.
        /// </summary>
        IRelayCommand ShowDetailCommand { get; }

        /// <summary>
        /// 用户基础信息.
        /// </summary>
        UserProfile User { get; }

        /// <summary>
        /// 用户自我介绍.
        /// </summary>
        string Introduce { get; }

        /// <summary>
        /// 是否为大会员.
        /// </summary>
        bool IsVip { get; }

        /// <summary>
        /// 等级.
        /// </summary>
        int Level { get; }

        /// <summary>
        /// 角色.
        /// </summary>
        string Role { get; }

        /// <summary>
        /// 关注数的可读文本.
        /// </summary>
        string FollowCountText { get; }

        /// <summary>
        /// 粉丝数的可读文本.
        /// </summary>
        string FansCountText { get; }

        /// <summary>
        /// 硬币数的可读文本.
        /// </summary>
        string CoinCountText { get; }

        /// <summary>
        /// 点赞文本.
        /// </summary>
        string LikeCountText { get; }

        /// <summary>
        /// 与该用户的关系.
        /// </summary>
        UserRelationStatus Relation { get; set; }

        /// <summary>
        /// 是否显示关系按钮.
        /// </summary>
        bool IsRelationButtonShown { get; set; }

        /// <summary>
        /// 是否正在修改关系.
        /// </summary>
        bool IsRelationChanging { get; }

        /// <summary>
        /// 填充用户信息.
        /// </summary>
        /// <param name="information">用户信息.</param>
        void SetInformation(AccountInformation information);

        /// <summary>
        /// 填充用户资料.
        /// </summary>
        /// <param name="profile">用户资料.</param>
        void SetProfile(RoleProfile profile);

        /// <summary>
        /// 填充用户资料.
        /// </summary>
        /// <param name="profile">用户资料.</param>
        void SetProfile(UserProfile profile);
    }
}

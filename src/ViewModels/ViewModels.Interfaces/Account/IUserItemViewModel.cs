// Copyright (c) Richasy. All rights reserved.

using System.Reactive;
using Bili.Models.Data.User;
using Bili.Models.Enums.Community;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces.Account
{
    /// <summary>
    /// 用户视图模型的接口定义.
    /// </summary>
    public interface IUserItemViewModel
    {
        /// <summary>
        /// 切换关系命令（关注或取消关注）.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ToggleRelationCommand { get; }

        /// <summary>
        /// 显示用户资料详情的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShowDetailCommand { get; }

        /// <summary>
        /// 初始化关系命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> InitializeRelationCommand { get; }

        /// <summary>
        /// 用户基础信息.
        /// </summary>
        public UserProfile User { get; }

        /// <summary>
        /// 用户自我介绍.
        /// </summary>
        public string Introduce { get; }

        /// <summary>
        /// 是否为大会员.
        /// </summary>
        public bool IsVip { get; }

        /// <summary>
        /// 等级.
        /// </summary>
        public int Level { get; }

        /// <summary>
        /// 角色.
        /// </summary>
        public string Role { get; }

        /// <summary>
        /// 关注数的可读文本.
        /// </summary>
        public string FollowCountText { get; }

        /// <summary>
        /// 粉丝数的可读文本.
        /// </summary>
        public string FansCountText { get; }

        /// <summary>
        /// 硬币数的可读文本.
        /// </summary>
        public string CoinCountText { get; }

        /// <summary>
        /// 点赞文本.
        /// </summary>
        public string LikeCountText { get; }

        /// <summary>
        /// 与该用户的关系.
        /// </summary>
        public UserRelationStatus Relation { get; set; }

        /// <summary>
        /// 是否显示关系按钮.
        /// </summary>
        public bool IsRelationButtonShown { get; set; }

        /// <summary>
        /// 是否正在修改关系.
        /// </summary>
        public bool IsRelationChanging { get; }

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

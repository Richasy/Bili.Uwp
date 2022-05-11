// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Enums.Community
{
    /// <summary>
    /// 用户之间的关系状态.
    /// </summary>
    public enum UserRelationStatus
    {
        /// <summary>
        /// 未知状态.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// 你未关注 TA.
        /// </summary>
        Unfollow = 1,

        /// <summary>
        /// 你正在关注 TA.
        /// </summary>
        Following = 2,

        /// <summary>
        /// 你被 TA 关注.
        /// </summary>
        BeFollowed = 3,

        /// <summary>
        /// 你和 TA 互相关注，也许是好友.
        /// </summary>
        Friends = 4,
    }
}

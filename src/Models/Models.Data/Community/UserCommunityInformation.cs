﻿// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Enums.Community;

namespace Bili.Models.Data.Community
{
    /// <summary>
    /// 用户的社区信息.
    /// </summary>
    /// <remarks>
    /// 包含发布的动态数，硬币数等信息.
    /// </remarks>
    public sealed class UserCommunityInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserCommunityInformation"/> class.
        /// </summary>
        public UserCommunityInformation()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCommunityInformation"/> class.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <param name="followCount">关注数.</param>
        /// <param name="fansCount">粉丝数.</param>
        /// <param name="coinCount">硬币数.</param>
        /// <param name="likeCount">点赞数.</param>
        /// <param name="dynamicCount">动态数.</param>
        /// <param name="relation">你和这名用户的关系.</param>
        public UserCommunityInformation(
            string userId,
            int followCount = -1,
            int fansCount = -1,
            double coinCount = -1,
            int likeCount = -1,
            int dynamicCount = -1,
            UserRelationStatus relation = UserRelationStatus.Unknown)
        {
            Id = userId;
            FollowCount = followCount;
            FansCount = fansCount;
            CoinCount = coinCount;
            LikeCount = likeCount;
            DynamicCount = dynamicCount;
            Relation = relation;
        }

        /// <summary>
        /// 用户Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 硬币数.
        /// </summary>
        public double CoinCount { get; }

        /// <summary>
        /// 关注数.
        /// </summary>
        public int FollowCount { get; }

        /// <summary>
        /// 粉丝数.
        /// </summary>
        public int FansCount { get; }

        /// <summary>
        /// 获赞数.
        /// </summary>
        public int LikeCount { get; }

        /// <summary>
        /// 动态数.
        /// </summary>
        public int DynamicCount { get; }

        /// <summary>
        /// 你与 TA 的关系.
        /// </summary>
        public UserRelationStatus Relation { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is UserCommunityInformation information && Id == information.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}

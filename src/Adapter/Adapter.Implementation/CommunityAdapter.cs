// Copyright (c) Richasy. All rights reserved.

using Bili.Adapter.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.Community;

namespace Bili.Adapter
{
    /// <summary>
    /// 社区数据适配器.
    /// </summary>
    public sealed class CommunityAdapter : ICommunityAdapter
    {
        /// <inheritdoc/>
        public UserCommunityInformation ConvertToUserCommunityInformation(Mine mine)
            => new UserCommunityInformation(
                mine.Mid.ToString(),
                mine.FollowCount,
                mine.FollowerCount,
                mine.CoinNumber,
                -1,
                mine.DynamicCount);

        /// <inheritdoc/>
        public UserCommunityInformation ConvertToUserCommunityInformation(UserSpaceInformation spaceInfo)
            => new UserCommunityInformation(
                spaceInfo.UserId,
                spaceInfo.FollowCount,
                spaceInfo.FollowerCount,
                -1,
                spaceInfo.LikeInformation.LikeCount,
                -1);
    }
}

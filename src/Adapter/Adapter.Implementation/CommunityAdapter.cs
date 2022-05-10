// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Adapter.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.BiliBili;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;

namespace Bili.Adapter
{
    /// <summary>
    /// 社区数据适配器.
    /// </summary>
    public sealed class CommunityAdapter : ICommunityAdapter
    {
        private readonly INumberToolkit _numberToolkit;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunityAdapter"/> class.
        /// </summary>
        /// <param name="numberToolkit">数字处理工具.</param>
        public CommunityAdapter(INumberToolkit numberToolkit)
            => _numberToolkit = numberToolkit;

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

        /// <inheritdoc/>
        public VideoCommunityInformation ConvertToVideoCommunityInformation(RecommendCard videoCard)
        {
            if (videoCard.CardGoto != ServiceConstants.Av)
            {
                throw new ArgumentException($"推荐卡片的 CardGoTo 属性应该是 {ServiceConstants.Av}，这里是 {videoCard.Goto}，不符合要求，请检查分配条件", nameof(videoCard));
            }

            var playCount = _numberToolkit.GetCountNumber(videoCard.PlayCountText, "观看");
            var danmakuCount = _numberToolkit.GetCountNumber(videoCard.SubStatusText, "弹幕");
            var recommendReason = videoCard.RecommendReason;
            return new VideoCommunityInformation(
                videoCard.Parameter,
                playCount: playCount,
                danmakuCount: danmakuCount,
                recommendReason: recommendReason);
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Community;

namespace Bili.Models.Data.Live
{
    /// <summary>
    /// 直播信息流视图信息.
    /// </summary>
    public sealed class LiveFeedView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveFeedView"/> class.
        /// </summary>
        /// <param name="banners">横幅列表.</param>
        /// <param name="hotPartitions">热门分区列表.</param>
        /// <param name="followLives">关注的直播间列表.</param>
        /// <param name="recommendLives">推荐的直播间列表.</param>
        public LiveFeedView(
            IEnumerable<BannerIdentifier> banners,
            IEnumerable<Partition> hotPartitions,
            IEnumerable<LiveInformation> followLives,
            IEnumerable<LiveInformation> recommendLives)
        {
            Banners = banners;
            HotPartitions = hotPartitions;
            FollowLives = followLives;
            RecommendLives = recommendLives;
        }

        /// <summary>
        /// 横幅列表.
        /// </summary>
        public IEnumerable<BannerIdentifier> Banners { get; }

        /// <summary>
        /// 热门分区列表.
        /// </summary>
        public IEnumerable<Partition> HotPartitions { get; }

        /// <summary>
        /// 我关注的正在直播的直播间列表.
        /// </summary>
        public IEnumerable<LiveInformation> FollowLives { get; }

        /// <summary>
        /// 推荐的直播列表.
        /// </summary>
        public IEnumerable<LiveInformation> RecommendLives { get; }
    }
}

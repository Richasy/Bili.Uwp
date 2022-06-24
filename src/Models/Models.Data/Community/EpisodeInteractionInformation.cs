// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Community
{
    /// <summary>
    /// 剧集单集的交互信息.
    /// </summary>
    public sealed class EpisodeInteractionInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodeInteractionInformation"/> class.
        /// </summary>
        public EpisodeInteractionInformation(
            bool isLiked,
            bool isCoined,
            bool isFavorited)
        {
            IsLiked = isLiked;
            IsCoined = isCoined;
            IsFavorited = isFavorited;
        }

        /// <summary>
        /// 是否已点赞.
        /// </summary>
        public bool IsLiked { get; set; }

        /// <summary>
        /// 是否已投币.
        /// </summary>
        public bool IsCoined { get; set; }

        /// <summary>
        /// 是否已收藏.
        /// </summary>
        public bool IsFavorited { get; set; }
    }
}

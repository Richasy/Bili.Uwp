// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Community
{
    /// <summary>
    /// 一键三连的结果信息.
    /// </summary>
    public sealed class TripleInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TripleInformation"/> class.
        /// </summary>
        /// <param name="id">对应视频的Id.</param>
        /// <param name="isLiked">是否已点赞.</param>
        /// <param name="isCoined">是否已投币.</param>
        /// <param name="isFavorited">是否已收藏.</param>
        public TripleInformation(string id, bool isLiked, bool isCoined, bool isFavorited)
        {
            Id = id;
            IsLiked = isLiked;
            IsCoined = isCoined;
            IsFavorited = isFavorited;
        }

        /// <summary>
        /// 对应视频的Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 是否点赞.
        /// </summary>
        public bool IsLiked { get; set; }

        /// <summary>
        /// 是否投币.
        /// </summary>
        public bool IsCoined { get; set; }

        /// <summary>
        /// 是否收藏.
        /// </summary>
        public bool IsFavorited { get; set; }
    }
}

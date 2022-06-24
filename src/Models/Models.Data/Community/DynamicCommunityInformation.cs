// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Community
{
    /// <summary>
    /// 动态的社区信息.
    /// </summary>
    public sealed class DynamicCommunityInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicCommunityInformation"/> class.
        /// </summary>
        /// <param name="id">动态 Id.</param>
        /// <param name="likeCount">点赞数.</param>
        /// <param name="commentCount">评论数.</param>
        /// <param name="isLiked">是否已点赞.</param>
        public DynamicCommunityInformation(
            string id,
            double likeCount,
            double commentCount,
            bool isLiked = false)
        {
            Id = id;
            LikeCount = likeCount;
            CommentCount = commentCount;
            IsLiked = isLiked;
        }

        /// <summary>
        /// 动态 Id.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 点赞数.
        /// </summary>
        public double LikeCount { get; set; }

        /// <summary>
        /// 评论数.
        /// </summary>
        public double CommentCount { get; }

        /// <summary>
        /// 是否已点赞.
        /// </summary>
        public bool IsLiked { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is DynamicCommunityInformation information && Id == information.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}

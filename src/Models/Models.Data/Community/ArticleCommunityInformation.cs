// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Community
{
    /// <summary>
    /// 文章社区数据.
    /// </summary>
    public sealed class ArticleCommunityInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleCommunityInformation"/> class.
        /// </summary>
        /// <param name="id">文章 Id.</param>
        /// <param name="viewCount">阅读次数.</param>
        /// <param name="favoriteCount">收藏次数.</param>
        /// <param name="likeCount">点赞数.</param>
        /// <param name="commentCount">评论数.</param>
        /// <param name="shareCount">分享数.</param>
        /// <param name="coinCount">投币数.</param>
        public ArticleCommunityInformation(
            string id,
            int viewCount,
            int favoriteCount = -1,
            int likeCount = -1,
            int commentCount = -1,
            int shareCount = -1,
            int coinCount = -1)
        {
            Id = id;
            ViewCount = viewCount;
            FavoriteCount = favoriteCount;
            LikeCount = likeCount;
            CommentCount = commentCount;
            ShareCount = shareCount;
            CoinCount = coinCount;
        }

        /// <summary>
        /// 所属文章 Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 阅读次数.
        /// </summary>
        public int ViewCount { get; }

        /// <summary>
        /// 收藏次数.
        /// </summary>
        public int FavoriteCount { get; }

        /// <summary>
        /// 点赞次数.
        /// </summary>
        public int LikeCount { get; }

        /// <summary>
        /// 评论次数.
        /// </summary>
        public int CommentCount { get; }

        /// <summary>
        /// 分享次数.
        /// </summary>
        public int ShareCount { get; }

        /// <summary>
        /// 投币数.
        /// </summary>
        public int CoinCount { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is ArticleCommunityInformation information && Id == information.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}

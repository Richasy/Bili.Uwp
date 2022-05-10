// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Community
{
    /// <summary>
    /// 视频的社区交互信息.
    /// </summary>
    /// <remarks>
    /// 比如观看次数、投币数等.
    /// </remarks>
    public sealed class VideoCommunityInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoCommunityInformation"/> class.
        /// </summary>
        /// <param name="videoId">视频 Id.</param>
        /// <param name="playCount">播放数.</param>
        /// <param name="danmakuCount">弹幕数.</param>
        /// <param name="likeCount">点赞数.</param>
        /// <param name="score">视频评分.</param>
        /// <param name="favoriteCount">收藏数.</param>
        /// <param name="coinCount">投币数.</param>
        /// <param name="commentCount">评论数.</param>
        /// <param name="recommendReason">推荐理由.</param>
        /// <param name="tags">标签列表.</param>
        /// <remarks>
        /// 对于数字类型的参数来说，默认值是 -1，表示暂时没有该类型的值输入.
        /// </remarks>
        public VideoCommunityInformation(
            string videoId,
            double playCount = -1,
            double danmakuCount = -1,
            double likeCount = -1,
            double score = -1,
            double favoriteCount = -1,
            double coinCount = -1,
            double commentCount = -1,
            string recommendReason = "",
            IEnumerable<Tag> tags = null)
        {
            Id = videoId;
            PlayCount = playCount;
            DanmakuCount = danmakuCount;
            LikeCount = likeCount;
            Score = score;
            FavoriteCount = favoriteCount;
            CoinCount = coinCount;
            CommentCount = commentCount;
            RecommendReason = recommendReason;
            Tags = tags;
        }

        /// <summary>
        /// 视频 Id.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 播放数.
        /// </summary>
        /// <remarks>
        /// 在有些情况下，原始数据可能不包含精确的播放次数，而是以可读文本形式表示（比如 <c>24.8万观看</c>），这时会将其转化为约数，即 <c>248000</c>.
        /// </remarks>
        public double PlayCount { get; set; }

        /// <summary>
        /// 弹幕数.
        /// </summary>
        /// <remarks>
        /// 在有些情况下，原始数据可能不包含精确的弹幕数，而是以可读文本形式表示（比如 <c>3.6万弹幕</c>），这时会将其转化为约数，即 <c>36000</c>.
        /// </remarks>
        public double DanmakuCount { get; set; }

        /// <summary>
        /// 点赞数.
        /// </summary>
        public double LikeCount { get; set; }

        /// <summary>
        /// 投币数.
        /// </summary>
        public double CoinCount { get; set; }

        /// <summary>
        /// 收藏数.
        /// </summary>
        public double FavoriteCount { get; set; }

        /// <summary>
        /// 评论数.
        /// </summary>
        public double CommentCount { get; set; }

        /// <summary>
        /// 评分.
        /// </summary>
        /// <remarks>
        /// 在排行榜一类的视频中，会有视频综合评分，这也是社区交互数据.
        /// </remarks>
        public double Score { get; }

        /// <summary>
        /// 视频的推荐理由.
        /// </summary>
        /// <remarks>
        /// 在视频推荐或者热门视频中，会有视频的推荐理由，这相当于一个实时标签.
        /// </remarks>
        public string RecommendReason { get; }

        /// <summary>
        /// 视频标签列表.
        /// </summary>
        public IEnumerable<Tag> Tags { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is VideoCommunityInformation information && Id == information.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}

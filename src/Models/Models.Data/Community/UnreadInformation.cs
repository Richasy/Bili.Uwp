// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Community
{
    /// <summary>
    /// 未读信息.
    /// </summary>
    public sealed class UnreadInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnreadInformation"/> class.
        /// </summary>
        /// <param name="atCount">提到的次数.</param>
        /// <param name="replyCount">回复的次数.</param>
        /// <param name="likeCount">点赞的次数.</param>
        public UnreadInformation(
            int atCount,
            int replyCount,
            int likeCount)
        {
            AtCount = atCount;
            ReplyCount = replyCount;
            LikeCount = likeCount;
        }

        /// <summary>
        /// 提到的次数.
        /// </summary>
        public int AtCount { get; }

        /// <summary>
        /// 回复的次数.
        /// </summary>
        public int ReplyCount { get; }

        /// <summary>
        /// 点赞的次数.
        /// </summary>
        public int LikeCount { get; }

        /// <summary>
        /// 未读消息总数.
        /// </summary>
        public int Total => AtCount + LikeCount + ReplyCount;
    }
}

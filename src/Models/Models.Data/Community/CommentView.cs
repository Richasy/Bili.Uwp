// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Community
{
    /// <summary>
    /// 评论视图.
    /// </summary>
    public sealed class CommentView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentView"/> class.
        /// </summary>
        /// <param name="comments">评论列表.</param>
        /// <param name="targetId">评论区或根评论 Id.</param>
        /// <param name="topComment">置顶评论.</param>
        /// <param name="isEnd">是否已经请求完全部内容.</param>
        public CommentView(
            IEnumerable<CommentInformation> comments,
            string targetId,
            CommentInformation topComment,
            bool isEnd)
        {
            Comments = comments;
            TargetId = targetId;
            TopComment = topComment;
            IsEnd = isEnd;
        }

        /// <summary>
        /// 评论列表.
        /// </summary>
        public IEnumerable<CommentInformation> Comments { get; }

        /// <summary>
        /// 评论区或根评论 Id.
        /// </summary>
        public string TargetId { get; }

        /// <summary>
        /// 置顶评论.
        /// </summary>
        public CommentInformation TopComment { get; set; }

        /// <summary>
        /// 是否到底.
        /// </summary>
        public bool IsEnd { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is CommentView view && TargetId == view.TargetId;

        /// <inheritdoc/>
        public override int GetHashCode() => TargetId.GetHashCode();
    }
}

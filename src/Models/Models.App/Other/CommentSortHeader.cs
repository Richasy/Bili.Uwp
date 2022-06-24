// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Enums.Bili;

namespace Bili.Models.App.Other
{
    /// <summary>
    /// 评论区排序方式标头.
    /// </summary>
    public sealed class CommentSortHeader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentSortHeader"/> class.
        /// </summary>
        /// <param name="type">排序方式.</param>
        /// <param name="title">排序方式的可读文本.</param>
        public CommentSortHeader(CommentSortType type, string title)
        {
            Type = type;
            Title = title;
        }

        /// <summary>
        /// 排序方式.
        /// </summary>
        public CommentSortType Type { get; }

        /// <summary>
        /// 排序方式的可读文本.
        /// </summary>
        public string Title { get; }

        /// <inheritdoc/>
        public override string ToString() => Title;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is CommentSortHeader header && Type == header.Type;

        /// <inheritdoc/>
        public override int GetHashCode() => Type.GetHashCode();
    }
}

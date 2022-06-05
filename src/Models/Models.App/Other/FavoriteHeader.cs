// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Enums.App;

namespace Bili.Models.App.Other
{
    /// <summary>
    /// 收藏夹分类头部信息.
    /// </summary>
    public sealed class FavoriteHeader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteHeader"/> class.
        /// </summary>
        /// <param name="type">收藏夹类型.</param>
        /// <param name="title">收藏夹类型标题.</param>
        public FavoriteHeader(FavoriteType type, string title)
        {
            Type = type;
            Title = title;
        }

        /// <summary>
        /// 收藏夹类型.
        /// </summary>
        public FavoriteType Type { get; set; }

        /// <summary>
        /// 收藏夹类型标题.
        /// </summary>
        public string Title { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is FavoriteHeader header && Type == header.Type;

        /// <inheritdoc/>
        public override int GetHashCode() => Type.GetHashCode();
    }
}

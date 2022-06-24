// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Video
{
    /// <summary>
    /// 收藏夹分组.
    /// </summary>
    public sealed class VideoFavoriteFolderGroup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoFavoriteFolderGroup"/> class.
        /// </summary>
        /// <param name="id">分组 Id.</param>
        /// <param name="title">标题.</param>
        /// <param name="isMine">是否由本人创建.</param>
        /// <param name="set">收藏集.</param>
        public VideoFavoriteFolderGroup(
            string id,
            string title,
            bool isMine,
            VideoFavoriteSet set)
        {
            Id = id;
            Title = title;
            IsMine = isMine;
            FavoriteSet = set;
        }

        /// <summary>
        /// 分组 Id.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 标题.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// 是否由本人创建.
        /// </summary>
        public bool IsMine { get; }

        /// <summary>
        /// 收藏集.
        /// </summary>
        public VideoFavoriteSet FavoriteSet { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is VideoFavoriteFolderGroup group && Id == group.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}

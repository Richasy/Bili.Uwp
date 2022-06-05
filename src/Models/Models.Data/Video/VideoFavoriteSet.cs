// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Local;

namespace Bili.Models.Data.Video
{
    /// <summary>
    /// 收藏夹集合.
    /// </summary>
    public sealed class VideoFavoriteSet : ContentSet<VideoFavoriteFolder>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoFavoriteSet"/> class.
        /// </summary>
        /// <param name="items">收藏夹列表.</param>
        /// <param name="totalCount">收藏夹总数.</param>
        public VideoFavoriteSet(
            IEnumerable<VideoFavoriteFolder> items,
            int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Video
{
    /// <summary>
    /// 视频收藏视图.
    /// </summary>
    public sealed class VideoFavoriteView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoFavoriteView"/> class.
        /// </summary>
        /// <param name="groups">收藏夹分组.</param>
        /// <param name="defaultFolder">默认收藏夹.</param>
        public VideoFavoriteView(
            IEnumerable<VideoFavoriteFolderGroup> groups,
            VideoFavoriteFolderDetail defaultFolder)
        {
            Groups = groups;
            DefaultFolder = defaultFolder;
        }

        /// <summary>
        /// 收藏夹分组列表.
        /// </summary>
        public IEnumerable<VideoFavoriteFolderGroup> Groups { get; }

        /// <summary>
        /// 默认收藏夹.
        /// </summary>
        public VideoFavoriteFolderDetail DefaultFolder { get; }
    }
}

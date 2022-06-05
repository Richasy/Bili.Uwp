// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Video
{
    /// <summary>
    /// 收藏夹详情.
    /// </summary>
    public sealed class VideoFavoriteFolderDetail
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteFolderDetail"/> class.
        /// </summary>
        /// <param name="folder">收藏夹信息.</param>
        /// <param name="videoSet">收藏夹内包含的视频集.</param>
        public VideoFavoriteFolderDetail(VideoFavoriteFolder folder, VideoSet videoSet)
        {
            Folder = folder;
            VideoSet = videoSet;
        }

        /// <summary>
        /// 收藏夹信息.
        /// </summary>
        public VideoFavoriteFolder Folder { get; }

        /// <summary>
        /// 收藏夹中显示的视频集.
        /// </summary>
        public VideoSet VideoSet { get; }
    }
}

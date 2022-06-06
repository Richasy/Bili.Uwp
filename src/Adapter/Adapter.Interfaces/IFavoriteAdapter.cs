// Copyright (c) Richasy. All rights reserved.

using Bili.Models.BiliBili;
using Bili.Models.Data.Video;

namespace Bili.Adapter.Interfaces
{
    /// <summary>
    /// 收藏夹数据适配器接口.
    /// </summary>
    public interface IFavoriteAdapter
    {
        /// <summary>
        /// 将收藏夹列表详情 <see cref="FavoriteListDetail"/> 转换为收藏夹信息.
        /// </summary>
        /// <param name="detail">收藏夹列表详情.</param>
        /// <returns><see cref="Models.Data.Community.VideoFavoriteFolder"/>.</returns>
        VideoFavoriteFolder ConvertToVideoFavoriteFolder(FavoriteListDetail detail);

        /// <summary>
        /// 将收藏夹元数据 <see cref="FavoriteMeta"/> 转换为收藏夹信息.
        /// </summary>
        /// <param name="meta">收藏夹元数据.</param>
        /// <returns><see cref="Models.Data.Community.VideoFavoriteFolder"/>.</returns>
        VideoFavoriteFolder ConvertToVideoFavoriteFolder(FavoriteMeta meta);

        /// <summary>
        /// 将收藏夹组 <see cref="FavoriteFolder"/> 转换为收藏夹分组.
        /// </summary>
        /// <param name="folder">收藏夹组.</param>
        /// <returns><see cref="VideoFavoriteFolderGroup"/>.</returns>
        VideoFavoriteFolderGroup ConvertToVideoFavoriteFolderGroup(FavoriteFolder folder);

        /// <summary>
        /// 将视频收藏夹列表响应 <see cref="VideoFavoriteListResponse"/> 转换为视频收藏夹详情.
        /// </summary>
        /// <param name="response">视频收藏夹列表响应.</param>
        /// <returns><see cref="VideoFavoriteFolderDetail"/>.</returns>
        VideoFavoriteFolderDetail ConvertToVideoFavoriteFolderDetail(VideoFavoriteListResponse response);

        /// <summary>
        /// 将视频收藏夹概览响应 <see cref="VideoFavoriteGalleryResponse"/> 转换为视频收藏视图.
        /// </summary>
        /// <param name="response">视频收藏夹概览响应.</param>
        /// <returns><see cref="VideoFavoriteView"/>.</returns>
        VideoFavoriteView ConvertToVideoFavoriteView(VideoFavoriteGalleryResponse response);
    }
}

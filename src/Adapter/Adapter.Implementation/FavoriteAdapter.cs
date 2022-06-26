// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using Bili.Adapter.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;

namespace Bili.Adapter
{
    /// <summary>
    /// 收藏夹数据适配器.
    /// </summary>
    public sealed class FavoriteAdapter : IFavoriteAdapter
    {
        private readonly IImageAdapter _imageAdapter;
        private readonly IUserAdapter _userAdapter;
        private readonly IVideoAdapter _videoAdapter;
        private readonly ITextToolkit _textToolkit;

        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteAdapter"/> class.
        /// </summary>
        /// <param name="imageAdapter">图片数据适配器.</param>
        /// <param name="userAdapter">用户数据适配器.</param>
        /// <param name="videoAdapter">视频数据适配器.</param>
        /// <param name="textToolkit">文本工具.</param>
        public FavoriteAdapter(
            IImageAdapter imageAdapter,
            IUserAdapter userAdapter,
            IVideoAdapter videoAdapter,
            ITextToolkit textToolkit)
        {
            _imageAdapter = imageAdapter;
            _userAdapter = userAdapter;
            _videoAdapter = videoAdapter;
            _textToolkit = textToolkit;
        }

        /// <inheritdoc/>
        public VideoFavoriteFolder ConvertToVideoFavoriteFolder(FavoriteListDetail detail)
        {
            var id = detail.Id.ToString();
            var title = _textToolkit.ConvertToTraditionalChineseIfNeeded(detail.Title);
            var cover = string.IsNullOrEmpty(detail.Cover)
                ? null
                : _imageAdapter.ConvertToImage(detail.Cover, 160, 120);
            var user = string.IsNullOrEmpty(detail.Publisher?.Publisher)
                ? null
                : _userAdapter.ConvertToRoleProfile(detail.Publisher, Models.Enums.App.AvatarSize.Size48).User;
            var desc = _textToolkit.ConvertToTraditionalChineseIfNeeded(detail.Description);
            var count = detail.MediaCount;

            return new VideoFavoriteFolder(id, title, cover, user, desc, count);
        }

        /// <inheritdoc/>
        public VideoFavoriteFolder ConvertToVideoFavoriteFolder(FavoriteMeta meta)
        {
            var id = meta.Id.ToString();
            var title = _textToolkit.ConvertToTraditionalChineseIfNeeded(meta.Title);
            var count = meta.MediaCount;

            return new VideoFavoriteFolder(id, title, default, default, default, count);
        }

        /// <inheritdoc/>
        public VideoFavoriteFolderDetail ConvertToVideoFavoriteFolderDetail(VideoFavoriteListResponse response)
        {
            var folder = ConvertToVideoFavoriteFolder(response.Detail ?? response.Information);
            var videos = response.Medias.Select(p => _videoAdapter.ConvertToVideoInformation(p));
            var videoSet = new VideoSet(videos, folder.TotalCount);
            return new VideoFavoriteFolderDetail(folder, videoSet);
        }

        /// <inheritdoc/>
        public VideoFavoriteFolderGroup ConvertToVideoFavoriteFolderGroup(FavoriteFolder folder)
        {
            var id = folder.Id;
            var name = _textToolkit.ConvertToTraditionalChineseIfNeeded(folder.Name);
            var isMine = id == 1;
            var folders = folder.MediaList.List.Select(p => ConvertToVideoFavoriteFolder(p));
            var set = new VideoFavoriteSet(folders, folder.MediaList.Count);
            return new VideoFavoriteFolderGroup(id.ToString(), name, isMine, set);
        }

        /// <inheritdoc/>
        public VideoFavoriteView ConvertToVideoFavoriteView(VideoFavoriteGalleryResponse response)
        {
            var defaultFolder = ConvertToVideoFavoriteFolderDetail(response.DefaultFavoriteList);

            // 过滤稍后再看的内容，稍后再看列表的Id为3.
            var favoriteSets = response.FavoriteFolderList
                .Where(p => p.Id != 3)
                .Select(p => ConvertToVideoFavoriteFolderGroup(p));
            return new VideoFavoriteView(favoriteSets, defaultFolder);
        }
    }
}

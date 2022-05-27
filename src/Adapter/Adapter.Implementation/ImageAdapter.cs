// Copyright (c) Richasy. All rights reserved.

using Bili.Adapter.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.Data.Appearance;

namespace Bili.Adapter
{
    /// <summary>
    /// 图片适配器，将视频封面、用户头像等转换为 <see cref="Image"/>.
    /// </summary>
    public class ImageAdapter : IImageAdapter
    {
        /// <inheritdoc/>
        public Image ConvertToImage(string uri)
            => new Image(uri);

        /// <inheritdoc/>
        public Image ConvertToImage(string uri, double width, double height)
            => new Image(uri, width, height, (w, h) => $"@{w}w_{h}h_1c_100q.jpg");

        /// <inheritdoc/>
        public Image ConvertToVideoCardCover(string uri)
            => ConvertToImage(uri, AppConstants.VideoCardCoverWidth, AppConstants.VideoCardCoverHeight);

        /// <inheritdoc/>
        public Image ConvertToPgcCover(string uri)
            => ConvertToImage(uri, AppConstants.PgcCoverWidth, AppConstants.PgcCoverHeight);

        /// <inheritdoc/>
        public Image ConvertToArticleCardCover(string uri)
            => ConvertToImage(uri, AppConstants.ArticleCardCoverWidth, AppConstants.ArticleCardCoverHeight);
    }
}

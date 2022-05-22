// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Community;

namespace Bili.Models.Data.Video
{
    /// <summary>
    /// 分区视频视图.
    /// </summary>
    public sealed class VideoPartitionView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPartitionView"/> class.
        /// </summary>
        /// <param name="id">分区标识符.</param>
        /// <param name="videos">视频列表.</param>
        /// <param name="banners">横幅列表.</param>
        public VideoPartitionView(
            string id,
            IEnumerable<VideoInformation> videos,
            IEnumerable<BannerIdentifier> banners)
        {
            Id = id;
            Videos = videos;
            Banners = banners;
        }

        /// <summary>
        /// 分区标识符.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 视频列表.
        /// </summary>
        public IEnumerable<VideoInformation> Videos { get; }

        /// <summary>
        /// 横幅列表.
        /// </summary>
        public IEnumerable<BannerIdentifier> Banners { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is VideoPartitionView view && EqualityComparer<string>.Default.Equals(Id, view.Id);

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}

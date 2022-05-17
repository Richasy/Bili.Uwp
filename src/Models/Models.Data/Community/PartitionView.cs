// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Video;

namespace Bili.Models.Data.Community
{
    /// <summary>
    /// 分区视频视图.
    /// </summary>
    public sealed class PartitionView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionView"/> class.
        /// </summary>
        /// <param name="id">分区标识符.</param>
        /// <param name="videos">视频列表.</param>
        /// <param name="banners">横幅列表.</param>
        public PartitionView(
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
        public override bool Equals(object obj) => obj is PartitionView view && EqualityComparer<string>.Default.Equals(Id, view.Id);

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}

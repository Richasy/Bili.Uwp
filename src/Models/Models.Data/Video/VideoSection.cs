// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Video
{
    /// <summary>
    /// 视频合集（专制B站的合集功能）.
    /// </summary>
    public sealed class VideoSection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoSection"/> class.
        /// </summary>
        /// <param name="id">合集 Id.</param>
        /// <param name="title">合集标题.</param>
        /// <param name="videos">合集里的视频列表.</param>
        public VideoSection(
            string id,
            string title,
            IEnumerable<VideoInformation> videos)
        {
            Id = id;
            Title = title;
            Videos = videos;
        }

        /// <summary>
        /// 合集 Id.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 合集标题.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// 视频列表.
        /// </summary>
        public IEnumerable<VideoInformation> Videos { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is VideoSection section && Id == section.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}

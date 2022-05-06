// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Data.Appearance;

namespace Bili.Models.Data.Player
{
    /// <summary>
    /// 视频标识，表示视频的核心信息.
    /// </summary>
    public struct VideoIdentifier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoIdentifier"/> struct.
        /// </summary>
        /// <param name="id">视频 Id.</param>
        /// <param name="title">视频名称.</param>
        /// <param name="duration">视频时长.</param>
        /// <param name="cover">封面.</param>
        public VideoIdentifier(string id, string title, int duration, Image cover)
        {
            Id = id;
            Title = title;
            Cover = cover;
            Duration = duration;
        }

        /// <summary>
        /// 视频标题.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// 视频封面.
        /// </summary>
        public Image Cover { get; }

        /// <summary>
        /// 视频时长，以秒为单位.
        /// </summary>
        public int Duration { get; }

        /// <summary>
        /// 视频 Id，属于网站的资源标识符.
        /// </summary>
        public string Id { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is VideoIdentifier identifier && Id == identifier.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => $"{Title} | {TimeSpan.FromSeconds(Duration)} | {Id}";
    }
}

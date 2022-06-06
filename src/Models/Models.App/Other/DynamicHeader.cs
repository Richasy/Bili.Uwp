// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.App.Other
{
    /// <summary>
    /// 动态分区标头.
    /// </summary>
    public sealed class DynamicHeader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicHeader"/> class.
        /// </summary>
        /// <param name="isVideo">是否为视频动态.</param>
        /// <param name="title">头标题.</param>
        public DynamicHeader(
            bool isVideo,
            string title)
        {
            IsVideo = isVideo;
            Title = title;
        }

        /// <summary>
        /// 是否为视频动态.
        /// </summary>
        public bool IsVideo { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        public string Title { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is DynamicHeader header && Title == header.Title;

        /// <inheritdoc/>
        public override int GetHashCode() => Title.GetHashCode();
    }
}

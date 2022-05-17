// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Appearance;

namespace Bili.Models.Data.Community
{
    /// <summary>
    /// 横幅条目的核心数据.
    /// </summary>
    public struct BannerIdentifier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BannerIdentifier"/> struct.
        /// </summary>
        /// <param name="id">标识符.</param>
        /// <param name="title">标题.</param>
        /// <param name="image">图片.</param>
        /// <param name="uri">导航地址.</param>
        public BannerIdentifier(
            string id,
            string title,
            Image image,
            string uri)
        {
            Id = id;
            Title = title;
            Image = image;
            Uri = uri;
        }

        /// <summary>
        /// 标识符.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 标题或说明文本.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// 图片.
        /// </summary>
        public Image Image { get; }

        /// <summary>
        /// 目标导航地址.
        /// </summary>
        public string Uri { get; }
    }
}

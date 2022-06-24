// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Appearance;

namespace Bili.Models.Data.Article
{
    /// <summary>
    /// 文章标识符.
    /// </summary>
    public struct ArticleIdentifier
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleIdentifier"/> struct.
        /// </summary>
        /// <param name="id">视频 Id.</param>
        /// <param name="title">视频名称.</param>
        /// <param name="summary">视频时长.</param>
        /// <param name="cover">封面.</param>
        public ArticleIdentifier(string id, string title, string summary, Image cover)
        {
            Id = id;
            Title = title;
            Cover = cover;
            Summary = summary;
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
        public string Summary { get; }

        /// <summary>
        /// 视频 Id，属于网站的资源标识符.
        /// </summary>
        public string Id { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is ArticleIdentifier identifier && Id == identifier.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => $"{Title} | {Id}";
    }
}

// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Community;

namespace Bili.Models.Data.Article
{
    /// <summary>
    /// 文章分区视图.
    /// </summary>
    public sealed class ArticlePartitionView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticlePartitionView"/> class.
        /// </summary>
        /// <param name="articles">文章列表.</param>
        /// <param name="banners">横幅列表.</param>
        /// <param name="ranks">排行榜.</param>
        public ArticlePartitionView(
            IEnumerable<ArticleInformation> articles,
            IEnumerable<BannerIdentifier> banners = default,
            IEnumerable<ArticleInformation> ranks = default)
        {
            Banners = banners;
            Ranks = ranks;
            Articles = articles;
        }

        /// <summary>
        /// 横幅列表.
        /// </summary>
        public IEnumerable<BannerIdentifier> Banners { get; }

        /// <summary>
        /// 排行榜.
        /// </summary>
        public IEnumerable<ArticleInformation> Ranks { get; }

        /// <summary>
        /// 文章列表.
        /// </summary>
        public IEnumerable<ArticleInformation> Articles { get; }
    }
}

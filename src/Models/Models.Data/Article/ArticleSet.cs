// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Local;

namespace Bili.Models.Data.Article
{
    /// <summary>
    /// 文章集.
    /// </summary>
    public sealed class ArticleSet : ContentSet<ArticleInformation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleSet"/> class.
        /// </summary>
        /// <param name="items">文章列表.</param>
        /// <param name="totalCount">文章总数.</param>
        public ArticleSet(
            IEnumerable<ArticleInformation> items,
            int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }
    }
}

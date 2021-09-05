// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 文章收藏更新事件参数.
    /// </summary>
    public class FavoriteArticleIterationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FavoriteArticleIterationEventArgs"/> class.
        /// </summary>
        /// <param name="response">响应结果.</param>
        /// <param name="pageNumebr">页码.</param>
        public FavoriteArticleIterationEventArgs(ArticleFavoriteListResponse response, int pageNumebr)
        {
            List = response.Items;
            TotalCount = response.Count;
            NextPageNumber = pageNumebr + 1;
        }

        /// <summary>
        /// 条目列表.
        /// </summary>
        public List<FavoriteArticleItem> List { get; set; }

        /// <summary>
        /// 下一页页码.
        /// </summary>
        public int NextPageNumber { get; set; }

        /// <summary>
        /// 总数.
        /// </summary>
        public int TotalCount { get; set; }
    }
}

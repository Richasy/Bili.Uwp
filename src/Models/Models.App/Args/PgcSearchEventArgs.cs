// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// PGC搜索事件参数.
    /// </summary>
    public class PgcSearchEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoSearchEventArgs"/> class.
        /// </summary>
        /// <param name="response">Pgc搜索响应结果.</param>
        /// <param name="currentPageNumber">当前页码.</param>
        /// <param name="keyword">搜索关键词.</param>
        public PgcSearchEventArgs(SubModuleSearchResultResponse<PgcSearchItem> response, int currentPageNumber, string keyword)
        {
            HasMore = response.PageNumber > currentPageNumber;
            NextPageNumber = HasMore ? currentPageNumber + 1 : -1;
            List = response.ItemList.ToList();
            Keyword = keyword;
        }

        /// <summary>
        /// 是否加载完成.
        /// </summary>
        public bool HasMore { get; set; }

        /// <summary>
        /// 下一页页码（如果没有下一页，则返回-1）.
        /// </summary>
        public int NextPageNumber { get; set; }

        /// <summary>
        /// Pgc搜索结果.
        /// </summary>
        public List<PgcSearchItem> List { get; set; }

        /// <summary>
        /// 关键词.
        /// </summary>
        public string Keyword { get; set; }
    }
}

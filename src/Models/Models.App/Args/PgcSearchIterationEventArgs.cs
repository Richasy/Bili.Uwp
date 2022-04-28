// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using Bili.Models.BiliBili;

namespace Bili.Models.App.Args
{
    /// <summary>
    /// PGC搜索事件参数.
    /// </summary>
    public class PgcSearchIterationEventArgs : SearchIterationEventArgs<PgcSearchItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoSearchIterationEventArgs"/> class.
        /// </summary>
        /// <param name="response">Pgc搜索响应结果.</param>
        /// <param name="currentPageNumber">当前页码.</param>
        /// <param name="keyword">搜索关键词.</param>
        public PgcSearchIterationEventArgs(SubModuleSearchResultResponse<PgcSearchItem> response, int currentPageNumber, string keyword)
        {
            HasMore = response.PageNumber > currentPageNumber;
            NextPageNumber = currentPageNumber + 1;
            List = response.ItemList.ToList();
            Keyword = keyword;
        }
    }
}

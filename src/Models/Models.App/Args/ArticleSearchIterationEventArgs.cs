// Copyright (c) GodLeaveMe. All rights reserved.

using System.Linq;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 专栏文章搜索事件参数.
    /// </summary>
    public class ArticleSearchIterationEventArgs : SearchIterationEventArgs<ArticleSearchItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleSearchIterationEventArgs"/> class.
        /// </summary>
        /// <param name="response">文章搜索响应结果.</param>
        /// <param name="currentPageNumber">当前页码.</param>
        /// <param name="keyword">搜索关键词.</param>
        public ArticleSearchIterationEventArgs(SubModuleSearchResultResponse<ArticleSearchItem> response, int currentPageNumber, string keyword)
        {
            HasMore = response.PageNumber > currentPageNumber;
            NextPageNumber = currentPageNumber + 1;
            List = response.ItemList.ToList();
            Keyword = keyword;
        }
    }
}

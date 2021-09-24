// Copyright (c) GodLeaveMe. All rights reserved.

using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 用户搜索事件参数.
    /// </summary>
    public class UserSearchIterationEventArgs : SearchIterationEventArgs<UserSearchItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSearchIterationEventArgs"/> class.
        /// </summary>
        /// <param name="response">搜索结果.</param>
        /// <param name="pageNumber">页码.</param>
        /// <param name="keyword">关键词.</param>
        public UserSearchIterationEventArgs(SubModuleSearchResultResponse<UserSearchItem> response, int pageNumber, string keyword)
        {
            HasMore = response.PageNumber > pageNumber;
            NextPageNumber = pageNumber + 1;
            List = response.ItemList;
            Keyword = keyword;
        }
    }
}

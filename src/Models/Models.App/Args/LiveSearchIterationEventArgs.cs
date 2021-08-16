// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 直播搜索事件参数.
    /// </summary>
    public class LiveSearchIterationEventArgs : SearchIterationEventArgs<LiveSearchItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveSearchIterationEventArgs"/> class.
        /// </summary>
        /// <param name="response">响应.</param>
        /// <param name="keyword">搜索关键词.</param>
        /// <param name="pageNumber">页码.</param>
        public LiveSearchIterationEventArgs(LiveSearchResultResponse response, string keyword, int pageNumber)
        {
            NextPageNumber = pageNumber + 1;
            List = response.RoomResult.Items;
            HasMore = response.PageNumber > pageNumber;
            Keyword = keyword;
        }
    }
}

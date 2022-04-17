// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 视频搜索结果参数.
    /// </summary>
    public class VideoSearchIterationEventArgs : SearchIterationEventArgs<VideoSearchItem>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoSearchIterationEventArgs"/> class.
        /// </summary>
        /// <param name="response">综合搜索响应结果.</param>
        /// <param name="currentPageNumber">当前页码.</param>
        public VideoSearchIterationEventArgs(ComprehensiveSearchResultResponse response, int currentPageNumber)
        {
            NextPageNumber = currentPageNumber + 1;
            Keyword = response.Keyword;
            HasMore = true;
            List = response.ItemList != null && response.ItemList.Any(p => p.Goto == "av")
                ? response.ItemList.Where(p => p.Goto == "av").ToList()
                : new System.Collections.Generic.List<VideoSearchItem>();
        }
    }
}

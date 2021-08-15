// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 视频搜索结果参数.
    /// </summary>
    public class VideoSearchEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoSearchEventArgs"/> class.
        /// </summary>
        /// <param name="response">综合搜索响应结果.</param>
        /// <param name="currentPageNumber">当前页码.</param>
        public VideoSearchEventArgs(ComprehensiveSearchResultResponse response, int currentPageNumber)
        {
            HasMore = currentPageNumber < response.PageNumber;
            NextPageNumber = HasMore ? currentPageNumber + 1 : -1;
            List = response.ItemList.Where(p => p.Goto == "av").ToList();
            Keyword = response.Keyword;
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
        /// 视频搜索结果.
        /// </summary>
        public List<VideoSearchItem> List { get; set; }

        /// <summary>
        /// 关键词.
        /// </summary>
        public string Keyword { get; set; }
    }
}

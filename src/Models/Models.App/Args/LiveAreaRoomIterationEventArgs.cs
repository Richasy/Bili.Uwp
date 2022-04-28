// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.BiliBili;

namespace Bili.Models.App.Args
{
    /// <summary>
    /// 直播分区的直播间迭代事件参数.
    /// </summary>
    public class LiveAreaRoomIterationEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveAreaRoomIterationEventArgs"/> class.
        /// </summary>
        /// <param name="response">响应结果.</param>
        /// <param name="nextPage">下一页码.</param>
        /// <param name="sortType">排序方式.</param>
        public LiveAreaRoomIterationEventArgs(LiveAreaDetailResponse response, int nextPage, string sortType)
        {
            List = response.List;
            Tags = response.Tags;
            Count = response.Count;
            NextPageNumber = nextPage;
            SortType = sortType;
        }

        /// <summary>
        /// 直播间列表.
        /// </summary>
        public List<LiveFeedRoom> List { get; internal set; }

        /// <summary>
        /// 子标签列表.
        /// </summary>
        public List<LiveAreaDetailTag> Tags { get; set; }

        /// <summary>
        /// 直播间总数.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 下一页码.
        /// </summary>
        public int NextPageNumber { get; internal set; }

        /// <summary>
        /// 排序方式.
        /// </summary>
        public string SortType { get; set; }
    }
}

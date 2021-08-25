// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 粉丝列表更改事件参数.
    /// </summary>
    public class FansIterationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FansIterationEventArgs"/> class.
        /// </summary>
        /// <param name="response">粉丝响应结果.</param>
        /// <param name="pageNumber">页码.</param>
        /// <param name="userId">用户Id.</param>
        public FansIterationEventArgs(FansResponse response, int pageNumber, int userId)
        {
            List = response.FansList;
            NextPageNumber = pageNumber + 1;
            TotalCount = response.TotalCount;
            UserId = userId;
        }

        /// <summary>
        /// 粉丝列表.
        /// </summary>
        public List<Fans> List { get; set; }

        /// <summary>
        /// 下一页页码.
        /// </summary>
        public int NextPageNumber { get; set; }

        /// <summary>
        /// 总数.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 查询的用户Id.
        /// </summary>
        public int UserId { get; set; }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// PGC索引结果更新事件参数.
    /// </summary>
    public class PgcIndexResultIterationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcIndexResultIterationEventArgs"/> class.
        /// </summary>
        /// <param name="response">响应结果.</param>
        /// <param name="type">PGC类型.</param>
        public PgcIndexResultIterationEventArgs(PgcIndexResultResponse response, PgcType type)
        {
            Type = type;
            List = response.List;
            HasNext = response.HasNext == 1;
            TotalCount = response.TotalCount;
            NextPageNumber = response.PageNumber + 1;
        }

        /// <summary>
        /// 类型.
        /// </summary>
        public PgcType Type { get; set; }

        /// <summary>
        /// 结果列表.
        /// </summary>
        public List<PgcIndexItem> List { get; set; }

        /// <summary>
        /// 是否还有更多.
        /// </summary>
        public bool HasNext { get; set; }

        /// <summary>
        /// 全部条目数.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 下一页码.
        /// </summary>
        public int NextPageNumber { get; set; }
    }
}

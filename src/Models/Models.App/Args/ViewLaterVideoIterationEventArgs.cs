// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Collections.Generic;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 稍后再看视频更新事件参数.
    /// </summary>
    public class ViewLaterVideoIterationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewLaterVideoIterationEventArgs"/> class.
        /// </summary>
        /// <param name="response">稍后再看响应结果.</param>
        /// <param name="pageNumebr">页码.</param>
        public ViewLaterVideoIterationEventArgs(ViewLaterResponse response, int pageNumebr)
        {
            List = response.List;
            TotalCount = response.Count;
            NextPageNumber = pageNumebr + 1;
        }

        /// <summary>
        /// 视频集合.
        /// </summary>
        public List<ViewLaterVideo> List { get; set; }

        /// <summary>
        /// 总条目数.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 下一页码.
        /// </summary>
        public int NextPageNumber { get; set; }
    }
}

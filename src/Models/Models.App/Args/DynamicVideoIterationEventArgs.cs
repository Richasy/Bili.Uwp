// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Bilibili.App.Dynamic.V2;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 视频动态更新事件参数.
    /// </summary>
    public class DynamicVideoIterationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicVideoIterationEventArgs"/> class.
        /// </summary>
        /// <param name="reply">响应结果.</param>
        /// <param name="pageNumber">页码.</param>
        public DynamicVideoIterationEventArgs(DynVideoReply reply)
        {
            var data = reply.DynamicList;
            List = data.List.Where(p =>
                p.CardType == DynamicType.Av
                || p.CardType == DynamicType.Pgc
                || p.CardType == DynamicType.UgcSeason).ToList();
            UpdateCount = Convert.ToInt32(data.UpdateNum);
            HasMore = data.HasMore;
            BaseLine = data.UpdateBaseline;
            UpdateOffset = data.HistoryOffset;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicVideoIterationEventArgs"/> class.
        /// </summary>
        /// <param name="reply">响应结果.</param>
        /// <param name="pageNumber">页码.</param>
        public DynamicVideoIterationEventArgs(DynAllReply reply)
        {
            var data = reply.DynamicList;
            List = data.List.Where(p =>
                p.CardType != DynamicType.DynNone
                && p.CardType != DynamicType.Ad).ToList();
            UpdateCount = Convert.ToInt32(data.UpdateNum);
            HasMore = data.HasMore;
            BaseLine = data.UpdateBaseline;
            UpdateOffset = data.HistoryOffset;
            IsComprehensive = true;
        }

        /// <summary>
        /// 动态条目列表.
        /// </summary>
        public List<DynamicItem> List { get; set; }

        /// <summary>
        /// 更新个数.
        /// </summary>
        public int UpdateCount { get; set; }

        /// <summary>
        /// 是否还有更多.
        /// </summary>
        public bool HasMore { get; set; }

        /// <summary>
        /// 标识符，更新基线.
        /// </summary>
        public string BaseLine { get; set; }

        /// <summary>
        /// 标识符，更新偏移值.
        /// </summary>
        public string UpdateOffset { get; set; }

        /// <summary>
        /// 是否为综合动态.
        /// </summary>
        public bool IsComprehensive { get; set; }
    }
}

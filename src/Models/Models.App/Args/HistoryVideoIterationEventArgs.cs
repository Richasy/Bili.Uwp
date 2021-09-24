// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Bilibili.App.Interfaces.V1;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 历史记录视频更新事件参数.
    /// </summary>
    public class HistoryVideoIterationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HistoryVideoIterationEventArgs"/> class.
        /// </summary>
        /// <param name="reply">响应结果.</param>
        public HistoryVideoIterationEventArgs(CursorV2Reply reply)
        {
            Cursor = reply.Cursor;
            List = reply.Items.ToList();
        }

        /// <summary>
        /// 历史记录条目.
        /// </summary>
        public List<CursorItem> List { get; set; }

        /// <summary>
        /// 游标.
        /// </summary>
        public Cursor Cursor { get; set; }
    }
}

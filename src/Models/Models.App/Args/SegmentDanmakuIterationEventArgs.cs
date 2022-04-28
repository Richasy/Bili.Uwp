// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Bilibili.Community.Service.Dm.V1;

namespace Bili.Models.App.Args
{
    /// <summary>
    /// 分片弹幕迭代事件参数.
    /// </summary>
    public class SegmentDanmakuIterationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentDanmakuIterationEventArgs"/> class.
        /// </summary>
        /// <param name="reply">响应结果.</param>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">分P Id.</param>
        public SegmentDanmakuIterationEventArgs(DmSegMobileReply reply, long videoId, long partId)
        {
            VideoId = videoId;
            PartId = partId;
            DanmakuList = reply.Elems.ToList();
        }

        /// <summary>
        /// 视频Id.
        /// </summary>
        public long VideoId { get; set; }

        /// <summary>
        /// 分P Id.
        /// </summary>
        public long PartId { get; set; }

        /// <summary>
        /// 弹幕响应结果.
        /// </summary>
        public List<DanmakuElem> DanmakuList { get; set; }
    }
}

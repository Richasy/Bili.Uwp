// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Video
{
    /// <summary>
    /// 历史记录视图.
    /// </summary>
    public sealed class VideoHistoryView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoHistoryView"/> class.
        /// </summary>
        /// <param name="items">视频列表.</param>
        /// <param name="isFinished">是否已经请求完全部数据.</param>
        public VideoHistoryView(
            IEnumerable<VideoInformation> items,
            bool isFinished)
        {
            Items = items;
            IsFinished = isFinished;
        }

        /// <summary>
        /// 视频列表.
        /// </summary>
        public IEnumerable<VideoInformation> Items { get; }

        /// <summary>
        /// 是否已经请求完全部数据.
        /// </summary>
        public bool IsFinished { get; }
    }
}

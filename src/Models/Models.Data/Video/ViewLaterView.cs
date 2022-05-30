// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Video
{
    /// <summary>
    /// 稍后再看视图.
    /// </summary>
    public sealed class ViewLaterView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewLaterView"/> class.
        /// </summary>
        /// <param name="items">视频列表.</param>
        /// <param name="totalCount">视频总数.</param>
        public ViewLaterView(
            IEnumerable<VideoInformation> items,
            int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }

        /// <summary>
        /// 视频列表.
        /// </summary>
        public IEnumerable<VideoInformation> Items { get; }

        /// <summary>
        /// 总数.
        /// </summary>
        public int TotalCount { get; }
    }
}

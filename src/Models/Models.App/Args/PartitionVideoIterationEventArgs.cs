// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 视频批量传输事件，用于传输视频列表.
    /// </summary>
    public class PartitionVideoIterationEventArgs : PartitionEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionVideoIterationEventArgs"/> class.
        /// </summary>
        /// <param name="subPartitionId">子分区Id.</param>
        /// <param name="requestDateTime">请求发生时间.</param>
        /// <param name="partionData">子分区数据.</param>
        /// <param name="nextPageNumber">下一页码.</param>
        public PartitionVideoIterationEventArgs(
            int subPartitionId,
            DateTimeOffset requestDateTime,
            SubPartition partionData,
            int nextPageNumber)
            : base(subPartitionId, requestDateTime)
        {
            this.SubPartitionId = subPartitionId;
            this.TopOffsetId = partionData.TopOffsetId;
            this.BottomOffsetId = partionData.BottomOffsetId;
            this.RequestDateTime = requestDateTime;
            this.NextPageNumber = nextPageNumber;

            var videoList = new List<PartitionVideo>();
            if (partionData.NewVideos?.Any() ?? false)
            {
                videoList = videoList.Concat(partionData.NewVideos).ToList();
            }

            if (partionData.RecommendVideos?.Any() ?? false)
            {
                videoList = videoList.Concat(partionData.RecommendVideos).ToList();
            }

            this.VideoList = videoList;
        }

        /// <summary>
        /// 视频列表.
        /// </summary>
        public List<PartitionVideo> VideoList { get; set; }

        /// <summary>
        /// 向上偏移标识符.
        /// </summary>
        public int TopOffsetId { get; set; }

        /// <summary>
        /// 向下偏移标识符.
        /// </summary>
        public int BottomOffsetId { get; set; }

        /// <summary>
        /// 下一个页码.
        /// </summary>
        public int NextPageNumber { get; set; }
    }
}

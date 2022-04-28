// Copyright (c) Richasy. All rights reserved.

using System;

namespace Bili.Models.App.Args
{
    /// <summary>
    /// 分区事件参数基类.
    /// </summary>
    public class PartitionEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionEventArgs"/> class.
        /// </summary>
        /// <param name="requestDateTime">请求发生的时间.</param>
        public PartitionEventArgs(DateTimeOffset requestDateTime)
        {
            this.RequestDateTime = requestDateTime;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionEventArgs"/> class.
        /// </summary>
        /// <param name="subPartitionId">子分区Id.</param>
        /// <param name="requestDateTime">请求发生的时间.</param>
        public PartitionEventArgs(int subPartitionId, DateTimeOffset requestDateTime)
            : this(requestDateTime)
        {
            this.SubPartitionId = subPartitionId;
        }

        /// <summary>
        /// 子分区标识符.
        /// </summary>
        public int SubPartitionId { get; set; }

        /// <summary>
        /// 请求发生时的时间.
        /// </summary>
        public DateTimeOffset RequestDateTime { get; set; }
    }
}

// Copyright (c) GodLeaveMe. All rights reserved.

using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Models.App.Other
{
    /// <summary>
    /// 排行榜分区.
    /// </summary>
    public class RankPartition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RankPartition"/> class.
        /// </summary>
        /// <param name="partitionName">分区名称.</param>
        /// <param name="logo">图标.</param>
        /// <param name="partitionId">分区Id.</param>
        public RankPartition(string partitionName, string logo, int partitionId = 0)
        {
            this.PartitionName = partitionName;
            this.PartitionId = partitionId;
            this.Logo = logo;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RankPartition"/> class.
        /// </summary>
        /// <param name="partition"><see cref="Partition"/>的实例.</param>
        public RankPartition(Partition partition)
        {
            this.PartitionId = partition.Tid;
            this.PartitionName = partition.Name;
            this.Logo = partition.Logo;
        }

        /// <summary>
        /// 分区Id.
        /// </summary>
        public int PartitionId { get; set; }

        /// <summary>
        /// 分区名称.
        /// </summary>
        public string PartitionName { get; set; }

        /// <summary>
        /// 图标.
        /// </summary>
        public string Logo { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is RankPartition partition && PartitionId == partition.PartitionId;

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            var hashCode = 1323653111;
            hashCode = (hashCode * -1521134295) + PartitionId.GetHashCode();
            return hashCode;
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Live
{
    /// <summary>
    /// 直播分区详情视图.
    /// </summary>
    public sealed class LivePartitionView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LivePartitionView"/> class.
        /// </summary>
        /// <param name="totalCount">分区直播间总数.</param>
        /// <param name="lives">直播间列表.</param>
        /// <param name="tags">分类标签列表.</param>
        public LivePartitionView(
            int totalCount,
            IEnumerable<LiveInformation> lives,
            IEnumerable<LiveTag> tags)
        {
            TotalCount = totalCount;
            Lives = lives;
            Tags = tags;
        }

        /// <summary>
        /// 分区标识符.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 直播列表.
        /// </summary>
        public IEnumerable<LiveInformation> Lives { get; }

        /// <summary>
        /// 分类标签列表.
        /// </summary>
        public IEnumerable<LiveTag> Tags { get; }

        /// <summary>
        /// 分区直播间总数.
        /// </summary>
        public int TotalCount { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is LivePartitionView view && EqualityComparer<string>.Default.Equals(Id, view.Id);

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}

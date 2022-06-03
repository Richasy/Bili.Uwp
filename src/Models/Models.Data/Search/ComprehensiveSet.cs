// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Video;
using Bili.Models.Enums;

namespace Bili.Models.Data.Search
{
    /// <summary>
    /// 综合搜索结果集.
    /// </summary>
    public sealed class ComprehensiveSet
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ComprehensiveSet"/> class.
        /// </summary>
        /// <param name="videoSet">视频条目.</param>
        /// <param name="metadata">元数据.</param>
        public ComprehensiveSet(
            SearchSet<VideoInformation> videoSet,
            Dictionary<SearchModuleType, int> metadata = default)
        {
            Metadata = metadata;
            VideoSet = videoSet;
        }

        /// <summary>
        /// 元数据，包含子模块的结果总数.
        /// </summary>
        public Dictionary<SearchModuleType, int> Metadata { get; }

        /// <summary>
        /// 综合搜索的视频结果.
        /// </summary>
        public SearchSet<VideoInformation> VideoSet { get; }
    }
}

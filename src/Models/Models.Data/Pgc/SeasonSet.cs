// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Local;

namespace Bili.Models.Data.Pgc
{
    /// <summary>
    /// 剧集内容集.
    /// </summary>
    public sealed class SeasonSet : ContentSet<SeasonInformation>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonSet"/> class.
        /// </summary>
        /// <param name="items">剧集列表.</param>
        /// <param name="totalCount">剧集总数.</param>
        public SeasonSet(
            IEnumerable<SeasonInformation> items,
            int totalCount)
        {
            Items = items;
            TotalCount = totalCount;
        }
    }
}

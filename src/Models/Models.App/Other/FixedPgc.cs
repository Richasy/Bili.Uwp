// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Richasy.Bili.Models.App
{
    /// <summary>
    /// 固定的剧集内容.
    /// </summary>
    public class FixedPgc
    {
        /// <summary>
        /// 剧集名.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 封面地址.
        /// </summary>
        public string CoverUrl { get; set; }

        /// <summary>
        /// 剧集Id.
        /// </summary>
        public string SeasonId { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is FixedPgc pgc && SeasonId == pgc.SeasonId;

        /// <inheritdoc/>
        public override int GetHashCode() => 415255181 + EqualityComparer<string>.Default.GetHashCode(SeasonId);
    }
}

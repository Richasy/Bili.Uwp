// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Community;

namespace Bili.Models.Data.Pgc
{
    /// <summary>
    /// PGC 页面视图.
    /// </summary>
    public sealed class PgcPageView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcPageView"/> class.
        /// </summary>
        /// <param name="banners">横幅列表.</param>
        /// <param name="ranks">排行榜数据.</param>
        /// <param name="playlists">播放列表.</param>
        /// <param name="videoPartitionId">视频分区 Id.</param>
        public PgcPageView(
            IEnumerable<BannerIdentifier> banners,
            Dictionary<string, IEnumerable<EpisodeInformation>> ranks = default,
            IEnumerable<PgcPlaylist> playlists = default,
            string videoPartitionId = default)
        {
            Banners = banners;
            Ranks = ranks;
            Playlists = playlists;
            VideoPartitionId = videoPartitionId;
        }

        /// <summary>
        /// 横幅列表.
        /// </summary>
        public IEnumerable<BannerIdentifier> Banners { get; }

        /// <summary>
        /// 排行榜列表.
        /// </summary>
        public Dictionary<string, IEnumerable<EpisodeInformation>> Ranks { get; }

        /// <summary>
        /// 播放列表.
        /// </summary>
        public IEnumerable<PgcPlaylist> Playlists { get; }

        /// <summary>
        /// 该 PGC 视图所属的视频分区 Id.
        /// </summary>
        public string VideoPartitionId { get; }
    }
}

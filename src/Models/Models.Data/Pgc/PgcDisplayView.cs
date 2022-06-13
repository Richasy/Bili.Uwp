// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Player;
using Bili.Models.Data.Video;

namespace Bili.Models.Data.Pgc
{
    /// <summary>
    /// PGC 视图信息.
    /// </summary>
    public sealed class PgcDisplayView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcDisplayView"/> class.
        /// </summary>
        /// <param name="information">剧集当季信息.</param>
        /// <param name="seasons">关联剧集信息.</param>
        /// <param name="episodes">分集列表.</param>
        /// <param name="extras">附加内容.</param>
        /// <param name="progress">播放进度.</param>
        /// <param name="warning">播放警告.</param>
        public PgcDisplayView(
            SeasonInformation information,
            IEnumerable<VideoIdentifier> seasons = null,
            IEnumerable<EpisodeInformation> episodes = null,
            Dictionary<string, IEnumerable<EpisodeInformation>> extras = null,
            PlayedProgress progress = null,
            string warning = default)
        {
            Information = information;
            Seasons = seasons;
            Episodes = episodes;
            Extras = extras;
            Progress = progress;
            Warning = warning;
        }

        /// <summary>
        /// 剧集信息.
        /// </summary>
        public SeasonInformation Information { get; }

        /// <summary>
        /// 关联的 PGC 内容季度信息.
        /// </summary>
        public IEnumerable<VideoIdentifier> Seasons { get; }

        /// <summary>
        /// 剧集列表.
        /// </summary>
        public IEnumerable<EpisodeInformation> Episodes { get; }

        /// <summary>
        /// 附加的视频列表，比如预告片、宣传片、片花等.
        /// </summary>
        public Dictionary<string, IEnumerable<EpisodeInformation>> Extras { get; }

        /// <summary>
        /// 上次播放进度.
        /// </summary>
        public PlayedProgress Progress { get; set; }

        /// <summary>
        /// 播放警告.
        /// </summary>
        public string Warning { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is PgcDisplayView view && EqualityComparer<SeasonInformation>.Default.Equals(Information, view.Information);

        /// <inheritdoc/>
        public override int GetHashCode() => Information.GetHashCode();
    }
}

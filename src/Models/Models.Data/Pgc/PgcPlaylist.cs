// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Pgc
{
    /// <summary>
    /// PGC 播放列表.
    /// </summary>
    public sealed class PgcPlaylist
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcPlaylist"/> class.
        /// </summary>
        /// <param name="title">标题.</param>
        /// <param name="id">标识符.</param>
        /// <param name="subtitle">副标题.</param>
        /// <param name="seasons">剧集列表.</param>
        public PgcPlaylist(
            string title,
            string id,
            string subtitle = default,
            IEnumerable<SeasonInformation> seasons = default)
        {
            Title = title;
            Id = id;
            Subtitle = subtitle;
            Seasons = seasons;
        }

        /// <summary>
        /// 标题.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// 标识符.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 副标题.
        /// </summary>
        public string Subtitle { get; }

        /// <summary>
        /// 剧集列表.
        /// </summary>
        public IEnumerable<SeasonInformation> Seasons { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is PgcPlaylist playlist && Id == playlist.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}

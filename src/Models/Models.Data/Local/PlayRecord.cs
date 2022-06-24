// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Video;

namespace Bili.Models.Data.Local
{
    /// <summary>
    /// 播放记录.
    /// </summary>
    public sealed class PlayRecord
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayRecord"/> class.
        /// </summary>
        public PlayRecord(VideoIdentifier identifier, PlaySnapshot snapshot)
        {
            Identifier = identifier;
            Snapshot = snapshot;
        }

        /// <summary>
        /// 视频基本信息.
        /// </summary>
        public VideoIdentifier Identifier { get; }

        /// <summary>
        /// 播放参数.
        /// </summary>
        public PlaySnapshot Snapshot { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is PlayRecord record && EqualityComparer<VideoIdentifier>.Default.Equals(Identifier, record.Identifier);

        /// <inheritdoc/>
        public override int GetHashCode() => Identifier.GetHashCode();
    }
}

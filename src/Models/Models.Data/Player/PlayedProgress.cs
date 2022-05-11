// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Video;
using Bili.Models.Enums.Player;

namespace Bili.Models.Data.Player
{
    /// <summary>
    /// 播放进度记录.
    /// </summary>
    public sealed class PlayedProgress
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayedProgress"/> class.
        /// </summary>
        /// <param name="progress">播放进度.</param>
        /// <param name="status">历史记录状态.</param>
        /// <param name="identifier">对应视频的标识符.</param>
        public PlayedProgress(
            double progress,
            PlayedProgressStatus status,
            VideoIdentifier identifier)
        {
            Progress = progress;
            Status = status;
            Identifier = identifier;
        }

        /// <summary>
        /// 已播放的进度（即播放了多少秒）.
        /// </summary>
        public double Progress { get; set; }

        /// <summary>
        /// 历史记录状态，如果未开始或者已经放完，则没有引导用户跳转到上一次播放记录点.
        /// </summary>
        public PlayedProgressStatus Status { get; set; }

        /// <summary>
        /// 进度对应的视频标识.
        /// </summary>
        public VideoIdentifier Identifier { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is PlayedProgress progress && EqualityComparer<VideoIdentifier>.Default.Equals(Identifier, progress.Identifier);

        /// <inheritdoc/>
        public override int GetHashCode() => Identifier.GetHashCode();
    }
}

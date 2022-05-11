// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Community;
using Bili.Models.Data.Player;

namespace Bili.Models.Data.Video
{
    /// <summary>
    /// 这个类囊括了视频的全部数据（除了播放数据）.
    /// </summary>
    public sealed class VideoView
    {
        /// <summary>
        /// 视频主要信息.
        /// </summary>
        public VideoInformation Information { get; }

        /// <summary>
        /// 分集信息.
        /// </summary>
        public IEnumerable<VideoIdentifier> SubVideos { get; }

        /// <summary>
        /// 合集信息.
        /// </summary>
        public IEnumerable<VideoSection> Sections { get; }

        /// <summary>
        /// 关联视频列表.
        /// </summary>
        public IEnumerable<VideoInformation> RelatedVideos { get; }

        /// <summary>
        /// 播放进度.
        /// </summary>
        public PlayedProgress Progress { get; }

        /// <summary>
        /// 用户对视频的操作信息.
        /// </summary>
        public VideoOpeartionInformation Operation { get; }

        /// <summary>
        /// 互动视频记录点.
        /// </summary>
        public InteractionVideoRecord InteractionVideo { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is VideoView view && EqualityComparer<VideoInformation>.Default.Equals(Information, view.Information);

        /// <inheritdoc/>
        public override int GetHashCode() => Information.GetHashCode();
    }
}

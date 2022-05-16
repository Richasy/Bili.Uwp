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
        /// Initializes a new instance of the <see cref="VideoView"/> class.
        /// </summary>
        /// <param name="information">视频信息.</param>
        /// <param name="publisherCommunityInformation">发布者的社区信息.</param>
        /// <param name="subVideos">分集.</param>
        /// <param name="sections">所属视频合集列表.</param>
        /// <param name="relatedVideos">关联视频.</param>
        /// <param name="progress">播放进度.</param>
        /// <param name="operation">视频操作信息.</param>
        /// <param name="interactionVideo">互动视频信息.</param>
        /// <param name="tags">视频标签列表.</param>
        public VideoView(
            VideoInformation information,
            UserCommunityInformation publisherCommunityInformation,
            IEnumerable<VideoIdentifier> subVideos,
            IEnumerable<VideoSection> sections,
            IEnumerable<VideoInformation> relatedVideos,
            PlayedProgress progress,
            VideoOpeartionInformation operation,
            InteractionVideoRecord interactionVideo,
            IEnumerable<Tag> tags)
        {
            Information = information;
            PublisherCommunityInformation = publisherCommunityInformation;
            SubVideos = subVideos;
            Sections = sections;
            RelatedVideos = relatedVideos;
            Progress = progress;
            Operation = operation;
            InteractionVideo = interactionVideo;
            Tags = tags;
        }

        /// <summary>
        /// 视频主要信息.
        /// </summary>
        public VideoInformation Information { get; }

        /// <summary>
        /// 发布者的社区信息.
        /// </summary>
        public UserCommunityInformation PublisherCommunityInformation { get; }

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

        /// <summary>
        /// 视频标签列表.
        /// </summary>
        public IEnumerable<Tag> Tags { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is VideoView view && EqualityComparer<VideoInformation>.Default.Equals(Information, view.Information);

        /// <inheritdoc/>
        public override int GetHashCode() => Information.GetHashCode();
    }
}

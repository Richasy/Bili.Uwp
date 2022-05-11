// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Bili.Models.Data.Community;
using Bili.Models.Data.User;

namespace Bili.Models.Data.Video
{
    /// <summary>
    /// 视频基础信息.
    /// </summary>
    public sealed class VideoInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoInformation"/> class.
        /// </summary>
        /// <param name="identifier">视频标识信息.</param>
        /// <param name="publisher">视频发布者信息.</param>
        /// <param name="collaborators">视频合作者信息.</param>
        /// <param name="otherId">视频的第二Id，对视频来说，指的是 bvid.</param>
        public VideoInformation(
            VideoIdentifier identifier,
            PublisherProfile publisher,
            string otherId = "",
            IEnumerable<PublisherProfile> collaborators = null)
            : this(identifier, publisher, otherId, string.Empty, string.Empty, default, collaborators)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoInformation"/> class.
        /// </summary>
        /// <param name="identifier">视频标识信息.</param>
        /// <param name="publisher">视频发布者信息.</param>
        /// <param name="otherId">第二Id，对视频来说，指的是 bvid.</param>
        /// <param name="description">视频描述.</param>
        /// <param name="subtitle">副标题.</param>
        /// <param name="publishTime">视频发布时间.</param>
        /// <param name="collaborators">视频合作者信息.</param>
        /// <param name="communityInformation">社区信息.</param>
        public VideoInformation(
            VideoIdentifier identifier,
            PublisherProfile publisher,
            string otherId = "",
            string description = "",
            string subtitle = "",
            DateTime publishTime = default,
            IEnumerable<PublisherProfile> collaborators = null,
            VideoCommunityInformation communityInformation = null)
        {
            Identifier = identifier;
            Publisher = publisher;
            Collaborators = collaborators;
            AlternateId = otherId;
            Description = description;
            Subtitle = subtitle;
            PublishTime = publishTime;
            CommunityInformation = communityInformation;
        }

        /// <summary>
        /// 标识符.
        /// </summary>
        public VideoIdentifier Identifier { get; }

        /// <summary>
        /// 备用 Id.
        /// </summary>
        /// <remarks>
        /// 该 Id 对于 BiliBili 视频来说是 bvid，在应用里，以 aid 作为主 id 进行数据处理.
        /// </remarks>
        public string AlternateId { get; set; }

        /// <summary>
        /// 发布时间.
        /// </summary>
        public DateTime PublishTime { get; }

        /// <summary>
        /// 发布者信息.
        /// </summary>
        public PublisherProfile Publisher { get; }

        /// <summary>
        /// 视频合作者列表.
        /// </summary>
        /// <remarks>
        /// 有时一个视频是由多位作者共同合作发布.
        /// </remarks>
        public IEnumerable<PublisherProfile> Collaborators { get; }

        /// <summary>
        /// 视频描述.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 视频副标题.
        /// </summary>
        public string Subtitle { get; }

        /// <summary>
        /// 社区交互数据.
        /// </summary>
        public VideoCommunityInformation CommunityInformation { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => obj is VideoInformation information && EqualityComparer<VideoIdentifier>.Default.Equals(Identifier, information.Identifier);

        /// <inheritdoc/>
        public override int GetHashCode() => Identifier.GetHashCode();
    }
}

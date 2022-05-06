// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Bili.Models.Data.Community;

namespace Bili.Models.Data.Player
{
    /// <summary>
    /// 视频基础信息.
    /// </summary>
    public sealed class VideoInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoInformation"/> class.
        /// </summary>
        public VideoInformation()
        {
            PublishTime = DateTime.MinValue;
            Description = string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoInformation"/> class.
        /// </summary>
        /// <param name="identifier">视频标识信息.</param>
        /// <param name="publisher">视频发布者信息.</param>
        /// <param name="collaborators">视频合作者信息.</param>
        public VideoInformation(VideoIdentifier identifier, PublisherProfile publisher, IEnumerable<PublisherProfile> collaborators = null)
            : this()
        {
            Identifier = identifier;
            Publisher = publisher;
            Collaborators = collaborators;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoInformation"/> class.
        /// </summary>
        /// <param name="identifier">视频标识信息.</param>
        /// <param name="publisher">视频发布者信息.</param>
        /// <param name="description">视频描述.</param>
        /// <param name="publishTime">视频发布时间.</param>
        /// <param name="collaborators">视频合作者信息.</param>
        public VideoInformation(VideoIdentifier identifier, PublisherProfile publisher, string description, DateTime publishTime, IEnumerable<PublisherProfile> collaborators = null)
            : this(identifier, publisher, collaborators)
        {
            Description = description;
            PublishTime = publishTime;
        }

        /// <summary>
        /// 标识符.
        /// </summary>
        public VideoIdentifier Identifier { get; }

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

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => obj is VideoInformation information && EqualityComparer<VideoIdentifier>.Default.Equals(Identifier, information.Identifier);

        /// <inheritdoc/>
        public override int GetHashCode() => Identifier.GetHashCode();
    }
}

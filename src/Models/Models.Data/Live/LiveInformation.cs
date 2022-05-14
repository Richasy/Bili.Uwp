// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.User;
using Bili.Models.Data.Video;
using Bili.Models.Enums.Community;

namespace Bili.Models.Data.Live
{
    /// <summary>
    /// 直播信息.
    /// </summary>
    public sealed class LiveInformation : IVideoBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveInformation"/> class.
        /// </summary>
        /// <param name="identifier">直播间标识.</param>
        /// <param name="user">正在直播的用户资料.</param>
        /// <param name="viewerCount">观看人数.</param>
        /// <param name="relation">与直播UP的关系.</param>
        /// <param name="subtitle">直播间副标题.</param>
        /// <param name="description">直播间描述.</param>
        public LiveInformation(
            VideoIdentifier identifier,
            UserProfile user,
            double viewerCount = -1,
            UserRelationStatus relation = UserRelationStatus.Unknown,
            string subtitle = default,
            string description = default)
        {
            Identifier = identifier;
            User = user;
            ViewerCount = viewerCount;
            Relation = relation;
            Subtitle = subtitle;
            Description = description;
        }

        /// <inheritdoc/>
        public VideoIdentifier Identifier { get; }

        /// <summary>
        /// 直播 UP 主信息.
        /// </summary>
        public UserProfile User { get; }

        /// <summary>
        /// 与 UP 主的关系.
        /// </summary>
        public UserRelationStatus Relation { get; set; }

        /// <summary>
        /// 副标题.
        /// </summary>
        public string Subtitle { get; }

        /// <summary>
        /// 直播间描述.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 在线观看人数.
        /// </summary>
        public double ViewerCount { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is LiveInformation information && EqualityComparer<VideoIdentifier>.Default.Equals(Identifier, information.Identifier);

        /// <inheritdoc/>
        public override int GetHashCode() => Identifier.GetHashCode();
    }
}

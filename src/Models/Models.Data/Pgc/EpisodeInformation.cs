// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Bili.Models.Data.Community;
using Bili.Models.Data.Video;

namespace Bili.Models.Data.Pgc
{
    /// <summary>
    /// PGC内容的单集.
    /// </summary>
    public sealed class EpisodeInformation : IVideoBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodeInformation"/> class.
        /// </summary>
        /// <param name="identifier">单集标识.</param>
        /// <param name="seasonId">剧集Id.</param>
        /// <param name="videoId">备用Id.</param>
        /// <param name="subtitle">副标题.</param>
        /// <param name="index">索引.</param>
        /// <param name="isVip">是否为会员专属剧集.</param>
        /// <param name="isPv">是否为预告片.</param>
        /// <param name="publishTime">发布时间.</param>
        /// <param name="communityInformation">社区信息.</param>
        /// <param name="highlight">高亮文本.</param>
        /// <param name="partId">分P Id.</param>
        /// <param name="seasonType">剧集类型.</param>
        public EpisodeInformation(
            VideoIdentifier identifier,
            string seasonId = "",
            string videoId = "",
            string subtitle = "",
            int index = -1,
            bool isVip = false,
            bool isPv = false,
            DateTime publishTime = default,
            VideoCommunityInformation communityInformation = default,
            string highlight = default,
            string partId = default,
            string seasonType = default)
        {
            Identifier = identifier;
            SeasonId = seasonId;
            SeasonType = seasonType;
            VideoId = videoId;
            PartId = partId;
            Subtitle = subtitle;
            Index = index;
            IsVip = isVip;
            IsPreviewVideo = isPv;
            PublishTime = publishTime;
            CommunityInformation = communityInformation;
            Highlight = highlight;
        }

        /// <summary>
        /// 视频标识符，包含单集的基本信息.
        /// </summary>
        public VideoIdentifier Identifier { get; }

        /// <summary>
        /// 备用 Id.
        /// </summary>
        /// <remarks>
        /// 有时对番剧单集的数据请求需要其 aid，这里用来存储其对应数据.
        /// </remarks>
        public string VideoId { get; set; }

        /// <summary>
        /// 分P Id，通常指分集作为视频时的 cid.
        /// </summary>
        public string PartId { get; set; }

        /// <summary>
        /// 发布时间.
        /// </summary>
        public DateTime PublishTime { get; }

        /// <summary>
        /// 所属剧集Id.
        /// </summary>
        public string SeasonId { get; }

        /// <summary>
        /// 剧集类型.
        /// </summary>
        public string SeasonType { get; }

        /// <summary>
        /// 分集排序索引.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// 副标题.
        /// </summary>
        public string Subtitle { get; set; }

        /// <summary>
        /// 是否为预告片.
        /// </summary>
        public bool IsPreviewVideo { get; }

        /// <summary>
        /// 是否为会员专属剧集.
        /// </summary>
        public bool IsVip { get; set; }

        /// <summary>
        /// 高亮文本.
        /// </summary>
        public string Highlight { get; set; }

        /// <summary>
        /// 社区信息.
        /// </summary>
        public VideoCommunityInformation CommunityInformation { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is EpisodeInformation information && EqualityComparer<VideoIdentifier>.Default.Equals(Identifier, information.Identifier);

        /// <inheritdoc/>
        public override int GetHashCode() => Identifier.GetHashCode();
    }
}

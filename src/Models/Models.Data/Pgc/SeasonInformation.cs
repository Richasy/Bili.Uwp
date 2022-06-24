// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Community;
using Bili.Models.Data.User;
using Bili.Models.Data.Video;
using Bili.Models.Enums;

namespace Bili.Models.Data.Pgc
{
    /// <summary>
    /// 剧集信息.
    /// </summary>
    public sealed class SeasonInformation : IVideoBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonInformation"/> class.
        /// </summary>
        /// <param name="identifier">剧集标识.</param>
        /// <param name="subtitle">副标题.</param>
        /// <param name="highlight">高亮信息.</param>
        /// <param name="tags">标签文本.</param>
        /// <param name="ratingCount">评分人数.</param>
        /// <param name="originName">原始名称.</param>
        /// <param name="alias">别名.</param>
        /// <param name="releaseDate">发布时间.</param>
        /// <param name="progress">连载进度.</param>
        /// <param name="description">剧集简介.</param>
        /// <param name="type">PGC 内容类型.</param>
        /// <param name="labors">工作人员说明.</param>
        /// <param name="communityInformation">社区交互信息.</param>
        /// <param name="celebrities">参演明星.</param>
        /// <param name="isTracking">是否已追番/追剧.</param>
        public SeasonInformation(
            VideoIdentifier identifier,
            string subtitle,
            string highlight = default,
            string tags = default,
            int ratingCount = 0,
            string originName = default,
            string alias = default,
            string releaseDate = default,
            string progress = default,
            string description = default,
            PgcType type = default,
            Dictionary<string, string> labors = default,
            VideoCommunityInformation communityInformation = default,
            IEnumerable<RoleProfile> celebrities = default,
            bool isTracking = false)
        {
            Identifier = identifier;
            Subtitle = subtitle;
            HighlightTitle = highlight;
            Tags = tags;
            RatingCount = ratingCount;
            OriginName = originName;
            Alias = alias;
            DisplayReleaseDate = releaseDate;
            DisplayProgress = progress;
            Description = description;
            LaborSections = labors;
            CommunityInformation = communityInformation;
            Celebrities = celebrities;
            Type = type;
            IsTracking = isTracking;
        }

        /// <summary>
        /// 剧集标识符，其 duration 属性一般是无效的.
        /// </summary>
        public VideoIdentifier Identifier { get; }

        /// <summary>
        /// 副标题.
        /// </summary>
        public string Subtitle { get; }

        /// <summary>
        /// 高亮标题，表示该剧的一些促销信息，比如 <c>限时免费</c>，<c>会员抢先</c> 等.
        /// </summary>
        public string HighlightTitle { get; }

        /// <summary>
        /// 剧集涉及的标签.
        /// </summary>
        public string Tags { get; }

        /// <summary>
        /// 评分人数.
        /// </summary>
        public int RatingCount { get; }

        /// <summary>
        /// 剧集的原始名称，比如番剧的原始名称就是日文名.
        /// </summary>
        public string OriginName { get; }

        /// <summary>
        /// 剧集的别名，比如有些番剧除了日文名、中文名之外还有英文名.
        /// </summary>
        public string Alias { get; }

        /// <summary>
        /// 显示的发布日期可读文本，比如 <c>2022年5月1日开播</c>.
        /// </summary>
        public string DisplayReleaseDate { get; }

        /// <summary>
        /// 显示的连载进度可读文本，比如 <c>已完结，共1话</c>.
        /// </summary>
        public string DisplayProgress { get; }

        /// <summary>
        /// 剧集简介.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// PGC 内容类型.
        /// </summary>
        public PgcType Type { get; set; }

        /// <summary>
        /// 台前幕后相关人员的说明.
        /// </summary>
        /// <remarks>
        /// 一般来说，服务器返回的演职人员及制作人员的信息都是换行文本，采用 <c>标题</c> + <c>内容</c> 的组织形式.
        /// 这里暂不做改动，<c>Key</c> 表示该区块的标题，比如 <c>角色声优</c>，<c>Value</c> 表示具体的内容.
        /// </remarks>
        public Dictionary<string, string> LaborSections { get; }

        /// <summary>
        /// 社区交互信息.
        /// </summary>
        public VideoCommunityInformation CommunityInformation { get; }

        /// <summary>
        /// 参演人员中的明星，他们有自己的头像和角色说明.
        /// </summary>
        public IEnumerable<RoleProfile> Celebrities { get; }

        /// <summary>
        /// 是否追番/追剧.
        /// </summary>
        public bool IsTracking { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SeasonInformation information && EqualityComparer<VideoIdentifier>.Default.Equals(Identifier, information.Identifier);

        /// <inheritdoc/>
        public override int GetHashCode() => Identifier.GetHashCode();
    }
}

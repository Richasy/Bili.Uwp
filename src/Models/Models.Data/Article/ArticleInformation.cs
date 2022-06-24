// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Bili.Models.Data.Community;
using Bili.Models.Data.User;

namespace Bili.Models.Data.Article
{
    /// <summary>
    /// 专栏文章信息.
    /// </summary>
    public sealed class ArticleInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleInformation"/> class.
        /// </summary>
        /// <param name="identifier">文章标识符.</param>
        /// <param name="subtitle">副标题.</param>
        /// <param name="partition">文章所属分区.</param>
        /// <param name="relatedPartitions">文章关联的分区.</param>
        /// <param name="user">用户资料.</param>
        /// <param name="publishDateTime">发布时间.</param>
        /// <param name="communityInformation">社区信息.</param>
        /// <param name="wordCount">字数.</param>
        public ArticleInformation(
            ArticleIdentifier identifier,
            string subtitle,
            Partition partition = default,
            IEnumerable<Partition> relatedPartitions = default,
            RoleProfile user = default,
            DateTime publishDateTime = default,
            ArticleCommunityInformation communityInformation = default,
            int wordCount = default)
        {
            Identifier = identifier;
            Subtitle = subtitle;
            Partition = partition;
            RelatedPartitions = relatedPartitions;
            Publisher = user;
            PublishDateTime = publishDateTime;
            CommunityInformation = communityInformation;
            WordCount = wordCount;
        }

        /// <summary>
        /// 文章标识.
        /// </summary>
        public ArticleIdentifier Identifier { get; }

        /// <summary>
        /// 副标题.
        /// </summary>
        public string Subtitle { get; }

        /// <summary>
        /// 文章所属分区.
        /// </summary>
        public Partition Partition { get; }

        /// <summary>
        /// 文章关联分区.
        /// </summary>
        public IEnumerable<Partition> RelatedPartitions { get; }

        /// <summary>
        /// 发布者.
        /// </summary>
        public RoleProfile Publisher { get; }

        /// <summary>
        /// 发布时间.
        /// </summary>
        public DateTime PublishDateTime { get; }

        /// <summary>
        /// 文章社区信息，包含点赞数、阅读数等.
        /// </summary>
        public ArticleCommunityInformation CommunityInformation { get; set; }

        /// <summary>
        /// 文章字数.
        /// </summary>
        public int WordCount { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is ArticleInformation information && EqualityComparer<ArticleIdentifier>.Default.Equals(Identifier, information.Identifier);

        /// <inheritdoc/>
        public override int GetHashCode() => Identifier.GetHashCode();
    }
}

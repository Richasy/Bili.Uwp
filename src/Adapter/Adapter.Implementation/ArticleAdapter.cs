// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Bili.Adapter.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.Article;
using Bili.Models.Data.Community;
using Bilibili.App.Dynamic.V2;
using Humanizer;

namespace Bili.Adapter
{
    /// <summary>
    /// 文章数据适配器.
    /// </summary>
    public sealed class ArticleAdapter : IArticleAdapter
    {
        private readonly IImageAdapter _imageAdapter;
        private readonly IUserAdapter _userAdapter;
        private readonly ICommunityAdapter _communityAdapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleAdapter"/> class.
        /// </summary>
        /// <param name="imageAdapter">图片数据适配器.</param>
        /// <param name="userAdapter">用户数据适配器.</param>
        /// <param name="communityAdapter">社区数据适配器.</param>
        public ArticleAdapter(
            IImageAdapter imageAdapter,
            IUserAdapter userAdapter,
            ICommunityAdapter communityAdapter)
        {
            _imageAdapter = imageAdapter;
            _userAdapter = userAdapter;
            _communityAdapter = communityAdapter;
        }

        /// <inheritdoc/>
        public ArticleInformation ConvertToArticleInformation(Article article)
        {
            var id = article.Id.ToString();
            var title = article.Title;
            var summary = article.Summary;
            var cover = article.CoverUrls?.Any() ?? false
                ? _imageAdapter.ConvertToArticleCardCover(article.CoverUrls.First())
                : null;
            var partition = _communityAdapter.ConvertToPartition(article.Category);
            var relatedPartitions = article.RelatedCategories?.Any() ?? false
                ? article.RelatedCategories.Select(p => _communityAdapter.ConvertToPartition(p)).ToList()
                : null;
            var publishTime = DateTimeOffset.FromUnixTimeSeconds(article.PublishTime).ToLocalTime().DateTime;
            var user = _userAdapter.ConvertToRoleProfile(article.Publisher, Models.Enums.App.AvatarSize.Size48);
            var subtitle = $"{user.User.Name} · {publishTime.Humanize()}";
            var wordCount = article.WordCount;
            var communityInfo = _communityAdapter.ConvertToArticleCommunityInformation(article.Stats, id);
            var identifier = new ArticleIdentifier(id, title, summary, cover);
            return new ArticleInformation(
                identifier,
                subtitle,
                partition,
                relatedPartitions,
                user,
                publishTime,
                communityInfo,
                wordCount);
        }

        /// <inheritdoc/>
        public ArticleInformation ConvertToArticleInformation(ArticleSearchItem item)
        {
            var id = item.Id.ToString();
            var title = Regex.Replace(item.Title, "<[^>]+>", string.Empty);
            var summary = item.Description;
            var cover = item.CoverUrls?.Any() ?? false
                ? _imageAdapter.ConvertToArticleCardCover(item.CoverUrls.First())
                : null;
            var subtitle = item.Name;
            var communityInfo = _communityAdapter.ConvertToArticleCommunityInformation(item);
            var identifier = new ArticleIdentifier(id, title, summary, cover);
            return new ArticleInformation(
                identifier,
                subtitle,
                communityInformation: communityInfo);
        }

        /// <inheritdoc/>
        public ArticleInformation ConvertToArticleInformation(FavoriteArticleItem item)
        {
            var id = item.Id.ToString();
            var title = item.Title;
            var summary = item.Summary;
            var cover = item.Images?.Any() ?? false
                ? _imageAdapter.ConvertToArticleCardCover(item.Images.First())
                : null;
            var collectTime = DateTimeOffset.FromUnixTimeSeconds(item.CollectTime).DateTime;
            var subtitle = $"{collectTime.Humanize()}收藏";
            var identifier = new ArticleIdentifier(id, title, summary, cover);
            return new ArticleInformation(
                identifier,
                subtitle);
        }

        /// <inheritdoc/>
        public ArticleInformation ConvertToArticleInformation(MdlDynArticle article)
        {
            var id = article.Id.ToString();
            var title = article.Title;
            var summary = article.Desc;
            var cover = article.Covers?.Any() ?? false
                ? _imageAdapter.ConvertToArticleCardCover(article.Covers.First())
                : null;
            var identifier = new ArticleIdentifier(id, title, summary, cover);
            return new ArticleInformation(
                identifier,
                article.Label);
        }

        /// <inheritdoc/>
        public ArticlePartitionView ConvertToArticlePartitionView(ArticleRecommendResponse response)
        {
            var articles = response.Articles?.Any() ?? false
                ? response.Articles.Select(p => ConvertToArticleInformation(p))
                : null;

            var ranks = response.Ranks?.Any() ?? false
                ? response.Ranks.Select(p => ConvertToArticleInformation(p))
                : null;

            IEnumerable<BannerIdentifier> banners = null;
            if (response.Banners?.Any() ?? false)
            {
                var tempBanners = response.Banners.ToList();
                tempBanners.ForEach(p => p.NavigateUri = $"https://www.bilibili.com/read/cv{p.Id}");
                banners = tempBanners.Select(p => _communityAdapter.ConvertToBannerIdentifier(p));
            }

            return new ArticlePartitionView(articles, banners, ranks);
        }

        /// <inheritdoc/>
        public ArticlePartitionView ConvertToArticlePartitionView(IEnumerable<Article> articles)
        {
            var items = articles.Select(p => ConvertToArticleInformation(p));
            return new ArticlePartitionView(items);
        }

        /// <inheritdoc/>
        public ArticleSet ConvertToArticleSet(ArticleFavoriteListResponse response)
        {
            var count = response.Count;
            var items = response.Items.Select(p => ConvertToArticleInformation(p));
            return new ArticleSet(items, count);
        }
    }
}

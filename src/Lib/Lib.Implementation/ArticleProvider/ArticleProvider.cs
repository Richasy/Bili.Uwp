// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.BiliBili;
using Bili.Models.Data.Article;
using Bili.Models.Enums;
using HtmlAgilityPack;
using static Bili.Models.App.Constants.ServiceConstants;

namespace Bili.Lib.Uwp
{
    /// <summary>
    /// 提供专栏文章相关的操作.
    /// </summary>
    public partial class ArticleProvider : IArticleProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleProvider"/> class.
        /// </summary>
        /// <param name="httpProvider">处理网络的工具.</param>
        /// <param name="accountProvider">处理账户信息的工具.</param>
        /// <param name="articleAdapter">文章数据适配器.</param>
        /// <param name="communityAdapter">社区数据适配器.</param>
        public ArticleProvider(
            IHttpProvider httpProvider,
            IAccountProvider accountProvider,
            IArticleAdapter articleAdapter,
            ICommunityAdapter communityAdapter)
        {
            _httpProvider = httpProvider;
            _accountProvider = accountProvider;
            _articleAdapter = articleAdapter;
            _communityAdapter = communityAdapter;

            _partitionCache = new Dictionary<string, (ArticleSortType Sort, int PageNumber)>();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Models.Data.Community.Partition>> GetPartitionsAsync()
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Device, "phone" },
            };
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, ApiConstants.Article.Categories, queryParameters);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<List<ArticleCategory>>>(response);
            var partitions = result.Data.Select(p => _communityAdapter.ConvertToPartition(p)).ToList();
            partitions.Insert(0, new Models.Data.Community.Partition("0", "推荐"));
            return partitions;
        }

        /// <inheritdoc/>
        public async Task<ArticlePartitionView> GetPartitionArticlesAsync(string partitionId, ArticleSortType sortType)
        {
            var pageNumber = 1;
            if (_partitionCache.TryGetValue(partitionId, out var cache) && cache.Sort == sortType)
            {
                pageNumber = cache.PageNumber + 1;
            }

            var queryParameters = new Dictionary<string, string>
            {
                { Query.CategoryId, partitionId },
                { Query.PageNumber, pageNumber.ToString() },
                { Query.PageSizeSlim, "20" },
            };

            if (partitionId != "0")
            {
                queryParameters.Add(Query.Sort, ((int)sortType).ToString());
            }

            if (_accountProvider.UserId > 0)
            {
                queryParameters.Add(Query.MyId, _accountProvider.UserId.ToString());
            }

            var uri = partitionId == "0"
                ? ApiConstants.Article.Recommend
                : ApiConstants.Article.ArticleList;
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, uri, queryParameters);
            var response = await _httpProvider.SendAsync(request);
            ArticlePartitionView view;
            if (partitionId == "0")
            {
                var data = (await _httpProvider.ParseAsync<ServerResponse<ArticleRecommendResponse>>(response)).Data;
                view = _articleAdapter.ConvertToArticlePartitionView(data);
            }
            else
            {
                var data = (await _httpProvider.ParseAsync<ServerResponse<List<Article>>>(response)).Data;
                view = _articleAdapter.ConvertToArticlePartitionView(data);
            }

            _partitionCache.Remove(partitionId);
            _partitionCache.Add(partitionId, (sortType, pageNumber));
            return view;
        }

        /// <inheritdoc/>
        public async Task<string> GetArticleContentAsync(string articleId)
        {
            var url = ApiConstants.Article.ArticleContent + articleId;
            var html = await _httpProvider.HttpClient.GetStringAsync(url);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var node = doc.DocumentNode.SelectNodes("//div[contains(@class, 'article-holder')]").FirstOrDefault();
            return node?.InnerHtml;
        }

        /// <inheritdoc/>
        public void ResetPartitionStatus(string partitionId)
            => _partitionCache.Remove(partitionId);
    }
}

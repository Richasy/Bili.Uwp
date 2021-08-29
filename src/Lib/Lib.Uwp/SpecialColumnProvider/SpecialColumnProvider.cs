// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.App.Constants;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using static Richasy.Bili.Models.App.Constants.ServiceConstants;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 提供专栏文章相关的操作.
    /// </summary>
    public partial class SpecialColumnProvider : ISpecialColumnProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialColumnProvider"/> class.
        /// </summary>
        /// <param name="httpProvider">处理网络的工具.</param>
        /// <param name="accountProvider">处理账户信息的工具.</param>
        public SpecialColumnProvider(IHttpProvider httpProvider, IAccountProvider accountProvider)
        {
            _httpProvider = httpProvider;
            _accountProvider = accountProvider;
        }

        /// <inheritdoc/>
        public async Task<List<ArticleCategory>> GetCategoriesAsync()
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Device, "phone" },
            };
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, ApiConstants.Article.Categories, queryParameters);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse<List<ArticleCategory>>>(response);
            return result.Data;
        }

        /// <inheritdoc/>
        public async Task<List<Article>> GetCategoryArticlesAsync(int categoryId, int pageNumber, ArticleSortType sort, int pageSize = 20)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException("页码需大于等于1");
            }

            var queryParameters = new Dictionary<string, string>
            {
                { Query.CategoryId, categoryId.ToString() },
                { Query.PageNumber, pageNumber.ToString() },
                { Query.PageSizeSlim, pageSize.ToString() },
                { Query.Sort, ((int)sort).ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, ApiConstants.Article.ArticleList, queryParameters);
            var response = await _httpProvider.SendAsync(request);
            var parsedResponse = await _httpProvider.ParseAsync<ServerResponse<List<Article>>>(response);
            return parsedResponse.Data;
        }

        /// <inheritdoc/>
        public async Task<ArticleRecommendResponse> GetRecommendArticlesAsync(int pageNumber, int pageSize = 20)
        {
            if (pageNumber < 1)
            {
                throw new ArgumentOutOfRangeException("页码需大于等于1");
            }

            var queryParameters = new Dictionary<string, string>
            {
                { Query.CategoryId, "0" },
                { Query.PageNumber, pageNumber.ToString() },
                { Query.PageSizeSlim, pageSize.ToString() },
            };

            if (_accountProvider.UserId > 0)
            {
                queryParameters.Add(Query.MyId, _accountProvider.UserId.ToString());
            }

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, ApiConstants.Article.Recommend, queryParameters);
            var response = await _httpProvider.SendAsync(request);
            var parsedResponse = await _httpProvider.ParseAsync<ServerResponse<ArticleRecommendResponse>>(response);
            return parsedResponse.Data;
        }
    }
}

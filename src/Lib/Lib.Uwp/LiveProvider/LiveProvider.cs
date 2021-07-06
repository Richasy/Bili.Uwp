// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using static Richasy.Bili.Models.App.Constants.ServiceConstants;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 提供直播相关的数据操作.
    /// </summary>
    public partial class LiveProvider : ILiveProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveProvider"/> class.
        /// </summary>
        /// <param name="httpProvider">网络请求处理工具.</param>
        public LiveProvider(IHttpProvider httpProvider)
        {
            _httpProvider = httpProvider;
        }

        /// <inheritdoc/>
        public async Task<LiveFeedResponse> GetLiveFeedsAsync(int page)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Page, page.ToString() },
                { Query.RelationPage, page.ToString() },
                { Query.Scale, "2" },
            };
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Api.Live.LiveFeed, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<LiveFeedResponse>(response);

            return result;
        }
    }
}

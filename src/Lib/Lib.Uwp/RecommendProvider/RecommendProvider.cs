// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.BiliBili;

using static Richasy.Bili.Models.App.Constants.ApiConstants;
using static Richasy.Bili.Models.App.Constants.ServiceConstants;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 首页视频处理程序.
    /// </summary>
    public partial class RecommendProvider : IRecommendProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecommendProvider"/> class.
        /// </summary>
        /// <param name="httpProvider">网络处理工具.</param>
        public RecommendProvider(IHttpProvider httpProvider)
        {
            this._httpProvider = httpProvider;
        }

        /// <inheritdoc/>
        public async Task<List<RecommendCard>> RequestRecommendCardsAsync(int offsetIdx)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Idx, offsetIdx.ToString() },
                { Query.Flush, "5" },
                { Query.Column, "4" },
                { Query.Device, "pad" },
                { Query.DeviceName, "iPad 6" },
                { Query.Pull, (offsetIdx == 0).ToString().ToLower() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(
                HttpMethod.Get,
                Home.Recommend,
                queryParameters,
                Models.Enums.RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse<HomeRecommendInfo>>(response);
            return data.Data.Items.Where(p => !string.IsNullOrEmpty(p.Goto)).ToList();
        }
    }
}

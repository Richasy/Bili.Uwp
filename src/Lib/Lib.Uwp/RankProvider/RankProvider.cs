// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Bilibili.App.Show.V1;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.Storage;
using static Richasy.Bili.Models.App.Constants.ServiceConstants;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 提供排行榜相关的数据操作.
    /// </summary>
    public partial class RankProvider : IRankProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RankProvider"/> class.
        /// </summary>
        /// <param name="httpProvider">网络操作工具.</param>
        public RankProvider(IHttpProvider httpProvider)
        {
            this._httpProvider = httpProvider;
        }

        /// <inheritdoc/>
        public async Task<RankInfo> GetRankDetailAsync(int partitionId, RankScope rankScope)
        {
            // var queryParameters = new Dictionary<string, string>
            // {
            //     { Query.PartitionId, partitionId.ToString() },
            //     { Query.Type, rankScope.ToString().ToLower() },
            // };

            // var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Api.Rank.Ranking, queryParameters, RequestClientType.Web);
            // var response = await _httpProvider.SendAsync(request);
            // var data = await _httpProvider.ParseAsync<ServerResponse<RankInfo>>(response);
            // return data.Data;
            var rankRequst = new RankRegionResultReq() { Rid = partitionId };
            var request = await _httpProvider.GetRequestMessageAsync(Api.Rank.RankingGRPC, rankRequst);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync(response, RankListReply.Parser);
            return new RankInfo();
        }
    }
}

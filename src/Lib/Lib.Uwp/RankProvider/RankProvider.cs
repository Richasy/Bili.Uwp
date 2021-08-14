// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bilibili.App.Show.V1;
using Richasy.Bili.Lib.Interfaces;
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
        public async Task<List<Item>> GetRankDetailAsync(int partitionId)
        {
            var rankRequst = new RankRegionResultReq() { Rid = partitionId };
            var request = await _httpProvider.GetRequestMessageAsync(Api.Home.RankingGRPC, rankRequst);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync(response, RankListReply.Parser);
            return data.Items.ToList();
        }
    }
}

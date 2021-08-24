// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bilibili.App.Card.V1;
using Bilibili.App.Show.V1;
using Richasy.Bili.Lib.Interfaces;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 热门数据处理.
    /// </summary>
    public partial class PopularProvider : IPopularProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PopularProvider"/> class.
        /// </summary>
        /// <param name="httpProvider">网络操作工具.</param>
        /// <param name="authorizeProvider">授权工具.</param>
        public PopularProvider(IHttpProvider httpProvider, IAuthorizeProvider authorizeProvider)
        {
            this._httpProvider = httpProvider;
            this._authorizeProvider = authorizeProvider;
        }

        /// <inheritdoc/>
        public async Task<List<Card>> GetPopularDetailAsync(int offsetIndex)
        {
            var isLogin = _authorizeProvider.State == Models.Enums.AuthorizeState.SignedIn;
            var popularReq = new PopularResultReq()
            {
                Idx = offsetIndex,
                LoginEvent = isLogin ? 2 : 1,
                Qn = 112,
                Fnval = 464,
                Fourk = 1,
                Spmid = "creation.hot-tab.0.0",
                PlayerArgs = new Bilibili.App.Archive.Middleware.V1.PlayerArgs
                {
                    Qn = 112,
                    Fnval = 464,
                },
            };
            var request = await _httpProvider.GetRequestMessageAsync(Models.App.Constants.ApiConstants.Home.PopularGRPC, popularReq);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync(response, PopularReply.Parser);
            return data.Items.ToList();
        }
    }
}

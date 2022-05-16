// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.Data.Video;
using Bilibili.App.Card.V1;
using Bilibili.App.Show.V1;

namespace Bili.Lib.Uwp
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
        /// <param name="videoAdapter">视频数据适配工具.</param>
        public PopularProvider(
            IHttpProvider httpProvider,
            IAuthorizeProvider authorizeProvider,
            IVideoAdapter videoAdapter)
        {
            _httpProvider = httpProvider;
            _authorizeProvider = authorizeProvider;
            _videoAdapter = videoAdapter;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<VideoInformation>> RequestPopularVideosAsync()
        {
            var isLogin = _authorizeProvider.State == Models.Enums.AuthorizeState.SignedIn;
            var popularReq = new PopularResultReq()
            {
                Idx = _offsetId,
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
            var request = await _httpProvider.GetRequestMessageAsync(ApiConstants.Home.PopularGRPC, popularReq);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync(response, PopularReply.Parser);
            var result = data.Items
                .Where(p => p.ItemCase == Card.ItemOneofCase.SmallCoverV5)
                .Where(p => p.SmallCoverV5 != null)
                .Where(p => p.SmallCoverV5.Base.CardGoto == ServiceConstants.Av)
                .Select(p => _videoAdapter.ConvertToVideoInformation(p));
            _offsetId = data.Items.Where(p => p.SmallCoverV5 != null).Last().SmallCoverV5.Base.Idx;
            return result;
        }

        /// <inheritdoc/>
        public void Reset() => _offsetId = 0;
    }
}

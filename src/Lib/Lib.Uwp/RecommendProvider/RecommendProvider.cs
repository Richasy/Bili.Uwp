// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.Video;
using static Bili.Models.App.Constants.ApiConstants;
using static Bili.Models.App.Constants.ServiceConstants;

namespace Bili.Lib.Uwp
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
        /// <param name="videoAdapter">视频数据适配器.</param>
        /// <param name="pgcAdapter">PGC 内容适配器.</param>
        public RecommendProvider(
            IHttpProvider httpProvider,
            IVideoAdapter videoAdapter,
            IPgcAdapter pgcAdapter)
        {
            _httpProvider = httpProvider;
            _videoAdapter = videoAdapter;
            _pgcAdapter = pgcAdapter;
            _offsetId = 0;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IVideoBase>> RequestRecommendVideosAsync()
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Idx, _offsetId.ToString() },
                { Query.Flush, "5" },
                { Query.Column, "4" },
                { Query.Device, "pad" },
                { Query.DeviceName, "iPad 6" },
                { Query.Pull, (_offsetId == 0).ToString().ToLower() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(
                HttpMethod.Get,
                Home.Recommend,
                queryParameters,
                Models.Enums.RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse<HomeRecommendInfo>>(response);
            var offsetId = data.Data.Items.Last().Index;
            var items = data.Data.Items.Where(p => !string.IsNullOrEmpty(p.Goto)).ToList();

            // 目前只接受视频和剧集.
            var list = new List<IVideoBase>();
            foreach (var item in items)
            {
                if (item.CardGoto == Av)
                {
                    list.Add(_videoAdapter.ConvertToVideoInformation(item));
                }
                else if (item.CardGoto == Bangumi
                    || item.CardGoto == Models.App.Constants.ServiceConstants.Pgc)
                {
                    list.Add(_pgcAdapter.ConvertToEpisodeInformation(item));
                }
            }

            _offsetId = offsetId;
            return list;
        }

        /// <inheritdoc/>
        public void Reset()
            => _offsetId = 0;
    }
}

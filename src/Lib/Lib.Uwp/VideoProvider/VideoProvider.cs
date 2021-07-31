// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Bilibili.App.Playurl.V1;
using Bilibili.App.View.V1;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.BiliBili;
using static Richasy.Bili.Models.App.Constants.ServiceConstants;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 提供视频操作.
    /// </summary>
    public partial class VideoProvider : IVideoProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoProvider"/> class.
        /// </summary>
        /// <param name="httpProvider">网络操作工具.</param>
        public VideoProvider(IHttpProvider httpProvider)
        {
            _httpProvider = httpProvider;
        }

        /// <inheritdoc/>
        public async Task<ViewReply> GetVideoDetailAsync(long videoId)
        {
            var viewRequest = new ViewReq()
            {
                Aid = videoId,
            };

            var request = await _httpProvider.GetRequestMessageAsync(Api.Video.Detail, viewRequest);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync(response, ViewReply.Parser);
            return data;
        }

        /// <inheritdoc/>
        public async Task<string> GetOnlineViewerCountAsync(long videoId, long partId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Aid, videoId.ToString() },
                { Query.Cid, partId.ToString() },
                { Query.Device, "phone" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(System.Net.Http.HttpMethod.Get, Api.Video.OnlineViewerCount, queryParameters, Models.Enums.RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse<OnlineViewerResponse>>(response);
            return data.Data.Data.DisplayText;
        }

        /// <inheritdoc/>
        public async Task<PlayViewReply> GetPlayViewAsync(long videoId, long partId)
        {
            var playViewReq = new PlayViewReq
            {
                Aid = videoId,
                Cid = partId,
                Fnver = 0,
                Fnval = 16,
                Fourk = true,
                Download = 0,
                PreferCodecType = CodeType.Code264,
                Qn = 64,
            };

            var playConfReq = new PlayConfReq();

            var request = await _httpProvider.GetRequestMessageAsync(Api.Video.PlayInformation, playViewReq);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync(response, PlayViewReply.Parser);
            return data;
        }
    }
}

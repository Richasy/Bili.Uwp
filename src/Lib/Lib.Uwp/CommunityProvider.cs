// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bilibili.Main.Community.Reply.V1;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.Enums.Bili;

using static Richasy.Bili.Models.App.Constants.ApiConstants;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 社区交互数据处理.
    /// </summary>
    public partial class CommunityProvider : ICommunityProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommunityProvider"/> class.
        /// </summary>
        /// <param name="httpProvider">网络处理工具.</param>
        public CommunityProvider(IHttpProvider httpProvider)
        {
            _httpProvider = httpProvider;
        }

        /// <inheritdoc/>
        public async Task<DetailListReply> GetReplyDetailListAsync(int targetId, ReplyType type, long rootId, CursorReq cursor)
        {
            var req = new DetailListReq
            {
                Scene = DetailListScene.Reply,
                Cursor = cursor,
                Oid = targetId,
                Root = rootId,
                Type = (int)type,
            };

            var request = await _httpProvider.GetRequestMessageAsync(Community.ReplyDetailList, req);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync(response, DetailListReply.Parser);
            return result;
        }

        /// <inheritdoc/>
        public async Task<MainListReply> GetReplyMainListAsync(int targetId, ReplyType type, CursorReq cursor)
        {
            var req = new MainListReq
            {
                Cursor = cursor,
                Oid = targetId,
                Type = (int)type,
            };

            var request = await _httpProvider.GetRequestMessageAsync(Community.ReplyMainList, req);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync(response, MainListReply.Parser);
            return result;
        }
    }
}

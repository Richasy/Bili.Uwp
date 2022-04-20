// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bilibili.App.Dynamic.V2;
using Bilibili.Main.Community.Reply.V1;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums.Bili;

using static Richasy.Bili.Models.App.Constants.ApiConstants;
using static Richasy.Bili.Models.App.Constants.ServiceConstants;

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
        public async Task<DetailListReply> GetReplyDetailListAsync(long targetId, ReplyType type, long rootId, CursorReq cursor)
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
        public async Task<MainListReply> GetReplyMainListAsync(long targetId, ReplyType type, CursorReq cursor)
        {
            var req = new MainListReq
            {
                Cursor = cursor,
                Oid = targetId,
                Type = (int)type,
                Rpid = 0,
            };

            var request = await _httpProvider.GetRequestMessageAsync(Community.ReplyMainList, req);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync(response, MainListReply.Parser);
            return result;
        }

        /// <inheritdoc/>
        public async Task<bool> AddReplyAsync(string message, long targetId, ReplyType type, long rootId, long parentId)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.MessageFull, message },
                { Query.Oid, targetId.ToString() },
                { Query.Type, ((int)type).ToString() },
                { Query.PlatformSlim, "3" },
                { Query.Root, rootId.ToString() },
                { Query.Parent, parentId.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Community.AddReply, queryParameters, Models.Enums.RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<bool> LikeReplyAsync(bool isLike, long replyId, long targetId, ReplyType type)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Oid, targetId.ToString() },
                { Query.ActionFull, isLike ? "1" : "0" },
                { Query.ReplyId, replyId.ToString() },
                { Query.Type, ((int)type).ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, Community.LikeReply, queryParameters, Models.Enums.RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse>(response);
            return result.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<DynAllReply> GetDynamicComprehensiveListAsync(string historyOffset, string baseLine)
        {
            var type = string.IsNullOrEmpty(historyOffset) ? Refresh.New : Refresh.History;
            var req = new DynAllReq
            {
                RefreshType = type,
                LocalTime = 8,
                Offset = historyOffset ?? string.Empty,
                UpdateBaseline = baseLine ?? string.Empty,
            };

            var request = await _httpProvider.GetRequestMessageAsync(Community.DynamicAll, req, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync(response, DynAllReply.Parser);
            return result;
        }

        /// <inheritdoc/>
        public async Task<DynVideoReply> GetDynamicVideoListAsync(string historyOffset, string baseLine)
        {
            var type = string.IsNullOrEmpty(historyOffset) ? Refresh.New : Refresh.History;
            var req = new DynVideoReq
            {
                RefreshType = type,
                LocalTime = 8,
                Offset = historyOffset ?? string.Empty,
                UpdateBaseline = baseLine ?? string.Empty,
            };

            var request = await _httpProvider.GetRequestMessageAsync(Community.DynamicVideo, req, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync(response, DynVideoReply.Parser);
            return result;
        }

        /// <inheritdoc/>
        public async Task<bool> LikeDynamicAsync(string dynamicId, bool isLike, long userId, string rid)
        {
            var req = new DynThumbReq
            {
                Type = isLike ? ThumbType.Thumb : ThumbType.Cancel,
                DynId = dynamicId.ToString(),
                Rid = rid,
                Uid = Convert.ToInt64(userId),
            };

            try
            {
                var request = await _httpProvider.GetRequestMessageAsync(Community.LikeDynamic, req, true);
                _ = await _httpProvider.SendAsync(request);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

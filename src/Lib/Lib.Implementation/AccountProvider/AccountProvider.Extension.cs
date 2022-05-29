// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.Community;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
using static Bili.Models.App.Constants.ApiConstants;
using static Bili.Models.App.Constants.ServiceConstants;

namespace Bili.Lib.Uwp
{
    /// <summary>
    /// 提供已登录用户相关的数据操作.
    /// </summary>
    public partial class AccountProvider
    {
        private readonly IHttpProvider _httpProvider;
        private readonly IUserAdapter _userAdapter;
        private readonly ICommunityAdapter _communityAdapter;
        private readonly Dictionary<MessageType, MessageCursor> _messageOffsetCache;

        private async Task<PgcFavoriteListResponse> GetPgcFavoriteListInternalAsync(string requestUrl, int pageNumber, int status)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.PageNumber, pageNumber.ToString() },
                { Query.PageSizeSlim, "20" },
                { Query.Status, status.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, requestUrl, queryParameters, RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse2<PgcFavoriteListResponse>>(response);
            return result.Result;
        }

        private async Task<MessageView> GetMessageInternalAsync(MessageType type, long id, long time)
        {
            var timeName = string.Empty;
            var url = string.Empty;

            switch (type)
            {
                case MessageType.Like:
                    timeName = Query.LikeTime;
                    url = Account.MessageLike;
                    break;
                case MessageType.At:
                    timeName = Query.AtTime;
                    url = Account.MessageAt;
                    break;
                case MessageType.Reply:
                    timeName = Query.ReplyTime;
                    url = Account.MessageReply;
                    break;
                default:
                    break;
            }

            var queryParameters = new Dictionary<string, string>
            {
                { Query.Id, id.ToString() },
                { timeName, time.ToString() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, url, queryParameters, RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);

            MessageView data = null;
            MessageCursor cursor = null;
            if (type == MessageType.Reply)
            {
                var parsed = await _httpProvider.ParseAsync<ServerResponse<ReplyMessageResponse>>(response);
                cursor = parsed.Data.Cursor;
                data = _communityAdapter.ConvertToMessageView(parsed.Data);
            }
            else if (type == MessageType.Like)
            {
                var parsed = await _httpProvider.ParseAsync<ServerResponse<LikeMessageResponse>>(response);
                cursor = parsed.Data.Total.Cursor;
                data = _communityAdapter.ConvertToMessageView(parsed.Data);
            }
            else if (type == MessageType.At)
            {
                var parsed = await _httpProvider.ParseAsync<ServerResponse<AtMessageResponse>>(response);
                cursor = parsed.Data.Cursor;
                data = _communityAdapter.ConvertToMessageView(parsed.Data);
            }

            _messageOffsetCache.Remove(type);
            _messageOffsetCache.Add(type, cursor);
            return data;
        }
    }
}

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
using Bili.Models.Enums.Bili;
using Bilibili.App.Interfaces.V1;
using static Bili.Models.App.Constants.ApiConstants;
using static Bili.Models.App.Constants.ServiceConstants;

namespace Bili.Lib
{
    /// <summary>
    /// 提供已登录用户相关的数据操作.
    /// </summary>
    public partial class AccountProvider
    {
        private readonly IHttpProvider _httpProvider;
        private readonly IUserAdapter _userAdapter;
        private readonly ICommunityAdapter _communityAdapter;
        private readonly IVideoAdapter _videoAdapter;
        private readonly Dictionary<MessageType, MessageCursor> _messageOffsetCache;
        private readonly Dictionary<RelationType, int> _relationOffsetCache;
        private readonly Dictionary<string, int> _myFollowOffsetCache;
        private int _viewLaterPageNumber;
        private string _spaceVideoOffset;
        private int _spaceSearchPageNumber;
        private Cursor _historyCursor;

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

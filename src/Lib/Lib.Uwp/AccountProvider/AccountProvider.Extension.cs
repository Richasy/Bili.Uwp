// Copyright (c) GodLeaveMe. All rights reserved.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Models.Enums.App;
using static Richasy.Bili.Models.App.Constants.ApiConstants;
using static Richasy.Bili.Models.App.Constants.ServiceConstants;

namespace Richasy.Bili.Lib.Uwp
{
    /// <summary>
    /// 提供已登录用户相关的数据操作.
    /// </summary>
    public partial class AccountProvider
    {
        private readonly IHttpProvider _httpProvider;

        private async Task<PgcFavoriteListResponse> GetPgcFavoriteListInternalAsync(string requestUrl, int pageNumber)
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.PageNumber, pageNumber.ToString() },
                { Query.PageSizeSlim, "20" },
                { Query.Status, "2" },
            };

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, requestUrl, queryParameters, RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var result = await _httpProvider.ParseAsync<ServerResponse2<PgcFavoriteListResponse>>(response);
            return result.Result;
        }

        private async Task<T> GetMessageInternalAsync<T>(MessageType type, long id, long time)
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
            var result = await _httpProvider.ParseAsync<ServerResponse<T>>(response);
            return result.Data;
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.Pgc;
using Bili.Models.Enums;
using static Bili.Models.App.Constants.ServiceConstants;

namespace Bili.Lib
{
    /// <summary>
    /// 收藏夹相关服务提供工具.
    /// </summary>
    public sealed partial class FavoriteProvider
    {
        private readonly IHttpProvider _httpProvider;
        private readonly IArticleAdapter _articleAdapter;
        private readonly IPgcAdapter _pgcAdapter;
        private readonly IFavoriteAdapter _favoriteAdapter;

        private int _videoFolderDetailPageNumber;
        private int _videoCollectFolderPageNumber;
        private int _videoCreatedFolderPageNumber;
        private int _animeFolderPageNumber;
        private int _cinemaFolderPageNumber;
        private int _articleFolderPageNumber;

        private async Task<SeasonSet> GetPgcFavoriteListInternalAsync(string requestUrl, int pageNumber, int status)
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
            return _pgcAdapter.ConvertToSeasonSet(result.Result);
        }
    }
}

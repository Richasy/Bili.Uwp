// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.Pgc;
using Bili.Models.Enums;
using static Bili.Models.App.Constants.ApiConstants;

namespace Bili.Lib.Uwp
{
    /// <summary>
    /// 获取专业内容创作数据的工具.
    /// </summary>
    public partial class PgcProvider : IPgcProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcProvider"/> class.
        /// </summary>
        /// <param name="httpProvider">网络操作工具.</param>
        /// <param name="partitionProvider">分区操作工具.</param>
        /// <param name="communityAdapter">社区数据适配工具.</param>
        /// <param name="pgcAdapter">PGC数据适配工具.</param>
        public PgcProvider(
            IHttpProvider httpProvider,
            IHomeProvider partitionProvider,
            ICommunityAdapter communityAdapter,
            IPgcAdapter pgcAdapter)
        {
            _httpProvider = httpProvider;
            _partitionProvider = partitionProvider;
            _communityAdapter = communityAdapter;
            _pgcAdapter = pgcAdapter;

            _pgcOffsetCache = new Dictionary<PgcType, string>();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Models.Data.Community.Partition>> GetAnimeTabsAsync(PgcType type)
        {
            var queryParameters = GetTabQueryParameters(type);
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Pgc.Tab, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse<List<PgcTab>>>(response);

            var items = data.Data.Where(p => p.Link.Contains("sub_page_id"))
                .Select(p => _communityAdapter.ConvertToPartition(p))
                .ToList();
            return items;
        }

        /// <inheritdoc/>
        public async Task<PgcPageView> GetPageDetailAsync(string tabId)
        {
            var queryParameters = GetPageDetailQueryParameters(tabId);
            var response = await GetPgcResponseInternalAsync(queryParameters);
            return _pgcAdapter.ConvertToPgcPageView(response);
        }

        /// <inheritdoc/>
        public async Task<PgcPageView> GetPageDetailAsync(PgcType type)
        {
            var cursor = _pgcOffsetCache.ContainsKey(type)
                ? _pgcOffsetCache[type]
                : string.Empty;
            var queryParameters = GetPageDetailQueryParameters(type, cursor);
            var response = await GetPgcResponseInternalAsync(queryParameters);
            _pgcOffsetCache.Remove(type);
            _pgcOffsetCache.Add(type, response.NextCursor);
            return _pgcAdapter.ConvertToPgcPageView(response);
        }

        /// <inheritdoc/>
        public async Task<SubPartition> GetPartitionRecommendVideoAsync(int partitionId, int offsetId = 0)
        {
            var data = await _partitionProvider.GetVideoSubPartitionDataAsync(partitionId.ToString(), false);
            return null;
        }

        /// <inheritdoc/>
        public async Task<PgcDisplayInformation> GetDisplayInformationAsync(int episodeId = 0, int seasonId = 0, string proxy = "", string area = "")
        {
            var queryParameters = GetPgcDetailInformationQueryParameters(episodeId, seasonId, area);
            var otherQuery = string.Empty;

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Pgc.SeasonDetail(proxy), queryParameters, RequestClientType.IOS, additionalQuery: otherQuery);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse<PgcDisplayInformation>>(response);
            return data.Data;
        }

        /// <inheritdoc/>
        public async Task<EpisodeInteraction> GetEpisodeInteractionAsync(int episodeId)
        {
            var queryParameters = GetEpisodeInteractionQueryParameters(episodeId);
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Pgc.EpisodeInteraction, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse<EpisodeInteraction>>(response);
            return data.Data;
        }

        /// <inheritdoc/>
        public async Task<bool> FollowAsync(int seasonId, bool isFollow)
        {
            var queryParameters = GetFollowQueryParameters(seasonId);
            var url = isFollow ? Pgc.Follow : Pgc.Unfollow;
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, url, queryParameters, RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse>(response);
            return data.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<PgcIndexConditionResponse> GetPgcIndexConditionsAsync(PgcType type)
        {
            var queryParameters = GetPgcIndexBaseQueryParameters(type);
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Pgc.IndexCondition, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse<PgcIndexConditionResponse>>(response);
            return data.Data;
        }

        /// <inheritdoc/>
        public async Task<PgcIndexResultResponse> GetPgcIndexResultAsync(PgcType type, int page, Dictionary<string, string> parameters)
        {
            var queryParameters = GetPgcIndexResultQueryParameters(type, page, parameters);
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Pgc.IndexResult, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse<PgcIndexResultResponse>>(response);
            return data.Data;
        }

        /// <inheritdoc/>
        public async Task<PgcTimeLineResponse> GetPgcTimeLineAsync(PgcType type)
        {
            var queryParameters = GetPgcTimeLineQueryParameters(type);
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Pgc.TimeLine, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse2<PgcTimeLineResponse>>(response);
            return data.Result;
        }

        /// <inheritdoc/>
        public async Task<PgcPlaylist> GetPgcPlaylistAsync(string listId)
        {
            var queryParameters = GetPgcPlayListQueryParameters(listId);
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Pgc.PlayList, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse2<PgcPlayListResponse>>(response);
            return _pgcAdapter.ConvertToPgcPlaylist(data.Result);
        }

        /// <inheritdoc/>
        public void ResetPageStatus(PgcType type)
            => _pgcOffsetCache.Remove(type);
    }
}

﻿// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using static Richasy.Bili.Models.App.Constants.ApiConstants;

namespace Richasy.Bili.Lib.Uwp
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
        public PgcProvider(IHttpProvider httpProvider, IPartitionProvider partitionProvider)
        {
            _httpProvider = httpProvider;
            _partitionProvider = partitionProvider;
        }

        /// <inheritdoc/>
        public async Task<PgcResponse> GetPageDetailAsync(int tabId)
        {
            var queryParameters = GetPageDetailQueryParameters(tabId);
            return await GetPgcResponseInternalAsync(queryParameters);
        }

        /// <inheritdoc/>
        public async Task<PgcResponse> GetPageDetailAsync(PgcType type, string cursor)
        {
            var queryParameters = GetPageDetailQueryParameters(type, cursor);
            return await GetPgcResponseInternalAsync(queryParameters);
        }

        /// <inheritdoc/>
        public async Task<List<PgcTab>> GetTabAsync(PgcType type)
        {
            var queryParameters = GetTabQueryParameters(type);
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Pgc.Tab, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse<List<PgcTab>>>(response);
            return data.Data;
        }

        /// <inheritdoc/>
        public async Task<SubPartition> GetPartitionRecommendVideoAsync(int partitionId, int offsetId = 0)
        {
            var data = await _partitionProvider.GetSubPartitionDataAsync(partitionId, false, offsetId);
            return data;
        }

        /// <inheritdoc/>
        public async Task<PgcDisplayInformation> GetDisplayInformationAsync(int episodeId = 0, int seasonId = 0)
        {
            var queryParameters = GetPgcDetailInformationQueryParameters(episodeId, seasonId);
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Pgc.SeasonDetail, queryParameters, RequestClientType.IOS);
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
        public async Task<PgcPlayListResponse> GetPgcPlayListAsync(int listId)
        {
            var queryParameters = GetPgcPlayListQueryParameters(listId);
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Pgc.PlayList, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse2<PgcPlayListResponse>>(response);
            return data.Result;
        }
    }
}

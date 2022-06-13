// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.App;
using Bili.Models.BiliBili;
using Bili.Models.Data.Appearance;
using Bili.Models.Data.Pgc;
using Bili.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static Bili.Models.App.Constants.ApiConstants;

namespace Bili.Lib
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
        public async Task<IEnumerable<Filter>> GetPgcIndexFiltersAsync(PgcType type)
        {
            var queryParameters = GetPgcIndexBaseQueryParameters(type);
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Pgc.IndexCondition, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse<PgcIndexConditionResponse>>(response);
            return _pgcAdapter.ConvertToFilters(data.Data);
        }

        /// <inheritdoc/>
        public async Task<(bool IsFinished, IEnumerable<SeasonInformation> Items)> GetPgcIndexResultAsync(PgcType type, Dictionary<string, string> parameters)
        {
            var queryParameters = GetPgcIndexResultQueryParameters(type, _indexPageNumber, parameters);
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Pgc.IndexResult, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse<PgcIndexResultResponse>>(response);
            var isFinish = data.Data.HasNext == 0;
            var items = data.Data.List.Select(p => _pgcAdapter.ConvertToSeasonInformation(p)).ToList();
            if (!isFinish)
            {
                _indexPageNumber++;
            }

            return (isFinish, items);
        }

        /// <inheritdoc/>
        public async Task<bool> FollowAsync(string seasonId, bool isFollow)
        {
            var queryParameters = GetFollowQueryParameters(seasonId);
            var url = isFollow ? Pgc.Follow : Pgc.Unfollow;
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Post, url, queryParameters, RequestClientType.IOS, true);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse>(response);
            return data.IsSuccess();
        }

        /// <inheritdoc/>
        public async Task<TimelineView> GetPgcTimelinesAsync(PgcType type)
        {
            var queryParameters = GetPgcTimeLineQueryParameters(type);
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Pgc.TimeLine, queryParameters, RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse2<PgcTimeLineResponse>>(response);
            return _pgcAdapter.ConvertToTimelineView(data.Result);
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
        public async Task<BiliPlusBangumi> GetBiliPlusBangumiInformationAsync(string videoId)
        {
            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
            };
            using var client = new HttpClient(handler);
            var url = $"https://www.biliplus.com/api/view?id={videoId}";
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var bytes = await response.Content.ReadAsByteArrayAsync();
            var str = Encoding.UTF8.GetString(bytes);
            var jObj = JObject.Parse(str);
            var bangumi = jObj["bangumi"].ToString();
            return JsonConvert.DeserializeObject<BiliPlusBangumi>(bangumi);
        }

        /// <inheritdoc/>
        public void ResetPageStatus(PgcType type)
            => _pgcOffsetCache.Remove(type);

        /// <inheritdoc/>
        public void ResetIndexStatus()
            => _indexPageNumber = 1;
    }
}

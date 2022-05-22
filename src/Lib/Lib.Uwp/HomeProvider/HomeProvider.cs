// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.App.Other;
using Bili.Models.BiliBili;
using Bili.Models.Data.Community;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bilibili.App.Show.V1;
using static Bili.Models.App.Constants.AppConstants;
using static Bili.Models.App.Constants.ServiceConstants;

namespace Bili.Lib.Uwp
{
    /// <summary>
    /// 提供分区及标签的数据操作.
    /// </summary>
    public partial class HomeProvider : IHomeProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeProvider"/> class.
        /// </summary>
        /// <param name="httpProvider">网络操作工具.</param>
        /// <param name="authorizeProvider">授权工具.</param>
        /// <param name="communityAdapter">社区数据适配器.</param>
        /// <param name="videoAdapter">视频数据适配器.</param>
        /// <param name="pgcAdapter">PGC数据适配器.</param>
        /// <param name="fileToolkit">文件管理工具.</param>
        public HomeProvider(
            IHttpProvider httpProvider,
            IAuthorizeProvider authorizeProvider,
            ICommunityAdapter communityAdapter,
            IVideoAdapter videoAdapter,
            IPgcAdapter pgcAdapter,
            IFileToolkit fileToolkit)
        {
            _httpProvider = httpProvider;
            _authorizeProvider = authorizeProvider;
            _communityAdapter = communityAdapter;
            _videoAdapter = videoAdapter;
            _pgcAdapter = pgcAdapter;
            _fileToolkit = fileToolkit;

            _popularOffsetId = 0;
            _recommendOffsetId = 0;
            _cacheVideoPartitionOffsets = new Dictionary<string, (int OffsetId, int PageNumber)>();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<IVideoBase>> RequestRecommendVideosAsync()
        {
            var queryParameters = new Dictionary<string, string>
            {
                { Query.Idx, _recommendOffsetId.ToString() },
                { Query.Flush, "5" },
                { Query.Column, "4" },
                { Query.Device, "pad" },
                { Query.DeviceName, "iPad 6" },
                { Query.Pull, (_recommendOffsetId == 0).ToString().ToLower() },
            };

            var request = await _httpProvider.GetRequestMessageAsync(
                HttpMethod.Get,
                ApiConstants.Home.Recommend,
                queryParameters,
                RequestClientType.IOS);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse<HomeRecommendInfo>>(response);
            var offsetId = data.Data.Items.Last().Index;
            var items = data.Data.Items.Where(p => !string.IsNullOrEmpty(p.Goto)).ToList();

            // 目前只接受视频和剧集.
            var list = new List<IVideoBase>();
            foreach (var item in items)
            {
                if (item.CardGoto == Av)
                {
                    list.Add(_videoAdapter.ConvertToVideoInformation(item));
                }
                else if (item.CardGoto == Bangumi
                    || item.CardGoto == Models.App.Constants.ServiceConstants.Pgc)
                {
                    list.Add(_pgcAdapter.ConvertToEpisodeInformation(item));
                }
            }

            _recommendOffsetId = offsetId;
            return list;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<VideoInformation>> RequestPopularVideosAsync()
        {
            var isLogin = _authorizeProvider.State == Models.Enums.AuthorizeState.SignedIn;
            var popularReq = new PopularResultReq()
            {
                Idx = _popularOffsetId,
                LoginEvent = isLogin ? 2 : 1,
                Qn = 112,
                Fnval = 464,
                Fourk = 1,
                Spmid = "creation.hot-tab.0.0",
                PlayerArgs = new Bilibili.App.Archive.Middleware.V1.PlayerArgs
                {
                    Qn = 112,
                    Fnval = 464,
                },
            };
            var request = await _httpProvider.GetRequestMessageAsync(ApiConstants.Home.PopularGRPC, popularReq);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync(response, PopularReply.Parser);
            var result = data.Items
                .Where(p => p.ItemCase == Bilibili.App.Card.V1.Card.ItemOneofCase.SmallCoverV5)
                .Where(p => p.SmallCoverV5 != null)
                .Where(p => p.SmallCoverV5.Base.CardGoto == Av)
                .Select(p => _videoAdapter.ConvertToVideoInformation(p));
            _popularOffsetId = data.Items.Where(p => p.SmallCoverV5 != null).Last().SmallCoverV5.Base.Idx;
            return result;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Models.Data.Community.Partition>> GetVideoPartitionIndexAsync()
        {
            var localCache = await GetPartitionCacheAsync();

            if (localCache != null)
            {
                return localCache;
            }

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, ApiConstants.Partition.PartitionIndex);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<Models.BiliBili.ServerResponse<List<Models.BiliBili.Partition>>>(response);

            var items = data.Data.Where(p => !string.IsNullOrEmpty(p.Uri) &&
                                        p.Uri.StartsWith("bilibili") &&
                                        p.Uri.Contains("region/") &&
                                        p.Children != null &&
                                        p.Children.Count > 0);
            var result = items.Select(p => _communityAdapter.ConvertToPartition(p));
            await CachePartitionsAsync(result);
            return result;
        }

        /// <inheritdoc/>
        public async Task<VideoPartitionView> GetVideoSubPartitionDataAsync(
            string subPartitionId,
            bool isRecommend,
            VideoSortType sortType = VideoSortType.Default)
        {
            if (_cacheVideoPartitionOffsets.ContainsKey(subPartitionId))
            {
                var (oid, pn) = _cacheVideoPartitionOffsets[subPartitionId];
                videoPartitionOffsetId = oid;
                videoPartitionPageNumber = pn;
            }

            var isOffset = videoPartitionOffsetId > 0;
            var isDefaultOrder = sortType == VideoSortType.Default;
            Models.BiliBili.SubPartition data;

            var requestUrl = isRecommend
                ? isOffset ? ApiConstants.Partition.SubPartitionRecommendOffset : ApiConstants.Partition.SubPartitionRecommend
                : !isDefaultOrder
                    ? ApiConstants.Partition.SubPartitionOrderOffset
                    : isOffset ? ApiConstants.Partition.SubPartitionNormalOffset : ApiConstants.Partition.SubPartitionNormal;

            var queryParameters = new Dictionary<string, string>
            {
                { Query.PartitionId, subPartitionId },
                { Query.Pull, "0" },
            };

            if (isOffset)
            {
                queryParameters.Add(Query.CreateTime, videoPartitionOffsetId.ToString());
            }

            if (!isDefaultOrder)
            {
                var sortStr = string.Empty;
                switch (sortType)
                {
                    case VideoSortType.Newest:
                        sortStr = Sort.Newest;
                        break;
                    case VideoSortType.Play:
                        sortStr = Sort.Play;
                        break;
                    case VideoSortType.Reply:
                        sortStr = Sort.Reply;
                        break;
                    case VideoSortType.Danmaku:
                        sortStr = Sort.Danmaku;
                        break;
                    case VideoSortType.Favorite:
                        sortStr = Sort.Favorite;
                        break;
                    default:
                        break;
                }

                queryParameters.Add(Query.Order, sortStr);
                queryParameters.Add(Query.PageNumber, videoPartitionPageNumber.ToString());
                queryParameters.Add(Query.PageSizeSlim, "30");
            }

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, requestUrl, queryParameters);
            var response = await _httpProvider.SendAsync(request);
            if (isOffset)
            {
                data = (await _httpProvider.ParseAsync<ServerResponse<SubPartition>>(response)).Data;
            }
            else if (!isRecommend)
            {
                if (!isDefaultOrder)
                {
                    var list = (await _httpProvider.ParseAsync<ServerResponse<List<PartitionVideo>>>(response)).Data;
                    data = new SubPartition()
                    {
                        NewVideos = list,
                    };
                }
                else
                {
                    data = (await _httpProvider.ParseAsync<ServerResponse<SubPartitionDefault>>(response)).Data;
                }
            }
            else
            {
                data = (await _httpProvider.ParseAsync<ServerResponse<Models.BiliBili.SubPartitionRecommend>>(response)).Data;
            }

            var id = subPartitionId;
            var videos = data.NewVideos
                .Concat(data.RecommendVideos ?? new List<PartitionVideo>())
                .Select(p => _videoAdapter.ConvertToVideoInformation(p));
            IEnumerable<BannerIdentifier> banners = null;
            if (data is Models.BiliBili.SubPartitionRecommend recommendView
                && recommendView.Banner != null)
            {
                banners = recommendView.Banner.TopBanners.Select(p => _communityAdapter.ConvertToBannerIdentifier(p));
            }

            videoPartitionOffsetId = data.BottomOffsetId;
            videoPartitionPageNumber = !isRecommend && sortType != VideoSortType.Default ? videoPartitionPageNumber + 1 : 1;
            currentPartitionId = subPartitionId;

            UpdateVideoPartitionCache();

            return new VideoPartitionView(id, videos, banners);
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<VideoInformation>> GetRankDetailAsync(string partitionId)
        {
            var rankRequst = new RankRegionResultReq() { Rid = System.Convert.ToInt32(partitionId) };
            var request = await _httpProvider.GetRequestMessageAsync(ApiConstants.Home.RankingGRPC, rankRequst);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync(response, RankListReply.Parser);
            return data.Items.ToList().Select(p => _videoAdapter.ConvertToVideoInformation(p));
        }

        /// <inheritdoc/>
        public void ResetSubPartitionState()
        {
            videoPartitionOffsetId = 0;
            videoPartitionPageNumber = 1;
            UpdateVideoPartitionCache();
        }

        /// <inheritdoc/>
        public void ClearPartitionState()
        {
            ResetSubPartitionState();
            _cacheVideoPartitionOffsets.Clear();
        }

        /// <inheritdoc/>
        public void ResetRecommendState()
            => _recommendOffsetId = 0;

        /// <inheritdoc/>
        public void ResetPopularState()
            => _popularOffsetId = 0;

        private async Task<IEnumerable<Models.Data.Community.Partition>> GetPartitionCacheAsync()
        {
            var cacheData = await _fileToolkit.ReadLocalDataAsync<LocalCache<List<Models.Data.Community.Partition>>>(
                Location.PartitionCache,
                folderName: Location.ServerFolder);

            if (cacheData == null || cacheData.ExpiryTime < System.DateTimeOffset.Now)
            {
                return null;
            }

            return cacheData.Data;
        }

        private async Task CachePartitionsAsync(IEnumerable<Models.Data.Community.Partition> data)
        {
            var localCache = new LocalCache<List<Models.Data.Community.Partition>>(System.DateTimeOffset.Now.AddDays(1), data.ToList());
            await _fileToolkit.WriteLocalDataAsync(Location.PartitionCache, localCache, Location.ServerFolder);
        }

        private void UpdateVideoPartitionCache()
        {
            _cacheVideoPartitionOffsets.Remove(currentPartitionId);
            _cacheVideoPartitionOffsets.Add(currentPartitionId, (videoPartitionOffsetId, videoPartitionPageNumber));
        }
    }
}

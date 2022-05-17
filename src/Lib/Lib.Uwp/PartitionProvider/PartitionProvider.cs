// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.App.Other;
using Bili.Models.Data.Community;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using static Bili.Models.App.Constants.AppConstants;
using static Bili.Models.App.Constants.ServiceConstants;

namespace Bili.Lib.Uwp
{
    /// <summary>
    /// 提供分区及标签的数据操作.
    /// </summary>
    public partial class PartitionProvider : IPartitionProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionProvider"/> class.
        /// </summary>
        /// <param name="httpProvider">网络操作工具.</param>
        /// <param name="communityAdapter">社区数据适配器.</param>
        /// <param name="videoAdapter">视频数据适配器.</param>
        /// <param name="fileToolkit">文件管理工具.</param>
        public PartitionProvider(
            IHttpProvider httpProvider,
            ICommunityAdapter communityAdapter,
            IVideoAdapter videoAdapter,
            IFileToolkit fileToolkit)
        {
            _httpProvider = httpProvider;
            _communityAdapter = communityAdapter;
            _videoAdapter = videoAdapter;
            _fileToolkit = fileToolkit;
            _cacheOffsets = new Dictionary<string, (int OffsetId, int PageNumber)>();
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Partition>> GetPartitionIndexAsync()
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
        public async Task<PartitionView> GetSubPartitionDataAsync(
            string subPartitionId,
            bool isRecommend,
            VideoSortType sortType = VideoSortType.Default)
        {
            if (_cacheOffsets.ContainsKey(subPartitionId))
            {
                var (oid, pn) = _cacheOffsets[subPartitionId];
                offsetId = oid;
                pageNumber = pn;
            }

            var isOffset = offsetId > 0;
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
                queryParameters.Add(Query.CreateTime, offsetId.ToString());
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
                queryParameters.Add(Query.PageNumber, pageNumber.ToString());
                queryParameters.Add(Query.PageSizeSlim, "30");
            }

            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, requestUrl, queryParameters);
            var response = await _httpProvider.SendAsync(request);
            if (isOffset)
            {
                data = (await _httpProvider.ParseAsync<Models.BiliBili.ServerResponse<Models.BiliBili.SubPartition>>(response)).Data;
            }
            else if (!isRecommend)
            {
                if (!isDefaultOrder)
                {
                    var list = (await _httpProvider.ParseAsync<Models.BiliBili.ServerResponse<List<Models.BiliBili.PartitionVideo>>>(response)).Data;
                    data = new Models.BiliBili.SubPartition()
                    {
                        NewVideos = list,
                    };
                }
                else
                {
                    data = (await _httpProvider.ParseAsync<Models.BiliBili.ServerResponse<Models.BiliBili.SubPartitionDefault>>(response)).Data;
                }
            }
            else
            {
                data = (await _httpProvider.ParseAsync<Models.BiliBili.ServerResponse<Models.BiliBili.SubPartitionRecommend>>(response)).Data;
            }

            var id = subPartitionId;
            var videos = data.NewVideos
                .Concat(data.RecommendVideos ?? new List<Models.BiliBili.PartitionVideo>())
                .Select(p => _videoAdapter.ConvertToVideoInformation(p));
            IEnumerable<BannerIdentifier> banners = null;
            if (data is Models.BiliBili.SubPartitionRecommend recommendView
                && recommendView.Banner != null)
            {
                banners = recommendView.Banner.TopBanners.Select(p => _communityAdapter.ConvertToBannerIdentifier(p));
            }

            offsetId = data.BottomOffsetId;
            pageNumber = !isRecommend && sortType != VideoSortType.Default ? pageNumber + 1 : 1;
            currentPartitionId = subPartitionId;

            UpdateCache();

            return new PartitionView(id, videos, banners);
        }

        /// <inheritdoc/>
        public void Reset()
        {
            offsetId = 0;
            pageNumber = 1;
            UpdateCache();
        }

        /// <inheritdoc/>
        public void Clear()
        {
            Reset();
            _cacheOffsets.Clear();
        }

        private async Task<IEnumerable<Partition>> GetPartitionCacheAsync()
        {
            var cacheData = await _fileToolkit.ReadLocalDataAsync<LocalCache<List<Partition>>>(
                Location.PartitionCache,
                folderName: Location.ServerFolder);

            if (cacheData == null || cacheData.ExpiryTime < System.DateTimeOffset.Now)
            {
                return null;
            }

            return cacheData.Data;
        }

        private async Task CachePartitionsAsync(IEnumerable<Partition> data)
        {
            var localCache = new LocalCache<List<Partition>>(System.DateTimeOffset.Now.AddDays(1), data.ToList());
            await _fileToolkit.WriteLocalDataAsync(Location.PartitionCache, localCache, Location.ServerFolder);
        }

        private void UpdateCache()
        {
            _cacheOffsets.Remove(currentPartitionId);
            _cacheOffsets.Add(currentPartitionId, (offsetId, pageNumber));
        }
    }
}

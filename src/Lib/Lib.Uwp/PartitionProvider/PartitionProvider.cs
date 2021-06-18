// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using static Richasy.Bili.Models.App.Constants.ServiceConstants;

namespace Richasy.Bili.Lib.Uwp
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
        public PartitionProvider(IHttpProvider httpProvider) => _httpProvider = httpProvider;

        /// <inheritdoc/>
        public async Task<IEnumerable<Partition>> GetPartitionIndexAsync()
        {
            var request = await _httpProvider.GetRequestMessageAsync(HttpMethod.Get, Api.Partition.PartitionIndex);
            var response = await _httpProvider.SendAsync(request);
            var data = await _httpProvider.ParseAsync<ServerResponse<List<Partition>>>(response);

            return data.Data.Where(p => p.IsNeedToShow());
        }

        /// <inheritdoc/>
        public async Task<SubPartition> GetSubPartitionDataAsync(
            int subPartitionId,
            bool isRecommend,
            int offsetId = 0,
            VideoSortType sortType = VideoSortType.Default,
            int pageNum = 1)
        {
            var requestUrl = string.Empty;
            var isOffset = offsetId != 0;
            var isDefaultOrder = sortType == VideoSortType.Default;
            SubPartition data;

            if (isRecommend)
            {
                requestUrl = isOffset ? Api.Partition.SubPartitionRecommendOffset : Api.Partition.SubPartitionRecommend;
            }
            else
            {
                if (!isDefaultOrder)
                {
                    requestUrl = Api.Partition.SubPartitionOrderOffset;
                }
                else
                {
                    requestUrl = isOffset ? Api.Partition.SubPartitionNormalOffset : Api.Partition.SubPartitionNormal;
                }
            }

            var queryParameters = new Dictionary<string, string>
            {
                { Query.PartitionId, subPartitionId.ToString() },
                { Query.Pull, "0" },
            };

            if (isOffset)
            {
                queryParameters.Add(Query.OffsetId, offsetId.ToString());
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
                queryParameters.Add(Query.PageNumber, pageNum.ToString());
                queryParameters.Add(Query.PageSize, "30");
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
                data = (await _httpProvider.ParseAsync<ServerResponse<SubPartitionRecommend>>(response)).Data;
            }

            return data;
        }
    }
}

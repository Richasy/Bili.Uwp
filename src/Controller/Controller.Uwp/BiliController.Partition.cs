// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using static Richasy.Bili.Models.App.Constants.ControllerConstants;

namespace Richasy.Bili.Controller.Uwp
{
    /// <summary>
    /// 控制器的分区及标签部分.
    /// </summary>
    public partial class BiliController
    {
        /// <summary>
        /// 请求分区索引.
        /// </summary>
        /// <returns>分区索引列表.</returns>
        public async Task<List<Partition>> RequestPartitionIndexAsync()
        {
            var cacheData = await _fileToolkit.ReadLocalDataAsync<LocalCache<List<Partition>>>(Names.PartitionIndex, folderName: Names.ServerFolder);
            var needRequest = true;
            List<Partition> data = null;
            if (cacheData != null)
            {
                needRequest = cacheData.ExpiryTime < DateTimeOffset.Now;
            }

            if (needRequest)
            {
                ThrowWhenNetworkUnavaliable();

                try
                {
                    var webResult = await _partitionProvider.GetPartitionIndexAsync();
                    data = webResult.ToList();
                    var localCache = new LocalCache<List<Partition>>(DateTimeOffset.Now.AddDays(1), data);
                    await _fileToolkit.WriteLocalDataAsync(Names.PartitionIndex, localCache, Names.ServerFolder);
                }
                catch (Exception ex)
                {
                    _loggerModule.LogError(ex);
                }
            }
            else
            {
                data = cacheData.Data;
            }

            foreach (var partion in data)
            {
                partion.Children.Insert(0, null);
            }

            return data;
        }

        /// <summary>
        /// 请求子分区数据.
        /// </summary>
        /// <param name="subPartitionId">子分区Id.</param>
        /// <param name="isRecommend">是否为推荐视频分区.</param>
        /// <param name="offsetId">偏移值.</param>
        /// <param name="sortType">排序方式.</param>
        /// <param name="pageNumber">页码.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestSubPartitionDataAsync(int subPartitionId, bool isRecommend, int offsetId = 0, VideoSortType sortType = VideoSortType.Default, int pageNumber = 1)
        {
            ThrowWhenNetworkUnavaliable();
            try
            {
                var requestDateTime = DateTimeOffset.Now;
                var data = await _partitionProvider.GetSubPartitionDataAsync(subPartitionId, isRecommend, offsetId, sortType, pageNumber);
                pageNumber = !isRecommend && sortType != VideoSortType.Default ? pageNumber + 1 : 1;
                SubPartitionVideoIteration?.Invoke(this, new PartitionVideoIterationEventArgs(subPartitionId, requestDateTime, data, pageNumber));

                if (data is SubPartitionRecommend recommend && (recommend.Banner?.TopBanners?.Any() ?? false))
                {
                    SubPartitionAdditionalDataChanged?.Invoke(
                        this,
                        new PartitionAdditionalDataChangedEventArgs(
                            subPartitionId,
                            requestDateTime,
                            recommend.Banner.TopBanners));
                }
                else if (data is SubPartitionDefault defaultData && (defaultData.TopTags?.Any() ?? false))
                {
                    SubPartitionAdditionalDataChanged?.Invoke(
                        this,
                        new PartitionAdditionalDataChangedEventArgs(
                            subPartitionId,
                            requestDateTime,
                            tagList: defaultData.TopTags));
                }
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, offsetId > 0);
                if (offsetId == 0)
                {
                    throw;
                }
            }
        }
    }
}

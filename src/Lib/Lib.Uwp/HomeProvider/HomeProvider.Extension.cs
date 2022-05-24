// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Toolkit.Interfaces;
using static Bili.Models.App.Constants.AppConstants;

namespace Bili.Lib.Uwp
{
    /// <summary>
    /// 分区及标签的相关定义和扩展方法.
    /// </summary>
    public partial class HomeProvider
    {
        private readonly IHttpProvider _httpProvider;
        private readonly IAuthorizeProvider _authorizeProvider;
        private readonly ICommunityAdapter _communityAdapter;
        private readonly IVideoAdapter _videoAdapter;
        private readonly IFileToolkit _fileToolkit;
        private readonly IPgcAdapter _pgcAdapter;

        private readonly Dictionary<string, (int OffsetId, int PageNumber)> _cacheVideoPartitionOffsets;

        private long _recommendOffsetId;
        private long _popularOffsetId;
        private int _videoPartitionOffsetId = 0;
        private int _videoPartitionPageNumber = 1;
        private string _currentPartitionId = string.Empty;

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
            _cacheVideoPartitionOffsets.Remove(_currentPartitionId);
            _cacheVideoPartitionOffsets.Add(_currentPartitionId, (_videoPartitionOffsetId, _videoPartitionPageNumber));
        }

        private void RetriveCachedSubPartitionOffset(string partitionId)
        {
            if (_cacheVideoPartitionOffsets.ContainsKey(partitionId))
            {
                var (oid, pn) = _cacheVideoPartitionOffsets[partitionId];
                _videoPartitionOffsetId = oid;
                _videoPartitionPageNumber = pn;
            }

            _currentPartitionId = partitionId;
        }
    }
}

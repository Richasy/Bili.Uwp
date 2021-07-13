// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Lib.Interfaces
{
    /// <summary>
    /// 提供直播相关的操作.
    /// </summary>
    public interface ILiveProvider
    {
        /// <summary>
        /// 获取直播源列表.
        /// </summary>
        /// <param name="page">页码.</param>
        /// <returns><see cref="LiveFeedResponse"/>.</returns>
        Task<LiveFeedResponse> GetLiveFeedsAsync(int page);
    }
}

// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Lib.Interfaces
{
    /// <summary>
    /// 首页推荐视频处理程序.
    /// </summary>
    public interface IHomeProvider
    {
        /// <summary>
        /// 请求推荐视频列表.
        /// </summary>
        /// <param name="offsetIdx">偏移标识符.</param>
        /// <returns>推荐视频或番剧的列表.</returns>
        Task<List<RecommendCard>> RequestRecommendCardsAsync(int offsetIdx);
    }
}

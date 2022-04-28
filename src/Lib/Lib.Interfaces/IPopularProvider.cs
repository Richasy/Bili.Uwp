// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Bilibili.App.Card.V1;

namespace Bili.Lib.Interfaces
{
    /// <summary>
    /// 热门数据处理.
    /// </summary>
    public interface IPopularProvider
    {
        /// <summary>
        /// 获取热门详情.
        /// </summary>
        /// <param name="offsetIndex">偏移值Id.</param>
        /// <returns>排行榜信息.</returns>
        Task<List<Card>> GetPopularDetailAsync(int offsetIndex);
    }
}

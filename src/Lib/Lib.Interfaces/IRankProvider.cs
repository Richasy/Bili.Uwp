﻿// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Bilibili.App.Show.V1;

namespace Richasy.Bili.Lib.Interfaces
{
    /// <summary>
    /// 提供排行榜相关的操作.
    /// </summary>
    public interface IRankProvider
    {
        /// <summary>
        /// 获取排行榜详情.
        /// </summary>
        /// <param name="partitionId">分区Id. 如果是全区则为0.</param>
        /// <returns>排行榜信息.</returns>
        Task<List<Item>> GetRankDetailAsync(int partitionId);
    }
}

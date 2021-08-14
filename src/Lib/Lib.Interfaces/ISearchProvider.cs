// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Lib.Interfaces
{
    /// <summary>
    /// 搜索操作.
    /// </summary>
    public interface ISearchProvider
    {
        /// <summary>
        /// 获取热搜列表.
        /// </summary>
        /// <returns>热搜推荐列表.</returns>
        Task<List<SearchRecommendItem>> GetHotSearchListAsync();
    }
}

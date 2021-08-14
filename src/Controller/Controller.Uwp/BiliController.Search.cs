// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Controller.Uwp
{
    /// <summary>
    /// 控制器的搜索部分.
    /// </summary>
    public partial class BiliController
    {
        /// <summary>
        /// 获取热搜列表.
        /// </summary>
        /// <returns>热搜列表.</returns>
        public async Task<List<SearchRecommendItem>> GetHotSearchListAsync()
        {
            return await _searchProvider.GetHotSearchListAsync();
        }
    }
}

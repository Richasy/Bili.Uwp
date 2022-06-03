// Copyright (c) Richasy. All rights reserved.

using Bili.Models.BiliBili;
using Bili.Models.Data.Search;
using Bilibili.App.Interfaces.V1;

namespace Bili.Adapter.Interfaces
{
    /// <summary>
    /// 搜索数据适配器接口定义.
    /// </summary>
    public interface ISearchAdapter
    {
        /// <summary>
        /// 将热搜条目 <see cref="SearchRecommendItem"/> 转换为搜索建议条目.
        /// </summary>
        /// <param name="item">热搜条目.</param>
        /// <returns><see cref="SearchSuggest"/>.</returns>
        SearchSuggest ConvertToSearchSuggest(SearchRecommendItem item);

        /// <summary>
        /// 将来自 Web 的搜索建议条目 <see cref="ResultItem"/> 转换为本地搜索建议条目.
        /// </summary>
        /// <param name="item">来自 Web 的搜索建议条目.</param>
        /// <returns><see cref="SearchSuggest"/>.</returns>
        SearchSuggest ConvertToSearchSuggest(ResultItem item);

        /// <summary>
        /// 将综合搜索结果响应 <see cref="ComprehensiveSearchResultResponse"/> 转换为综合数据集.
        /// </summary>
        /// <param name="response">综合搜索结果响应.</param>
        /// <returns><see cref="ComprehensiveSet"/>.</returns>
        ComprehensiveSet ConvertToComprehensiveSet(ComprehensiveSearchResultResponse response);
    }
}

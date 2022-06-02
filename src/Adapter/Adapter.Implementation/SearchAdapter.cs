// Copyright (c) Richasy. All rights reserved.

using Bili.Adapter.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.Search;
using Bilibili.App.Interfaces.V1;

namespace Bili.Adapter
{
    /// <summary>
    /// 搜索数据适配器.
    /// </summary>
    public sealed class SearchAdapter : ISearchAdapter
    {
        /// <inheritdoc/>
        public SearchSuggest ConvertToSearchSuggest(SearchRecommendItem item)
            => new SearchSuggest(item.Position, item.DisplayName, item.Keyword, item.Icon);

        /// <inheritdoc/>
        public SearchSuggest ConvertToSearchSuggest(ResultItem item)
            => new SearchSuggest(item.Position, item.Title, item.Keyword);
    }
}

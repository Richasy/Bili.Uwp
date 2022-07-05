// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Bili.Adapter.Interfaces;
using Bili.Models.App.Constants;
using Bili.Models.BiliBili;
using Bili.Models.Data.Search;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bilibili.App.Interfaces.V1;

namespace Bili.Adapter
{
    /// <summary>
    /// 搜索数据适配器.
    /// </summary>
    public sealed class SearchAdapter : ISearchAdapter
    {
        private readonly IVideoAdapter _videoAdapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchAdapter"/> class.
        /// </summary>
        /// <param name="videoAdapter">视频数据适配器.</param>
        public SearchAdapter(IVideoAdapter videoAdapter)
            => _videoAdapter = videoAdapter;

        /// <inheritdoc/>
        public SearchSuggest ConvertToSearchSuggest(SearchRecommendItem item)
            => new SearchSuggest(item.Position, item.DisplayName, item.Keyword, item.Icon);

        /// <inheritdoc/>
        public SearchSuggest ConvertToSearchSuggest(ResultItem item)
            => new SearchSuggest(item.Position, item.Title, item.Keyword);

        /// <inheritdoc/>
        public ComprehensiveSet ConvertToComprehensiveSet(ComprehensiveSearchResultResponse response)
        {
            var metaList = new Dictionary<SearchModuleType, int>();
            foreach (var item in response.SubModuleList)
            {
                metaList.Add((SearchModuleType)item.Type, item.Total);
            }

            var isEnd = response.ItemList == null;
            var videos = isEnd
                ? new List<VideoInformation>()
                : response.ItemList.Where(p => p.Goto == ServiceConstants.Av).Select(p => _videoAdapter.ConvertToVideoInformation(p)).ToList();
            var videoSet = new SearchSet<VideoInformation>(videos, isEnd);

            return new ComprehensiveSet(videoSet, metaList);
        }
    }
}

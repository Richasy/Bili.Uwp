// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Search
{
    /// <summary>
    /// 搜索建议条目.
    /// </summary>
    public sealed class SearchSuggest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchSuggest"/> class.
        /// </summary>
        /// <param name="position">位置.</param>
        /// <param name="displayText">显示文本.</param>
        /// <param name="keyword">搜索关键词.</param>
        /// <param name="icon">图标.</param>
        public SearchSuggest(
            int position,
            string displayText,
            string keyword,
            string icon = default)
        {
            Position = position;
            DisplayText = displayText;
            Keyword = keyword;
            Icon = icon;
        }

        /// <summary>
        /// 位置/序号.
        /// </summary>
        public int Position { get; }

        /// <summary>
        /// 显示的文本.
        /// </summary>
        public string DisplayText { get; }

        /// <summary>
        /// 搜索关键词.
        /// </summary>
        public string Keyword { get; }

        /// <summary>
        /// 图标.
        /// </summary>
        public string Icon { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SearchSuggest suggest && Keyword == suggest.Keyword;

        /// <inheritdoc/>
        public override int GetHashCode() => Keyword.GetHashCode();
    }
}

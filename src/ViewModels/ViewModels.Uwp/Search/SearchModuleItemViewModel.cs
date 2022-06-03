// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Enums;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Search
{
    /// <summary>
    /// 搜索模块条目视图模型.
    /// </summary>
    public sealed class SearchModuleItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchModuleItemViewModel"/> class.
        /// </summary>
        /// <param name="type">搜索模块类型.</param>
        /// <param name="title">标题栏.</param>
        /// <param name="isEnabled">是否启用.</param>
        public SearchModuleItemViewModel(
            SearchModuleType type,
            string title,
            bool isEnabled = true)
        {
            Type = type;
            Title = title;
            IsEnabled = isEnabled;
        }

        /// <summary>
        /// 类型.
        /// </summary>
        [Reactive]
        public SearchModuleType Type { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <summary>
        /// 是否可用.
        /// </summary>
        [Reactive]
        public bool IsEnabled { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SearchModuleItemViewModel model && Type == model.Type;

        /// <inheritdoc/>
        public override int GetHashCode() => Type.GetHashCode();
    }
}

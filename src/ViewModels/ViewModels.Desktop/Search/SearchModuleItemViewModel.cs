// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Enums;
using Bili.ViewModels.Interfaces.Search;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bili.ViewModels.Desktop.Search
{
    /// <summary>
    /// 搜索模块条目视图模型.
    /// </summary>
    public sealed partial class SearchModuleItemViewModel : ViewModelBase, ISearchModuleItemViewModel
    {
        [ObservableProperty]
        private SearchModuleType _type;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private bool _isEnabled;

        /// <inheritdoc/>
        public void SetData(SearchModuleType type, string title, bool isEnabled = true)
        {
            Type = type;
            Title = title;
            IsEnabled = isEnabled;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SearchModuleItemViewModel model && Type == model.Type;

        /// <inheritdoc/>
        public override int GetHashCode() => Type.GetHashCode();
    }
}

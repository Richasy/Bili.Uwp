// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Article;
using Bili.Models.Data.Community;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Article;
using Bili.ViewModels.Interfaces.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Uwp.Article
{
    /// <summary>
    /// 文章分区页面视图模型.
    /// </summary>
    public sealed partial class ArticlePartitionPageViewModel
    {
        private readonly IArticleProvider _articleProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly Dictionary<Partition, IEnumerable<ArticleInformation>> _caches;

        [ObservableProperty]
        private Partition _currentPartition;

        [ObservableProperty]
        private ArticleSortType _sortType;

        [ObservableProperty]
        private bool _isRecommendPartition;

        [ObservableProperty]
        private bool _isShowBanner;

        /// <inheritdoc/>
        public ObservableCollection<IBannerViewModel> Banners { get; }

        /// <inheritdoc/>
        public ObservableCollection<IArticleItemViewModel> Ranks { get; }

        /// <inheritdoc/>
        public ObservableCollection<Partition> Partitions { get; }

        /// <inheritdoc/>
        public ObservableCollection<ArticleSortType> SortTypes { get; }

        /// <inheritdoc/>
        public IRelayCommand<Partition> SelectPartitionCommand { get; }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Article;
using Bili.Models.Data.Community;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Article;
using Bili.ViewModels.Interfaces.Common;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

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

        /// <inheritdoc/>
        [ObservableProperty]
        public Partition CurrentPartition { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public ArticleSortType SortType { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsRecommendPartition { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsShowBanner { get; set; }
    }
}

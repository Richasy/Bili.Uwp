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
        private readonly ObservableAsPropertyHelper<bool> _isShowBanner;
        private readonly ObservableAsPropertyHelper<bool> _isRecommendPartition;

        /// <summary>
        /// 横幅集合.
        /// </summary>
        public ObservableCollection<IBannerViewModel> Banners { get; }

        /// <summary>
        /// 横幅集合.
        /// </summary>
        public ObservableCollection<IArticleItemViewModel> Ranks { get; }

        /// <summary>
        /// 子分区集合.
        /// </summary>
        public ObservableCollection<Partition> Partitions { get; }

        /// <summary>
        /// 文章排序方式集合.
        /// </summary>
        public ObservableCollection<ArticleSortType> SortTypes { get; }

        /// <summary>
        /// 选中子分区命令.
        /// </summary>
        public ReactiveCommand<Partition, Unit> SelectPartitionCommand { get; }

        /// <summary>
        /// 当前选中的子分区.
        /// </summary>
        [Reactive]
        public Partition CurrentPartition { get; set; }

        /// <summary>
        /// 排序方式.
        /// </summary>
        [Reactive]
        public ArticleSortType SortType { get; set; }

        /// <summary>
        /// 是否为推荐子分区.
        /// </summary>
        public bool IsRecommendPartition => _isRecommendPartition.Value;

        /// <summary>
        /// 是否显示横幅内容.
        /// </summary>
        public bool IsShowBanner => _isShowBanner.Value;
    }
}

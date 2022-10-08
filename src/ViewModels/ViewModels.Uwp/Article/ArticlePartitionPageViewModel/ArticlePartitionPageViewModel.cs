// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Article;
using Bili.Models.Data.Community;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Article;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Uwp.Base;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Article
{
    /// <summary>
    /// 文章分区页面视图模型.
    /// </summary>
    public sealed partial class ArticlePartitionPageViewModel : InformationFlowViewModelBase<IArticleItemViewModel>, IArticlePartitionPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticlePartitionPageViewModel"/> class.
        /// </summary>
        /// <param name="resourceToolkit">资源管理工具.</param>
        /// <param name="articleProvider">文章服务提供工具.</param>
        /// <param name="coreDispatcher">UI 调度器.</param>
        public ArticlePartitionPageViewModel(
            IResourceToolkit resourceToolkit,
            IArticleProvider articleProvider,
            CoreDispatcher coreDispatcher)
            : base(coreDispatcher)
        {
            _resourceToolkit = resourceToolkit;
            _articleProvider = articleProvider;
            _caches = new Dictionary<Partition, IEnumerable<ArticleInformation>>();

            Banners = new ObservableCollection<IBannerViewModel>();
            Ranks = new ObservableCollection<IArticleItemViewModel>();
            Partitions = new ObservableCollection<Partition>();
            SortTypes = new ObservableCollection<ArticleSortType>()
            {
                ArticleSortType.Default,
                ArticleSortType.Newest,
                ArticleSortType.Read,
                ArticleSortType.Reply,
                ArticleSortType.Like,
                ArticleSortType.Favorite,
            };

            SortType = ArticleSortType.Default;

            var isRecommend = this.WhenAnyValue(
                x => x.CurrentPartition,
                partition => partition?.Id == "0");

            isRecommend.Merge(this.WhenAnyValue(x => x.Banners.Count, count => count > 0))
                .ToPropertyEx(this, x => x.IsShowBanner);

            isRecommend.ToPropertyEx(this, x => x.IsRecommendPartition);

            SelectPartitionCommand = new RelayCommand<Partition>(SelectPartition);
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
        {
            if (CurrentPartition != null)
            {
                _articleProvider.ResetPartitionStatus(CurrentPartition.Id);
            }
        }

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
            => $"{_resourceToolkit.GetLocaleString(LanguageNames.RequestArticleFailed)}\n{errorMsg}";

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            if (Partitions.Count == 0)
            {
                var partitions = await _articleProvider.GetPartitionsAsync();
                partitions.ToList().ForEach(p => Partitions.Add(p));

                await FakeLoadingAsync();
                CurrentPartition = partitions.First();
            }

            var partition = CurrentPartition;
            var data = await _articleProvider.GetPartitionArticlesAsync(partition.Id, SortType);
            if (data.Banners?.Count() > 0)
            {
                foreach (var item in data.Banners)
                {
                    if (!Banners.Any(p => p.Uri == item.Uri))
                    {
                        var vm = Locator.Current.GetService<IBannerViewModel>();
                        vm.InjectData(item);
                        Banners.Add(vm);
                    }
                }
            }

            if (data.Ranks?.Count() > 0)
            {
                foreach (var article in data.Ranks)
                {
                    if (!Ranks.Any(p => p.Data.Equals(article)))
                    {
                        var articleVM = Locator.Current.GetService<IArticleItemViewModel>();
                        articleVM.InjectData(article);
                        Ranks.Add(articleVM);
                    }
                }
            }

            if (data.Articles?.Count() > 0)
            {
                foreach (var article in data.Articles)
                {
                    if (Items.Any(p => p.Data.Equals(article)))
                    {
                        continue;
                    }

                    var articleVM = Locator.Current.GetService<IArticleItemViewModel>();
                    articleVM.InjectData(article);
                    Items.Add(articleVM);
                }

                var videos = Items
                        .OfType<IArticleItemViewModel>()
                        .Select(p => p.Data)
                        .ToList();
                if (_caches.ContainsKey(CurrentPartition))
                {
                    _caches[CurrentPartition] = videos;
                }
                else
                {
                    _caches.Add(CurrentPartition, videos);
                }
            }
        }

        private void SelectPartition(Partition partition)
        {
            TryClear(Items);
            CurrentPartition = partition;
            if (_caches.ContainsKey(partition))
            {
                var items = _caches[partition];
                foreach (var data in items)
                {
                    var articleVM = Locator.Current.GetService<IArticleItemViewModel>();
                    articleVM.InjectData(data);
                    Items.Add(articleVM);
                }
            }
            else
            {
                InitializeCommand.Execute().Subscribe();
            }
        }
    }
}

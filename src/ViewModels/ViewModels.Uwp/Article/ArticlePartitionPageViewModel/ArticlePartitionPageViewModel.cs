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
using Bili.ViewModels.Uwp.Base;
using ReactiveUI;
using Splat;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Article
{
    /// <summary>
    /// 文章分区页面视图模型.
    /// </summary>
    public sealed partial class ArticlePartitionPageViewModel : InformationFlowViewModelBase<ArticleItemViewModel>
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

            Banners = new ObservableCollection<BannerViewModel>();
            Ranks = new ObservableCollection<ArticleItemViewModel>();
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

            _isShowBanner = isRecommend.Merge(this.WhenAnyValue(x => x.Banners.Count, count => count > 0))
                .ToProperty(this, x => x.IsShowBanner, scheduler: RxApp.MainThreadScheduler);

            _isRecommendPartition = isRecommend
                .ToProperty(this, x => x.IsRecommendPartition, scheduler: RxApp.MainThreadScheduler);

            SelectPartitionCommand = ReactiveCommand.Create<Partition>(SelectPartition, outputScheduler: RxApp.MainThreadScheduler);
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
                        Banners.Add(new BannerViewModel(item));
                    }
                }
            }

            if (data.Ranks?.Count() > 0)
            {
                foreach (var article in data.Ranks)
                {
                    if (!Ranks.Any(p => p.Information.Equals(article)))
                    {
                        var articleVM = Splat.Locator.Current.GetService<ArticleItemViewModel>();
                        articleVM.SetInformation(article);
                        Ranks.Add(articleVM);
                    }
                }
            }

            if (data.Articles?.Count() > 0)
            {
                foreach (var article in data.Articles)
                {
                    var articleVM = Splat.Locator.Current.GetService<ArticleItemViewModel>();
                    articleVM.SetInformation(article);
                    Items.Add(articleVM);
                }

                var videos = Items
                        .OfType<ArticleItemViewModel>()
                        .Select(p => p.Information)
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
            Items.Clear();
            CurrentPartition = partition;
            if (_caches.ContainsKey(partition))
            {
                var data = _caches[partition];
                foreach (var video in data)
                {
                    var videoVM = Splat.Locator.Current.GetService<ArticleItemViewModel>();
                    videoVM.SetInformation(video);
                    Items.Add(videoVM);
                }
            }
            else
            {
                InitializeCommand.Execute().Subscribe();
            }
        }
    }
}

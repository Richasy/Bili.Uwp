// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 专栏分类视图模型.
    /// </summary>
    public partial class SpecialColumnCategoryViewModel : WebRequestViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialColumnCategoryViewModel"/> class.
        /// </summary>
        /// <param name="category">分类.</param>
        public SpecialColumnCategoryViewModel(ArticleCategory category)
            : this()
        {
            Category = category;
            Id = category.Id;
            if (category.Children?.Any() ?? false)
            {
                category.Children.ForEach(p => Children.Add(new SpecialColumnCategoryViewModel(p)));
                CurrentCategory = Children.First();
            }

            if (category.Id != 0)
            {
                GenerateSortType();
                CurrentSortType = ArticleSortType.Default;
            }

            Title = category.Name;
            IsRecommend = category.Id == 0;
            CurrentSortType = ArticleSortType.Default;
            this.PropertyChanged += OnPropertyChangedAsync;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialColumnCategoryViewModel"/> class.
        /// </summary>
        protected SpecialColumnCategoryViewModel()
        {
            ArticleCollection = new ObservableCollection<ArticleViewModel>();
            Children = new ObservableCollection<SpecialColumnCategoryViewModel>();
            BannerCollection = new ObservableCollection<BannerViewModel>();
            RankCollection = new ObservableCollection<ArticleViewModel>();
        }

        /// <summary>
        /// 激活该分区.
        /// </summary>
        public void Activate()
        {
            Controller.SpecialColumnArticleIteration += OnCategoryArticleIteration;
            Controller.SpecialColumnAdditionalDataChanged += OnCategoryAdditionalDataChanged;
            SpecialColumnModuleViewModel.Instance.CurrentCategory = this;
            IsActive = true;
            IsRequested = _pageNumber > 1;
        }

        /// <summary>
        /// 停用该分区.
        /// </summary>
        public void Deactive()
        {
            Controller.SpecialColumnArticleIteration -= OnCategoryArticleIteration;
            Controller.SpecialColumnAdditionalDataChanged -= OnCategoryAdditionalDataChanged;
            IsActive = false;
        }

        /// <summary>
        /// 检查激活状态.
        /// </summary>
        /// <param name="categoryId">需要激活的分类Id.</param>
        public void CheckActive(int categoryId)
        {
            if (categoryId == Id)
            {
                if (!IsActive)
                {
                    Activate();
                }
            }
            else
            {
                Deactive();
                if (Children?.Any() ?? false)
                {
                    foreach (var item in Children)
                    {
                        item.CheckActive(categoryId);
                    }
                }
            }
        }

        /// <summary>
        /// 请求数据.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestDataAsync()
        {
            if (IsRequested)
            {
                await DeltaRequestAsync();
            }
            else
            {
                await InitializeRequestAsync();
            }

            IsRequested = _pageNumber > 1;
        }

        /// <summary>
        /// 执行初始请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeRequestAsync()
        {
            if (!IsInitializeLoading && !IsDeltaLoading)
            {
                IsInitializeLoading = true;
                ArticleCollection.Clear();
                _pageNumber = 1;
                IsError = false;
                ErrorText = string.Empty;
                try
                {
                    if (!IsRecommend)
                    {
                        await Controller.RequestCategoryArticlesAsync(Id, _pageNumber, CurrentSortType);
                    }
                    else
                    {
                        await Controller.RequestRecommendArticlesAsync(_pageNumber);
                    }
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestArticleListFailed)}\n{ex.Error?.Message ?? ex.Message}";
                }
                catch (InvalidOperationException invalidEx)
                {
                    IsError = true;
                    ErrorText = invalidEx.Message;
                }

                IsInitializeLoading = false;
            }

            IsRequested = _pageNumber > 1;
        }

        /// <summary>
        /// 执行增量请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        internal async Task DeltaRequestAsync()
        {
            if (!IsDeltaLoading)
            {
                IsDeltaLoading = true;
                if (!IsRecommend)
                {
                    await Controller.RequestCategoryArticlesAsync(Id, _pageNumber, CurrentSortType);
                }
                else
                {
                    await Controller.RequestRecommendArticlesAsync(_pageNumber);
                }

                IsDeltaLoading = false;
            }
        }

        private void GenerateSortType()
        {
            SortTypeCollection = new ObservableCollection<ArticleSortType>()
            {
                ArticleSortType.Default,
                ArticleSortType.Newest,
                ArticleSortType.Read,
                ArticleSortType.Reply,
                ArticleSortType.Like,
                ArticleSortType.Favorite,
            };
        }

        private void OnCategoryAdditionalDataChanged(object sender, SpecialColumnAdditionalDataChangedEventArgs e)
        {
            IsShowBanner = e.Banners?.Any() ?? false;
            IsShowRank = e.Ranks?.Any() ?? false;
            if (IsShowBanner)
            {
                BannerCollection.Clear();
                e.Banners.ForEach(p => BannerCollection.Add(new BannerViewModel(p)));
            }

            if (IsShowRank)
            {
                RankCollection.Clear();
                e.Ranks.ForEach(p => RankCollection.Add(new ArticleViewModel(p)));
            }
        }

        private void OnCategoryArticleIteration(object sender, SpecialColumnArticleIterationEventArgs e)
        {
            if (e.CategoryId == Id)
            {
                _pageNumber = e.NextPageNumber;

                if (e.Articles?.Any() ?? false)
                {
                    e.Articles.ForEach(p => ArticleCollection.Add(new ArticleViewModel(p)));
                }
            }
        }

        private async void OnPropertyChangedAsync(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrentSortType))
            {
                await InitializeRequestAsync();
            }
        }
    }
}

// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 文章收藏夹视图模型.
    /// </summary>
    public partial class FavoriteArticleViewModel : WebRequestViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleFavoriteViewModel"/> class.
        /// </summary>
        /// <param name="type">收藏夹类型.</param>
        protected FavoriteArticleViewModel()
        {
            ArticleCollection = new ObservableCollection<ArticleViewModel>();
            Controller.ArticleFavoriteIteration += OnArticleFavoriteIteration;
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
        }

        /// <summary>
        /// 初始化请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeRequestAsync()
        {
            if (!IsInitializeLoading && !IsDeltaLoading && !_isCompleted)
            {
                IsInitializeLoading = true;
                IsError = false;
                _pageNumber = 1;
                ArticleCollection.Clear();
                try
                {
                    await Controller.RequestArticleFavoriteListAsync(_pageNumber);
                    IsRequested = true;
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestArticleFavoriteFailed)}\n{ex.Error?.Message ?? ex.Message}";
                }
                catch (InvalidOperationException invalidEx)
                {
                    IsError = true;
                    ErrorText = invalidEx.Message;
                }

                IsInitializeLoading = false;
            }
        }

        /// <summary>
        /// 取消关注文章.
        /// </summary>
        /// <param name="vm">文章视图模型.</param>
        /// <returns>取消收藏结果.</returns>
        public async Task RemoveFavoriteArticleAsync(ArticleViewModel vm)
        {
            var result = await Controller.RemoveFavoriteArticleAsync(Convert.ToInt32(vm.Id));
            if (result)
            {
                ArticleCollection.Remove(vm);
                IsShowEmpty = ArticleCollection.Count == 0;
            }
        }

        /// <summary>
        /// 数据源增量请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        internal async Task DeltaRequestAsync()
        {
            if (!IsDeltaLoading && !IsInitializeLoading && !_isCompleted)
            {
                IsDeltaLoading = true;
                await Controller.RequestArticleFavoriteListAsync(_pageNumber);
                IsDeltaLoading = false;
            }
        }

        private void OnArticleFavoriteIteration(object sender, FavoriteArticleIterationEventArgs e)
        {
            if (e.List != null && e.List.Count > 0)
            {
                e.List.ForEach(p => ArticleCollection.Add(new ArticleViewModel(p)));
            }

            _pageNumber = e.NextPageNumber;
            _isCompleted = ArticleCollection.Count >= e.TotalCount;
            IsShowEmpty = ArticleCollection.Count == 0;
        }
    }
}

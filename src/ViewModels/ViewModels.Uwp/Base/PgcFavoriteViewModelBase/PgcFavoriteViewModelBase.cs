// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Models.Enums.App;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// PGC收藏夹视图模型.
    /// </summary>
    public partial class PgcFavoriteViewModelBase : WebRequestViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcFavoriteViewModelBase"/> class.
        /// </summary>
        /// <param name="type">收藏夹类型.</param>
        public PgcFavoriteViewModelBase(FavoriteType type)
        {
            Type = type;
            SeasonCollection = new ObservableCollection<SeasonViewModel>();
            Controller.PgcFavoriteIteration += OnPgcFavoriteIteration;
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
            if (!IsInitializeLoading && !IsDeltaLoading)
            {
                IsInitializeLoading = true;
                IsError = false;
                _pageNumber = 1;
                SeasonCollection.Clear();
                try
                {
                    await Controller.RequestPgcFavoriteListAsync(_pageNumber, Type);
                    IsRequested = true;
                }
                catch (ServiceException ex)
                {
                    var prefix = string.Empty;
                    if (Type == FavoriteType.Anime)
                    {
                        prefix = ResourceToolkit.GetLocaleString(LanguageNames.RequestAnimeFavoriteFailed);
                    }
                    else if (Type == FavoriteType.Cinema)
                    {
                        prefix = ResourceToolkit.GetLocaleString(LanguageNames.RequestCinemaFavoriteFailed);
                    }

                    IsError = true;
                    ErrorText = $"{prefix}\n{ex.Error?.Message ?? ex.Message}";
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
        /// 取消关注番剧/影视.
        /// </summary>
        /// <param name="vm">剧集视图模型.</param>
        /// <returns>取消收藏结果.</returns>
        public async Task RemoveFavoritePgcAsync(SeasonViewModel vm)
        {
            var result = await Controller.RemoveFavoritePgcAsync(vm.SeasonId);
            if (result)
            {
                SeasonCollection.Remove(vm);
                IsShowEmpty = SeasonCollection.Count == 0;
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
                await Controller.RequestPgcFavoriteListAsync(_pageNumber, Type);
                IsDeltaLoading = false;
            }
        }

        private void OnPgcFavoriteIteration(object sender, FavoritePgcIterationEventArgs e)
        {
            if (e.Type == Type)
            {
                if (e.List != null && e.List.Count > 0)
                {
                    e.List.ForEach(p => SeasonCollection.Add(new SeasonViewModel(p)));
                }

                _pageNumber = e.NextPageNumber;
                _isCompleted = !e.HasMore || SeasonCollection.Count >= e.TotalCount;
                IsShowEmpty = SeasonCollection.Count == 0;
            }
        }
    }
}

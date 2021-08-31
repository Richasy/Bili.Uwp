// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// PGC播放列表视图模型.
    /// </summary>
    public partial class PgcPlayListViewModel : WebRequestViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcPlayListViewModel"/> class.
        /// </summary>
        protected PgcPlayListViewModel()
        {
            SeasonCollection = new ObservableCollection<SeasonViewModel>();
        }

        /// <summary>
        /// 初始化.
        /// </summary>
        /// <param name="playListId">播放列表Id.</param>
        /// <param name="isRefresh">是否刷新.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeAsync(int playListId, bool isRefresh = false)
        {
            if (playListId == Id && !isRefresh)
            {
                return;
            }

            if (IsInitializeLoading)
            {
                return;
            }

            Id = playListId;
            SeasonCollection.Clear();
            Title = "--";
            TotalCount = "--";
            try
            {
                IsInitializeLoading = true;
                var response = await Controller.GetPgcPlayListAsync(Id);
                Title = response.Title;
                TotalCount = response.Total;
                foreach (var item in response.Seasons)
                {
                    SeasonCollection.Add(SeasonViewModel.CreateFromPlayListItem(item));
                }
            }
            catch (ServiceException ex)
            {
                IsError = true;
                ErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestPlayListFailed)}\n{ex.Error?.Message ?? ex.Message}";
            }
            catch (Exception invalidEx)
            {
                IsError = true;
                ErrorText = invalidEx.Message;
            }

            IsInitializeLoading = false;
        }
    }
}

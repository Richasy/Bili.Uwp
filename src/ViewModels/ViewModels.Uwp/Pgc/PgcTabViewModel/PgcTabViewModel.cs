// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// PGC标签页视图模型.
    /// </summary>
    public partial class PgcTabViewModel : WebRequestViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcTabViewModel"/> class.
        /// </summary>
        /// <param name="tab">标签页数据.</param>
        public PgcTabViewModel(PgcTab tab)
            : this()
        {
            Id = tab.Id;
            Title = tab.Title;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PgcTabViewModel"/> class.
        /// </summary>
        protected PgcTabViewModel()
        {
            RankCollection = new ObservableCollection<PgcModuleViewModel>();
            ModuleCollection = new ObservableCollection<PgcModuleViewModel>();
            BannerCollection = new ObservableCollection<BannerViewModel>();
            VideoCollection = new ObservableCollection<VideoViewModel>();
        }

        /// <summary>
        /// 激活该标签页.
        /// </summary>
        public void Activate()
        {
            Controller.PgcModuleAdditionalDataChanged += OnPgcModuleAdditionalDataChangedAsync;
            Controller.PgcModuleIteration += OnPgcModuleIteration;
            Controller.SubPartitionVideoIteration += OnVideoIteration;
            IsActivate = true;
        }

        /// <summary>
        /// 停用该标签页.
        /// </summary>
        public void Deactive()
        {
            Controller.PgcModuleAdditionalDataChanged -= OnPgcModuleAdditionalDataChangedAsync;
            Controller.PgcModuleIteration -= OnPgcModuleIteration;
            Controller.SubPartitionVideoIteration -= OnVideoIteration;
            IsActivate = false;
        }

        /// <summary>
        /// 初始化请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeRequestAsync()
        {
            if (!IsInitializeLoading)
            {
                IsInitializeLoading = true;
                IsError = false;
                BannerCollection.Clear();
                ModuleCollection.Clear();
                RankCollection.Clear();
                try
                {
                    await Controller.RequestPgcPageDetailAsync(Id);
                    IsRequested = true;
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestTabDetailFailed)}\n{ex.Error?.Message ?? ex.Message}";
                }
                catch (InvalidOperationException invalidEx)
                {
                    IsError = true;
                    ErrorText = invalidEx.Message;
                }

                IsShowBanner = BannerCollection.Count > 0;
                IsInitializeLoading = false;
            }
        }

        /// <summary>
        /// 初始化分区请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializePartitionRequestAsync()
        {
            if (!IsPartitionInitializeLoading && !IsDeltaLoading)
            {
                IsPartitionInitializeLoading = true;
                VideoCollection.Clear();
                _offsetId = 0;
                IsError = false;

                try
                {
                    await Controller.RequestSubPartitionDataAsync(PartitionId, _offsetId);
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestSubPartitionFailed)}\n{ex.Error?.Message ?? ex.Message}";
                }
                catch (InvalidOperationException invalidEx)
                {
                    IsError = true;
                    ErrorText = invalidEx.Message;
                }

                IsPartitionInitializeLoading = false;
            }
        }

        /// <summary>
        /// 分区视频增量请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task DeltaPartitionRequestAsync()
        {
            if (!IsDeltaLoading && !IsPartitionInitializeLoading)
            {
                IsDeltaLoading = true;
                await Controller.RequestSubPartitionDataAsync(PartitionId, _offsetId);
                IsDeltaLoading = false;
            }
        }

        private void OnPgcModuleIteration(object sender, PgcModuleIterationEventArgs e)
        {
            if (e.TabId == Id)
            {
                foreach (var item in e.Modules)
                {
                    ModuleCollection.Add(PgcModuleViewModel.CreateFromAnime(item));
                }
            }
        }

        private async void OnPgcModuleAdditionalDataChangedAsync(object sender, PgcModuleAdditionalDataChangedEventArgs e)
        {
            if (e.TabId == Id)
            {
                if (e.Banners?.Any() ?? false)
                {
                    e.Banners.ForEach(p => BannerCollection.Add(new BannerViewModel(p)));
                }

                if (e.Rank?.Any() ?? false)
                {
                    e.Rank.ForEach(p => RankCollection.Add(PgcModuleViewModel.CreateFromRank(p)));
                }

                if (e.RequestPartitionId > 0)
                {
                    PartitionId = e.RequestPartitionId;
                    await InitializePartitionRequestAsync();
                }

                IsShowVideo = PartitionId > 0;
                IsShowBanner = BannerCollection.Any();
            }
        }

        private void OnVideoIteration(object sender, PartitionVideoIterationEventArgs e)
        {
            if (e.SubPartitionId == PartitionId)
            {
                _offsetId = e.BottomOffsetId;
                if (e.VideoList?.Any() ?? false)
                {
                    e.VideoList.ForEach(p => VideoCollection.Add(new VideoViewModel(p)));
                }
            }
        }
    }
}

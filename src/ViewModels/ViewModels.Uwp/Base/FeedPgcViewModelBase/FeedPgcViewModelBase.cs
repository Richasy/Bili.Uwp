// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 数据源式PGC视图模型基类.
    /// </summary>
    public partial class FeedPgcViewModelBase : PgcViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FeedPgcViewModelBase"/> class.
        /// </summary>
        /// <param name="type">PGC类型.</param>
        public FeedPgcViewModelBase(PgcType type)
            : base(type)
        {
            SeasonCollection = new ObservableCollection<SeasonViewModel>();
            BannerCollection = new ObservableCollection<BannerViewModel>();
            Controller.PgcModuleAdditionalDataChanged += OnPgcModuleAdditionalDataChanged;
            Controller.PgcModuleIteration += OnPgcModuleIteration;
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
                BannerCollection.Clear();
                SeasonCollection.Clear();
                try
                {
                    await Controller.RequestPgcPageDetailAsync(Type, _cursor);
                    IsRequested = true;
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestFeedDetailFailed)}\n{ex.Error?.Message ?? ex.Message}";
                }
                catch (InvalidOperationException invalidEx)
                {
                    IsError = true;
                    ErrorText = invalidEx.Message;
                }

                IsShowBanner = BannerCollection.Any();
                IsInitializeLoading = false;
            }
        }

        /// <summary>
        /// 数据源增量请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task DeltaRequestAsync()
        {
            if (!IsDeltaLoading && !IsInitializeLoading)
            {
                IsDeltaLoading = true;
                await Controller.RequestPgcPageDetailAsync(Type, _cursor);
                IsDeltaLoading = false;
            }
        }

        private void OnPgcModuleIteration(object sender, PgcModuleIterationEventArgs e)
        {
            if (e.PgcType == Type)
            {
                var module = e.Modules.First();
                module.Items.ForEach(p => SeasonCollection.Add(new SeasonViewModel(p)));
                _cursor = e.Cursor;
            }
        }

        private void OnPgcModuleAdditionalDataChanged(object sender, PgcModuleAdditionalDataChangedEventArgs e)
        {
            if (e.PgcType == Type)
            {
                if (e.Banners?.Any() ?? false)
                {
                    e.Banners.ForEach(p => BannerCollection.Add(new BannerViewModel(p)));
                }

                IsShowBanner = BannerCollection.Any();
            }
        }
    }
}

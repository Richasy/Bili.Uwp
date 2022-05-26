// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using Bili.ViewModels.Uwp.Pgc;
using ReactiveUI;
using Splat;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Base
{
    /// <summary>
    /// PGC 信息流页面（不包括动漫）的通用视图模型.
    /// </summary>
    public partial class PgcPageViewModelBase : InformationFlowViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcPageViewModelBase"/> class.
        /// </summary>
        /// <param name="pgcProvider">PGC 服务提供工具.</param>
        /// <param name="resourceToolkit">资源管理工具.</param>
        /// <param name="dispatcher">UI 调度器.</param>
        /// <param name="navigationViewModel">导航服务视图模型.</param>
        /// <param name="type">PGC 类型.</param>
        public PgcPageViewModelBase(
            IPgcProvider pgcProvider,
            IResourceToolkit resourceToolkit,
            CoreDispatcher dispatcher,
            NavigationViewModel navigationViewModel,
            PgcType type)
            : base(dispatcher)
        {
            _type = type;
            _pgcProvider = pgcProvider;
            _resourceToolkit = resourceToolkit;
            _navigationViewModel = navigationViewModel;
            Banners = new ObservableCollection<BannerViewModel>();

            GotoIndexPageCommand = ReactiveCommand.Create(GotoIndexPage, outputScheduler: RxApp.MainThreadScheduler);

            Title = _type switch
            {
                PgcType.Documentary => _resourceToolkit.GetLocaleString(LanguageNames.Documentary),
                PgcType.Movie => _resourceToolkit.GetLocaleString(LanguageNames.Movie),
                PgcType.TV => _resourceToolkit.GetLocaleString(LanguageNames.TV),
                _ => throw new ArgumentException("不是预期的 PGC 类型", nameof(type))
            };
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
        {
            VideoCollection.Clear();
            Banners.Clear();
            IsShowBanner = false;
            _pgcProvider.ResetPageStatus(_type);
        }

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            var data = await _pgcProvider.GetPageDetailAsync(_type);
            if (data.Banners != null && data.Banners.Count() > 0)
            {
                data.Banners.ToList().ForEach(p => Banners.Add(new BannerViewModel(p)));
            }

            IsShowBanner = Banners.Count > 0;

            if (data.Seasons != null && data.Seasons.Count() > 0)
            {
                foreach (var item in data.Seasons)
                {
                    var seasonVM = Splat.Locator.Current.GetService<SeasonItemViewModel>();
                    seasonVM.SetInformation(item);
                    VideoCollection.Add(seasonVM);
                }
            }
        }

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
            => $"{_resourceToolkit.GetLocaleString(LanguageNames.RequestFeedDetailFailed)}\n{errorMsg}";

        private void GotoIndexPage()
            => _navigationViewModel.NavigateToSecondaryView(PageIds.PgcIndex, _type);
    }
}

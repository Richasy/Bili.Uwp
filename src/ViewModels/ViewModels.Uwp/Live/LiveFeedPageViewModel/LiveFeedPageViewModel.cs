// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.App.Args;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Live;
using Bili.ViewModels.Uwp.Base;
using ReactiveUI;
using Splat;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Live
{
    /// <summary>
    /// 直播首页视图模型.
    /// </summary>
    public sealed partial class LiveFeedPageViewModel : InformationFlowViewModelBase<ILiveItemViewModel>, ILiveFeedPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveFeedPageViewModel"/> class.
        /// </summary>
        /// <param name="dispatcher">UI调度器.</param>
        /// <param name="liveProvider">直播服务提供工具.</param>
        /// <param name="authorizeProvider">账户服务提供工具.</param>
        /// <param name="resourceToolkit">资源工具.</param>
        public LiveFeedPageViewModel(
            CoreDispatcher dispatcher,
            ILiveProvider liveProvider,
            IAuthorizeProvider authorizeProvider,
            IResourceToolkit resourceToolkit,
            INavigationViewModel navigationViewModel)
            : base(dispatcher)
        {
            _liveProvider = liveProvider;
            _authorizeProvider = authorizeProvider;
            _resourceToolkit = resourceToolkit;
            _navigationViewModel = navigationViewModel;

            Banners = new ObservableCollection<IBannerViewModel>();
            Follows = new ObservableCollection<ILiveItemViewModel>();
            HotPartitions = new ObservableCollection<Models.Data.Community.Partition>();

            SeeAllPartitionsCommand = ReactiveCommand.Create(SeeAllPartitions);

            _authorizeProvider.StateChanged += OnAuthorizeStateChanged;
            IsLoggedIn = _authorizeProvider.State == AuthorizeState.SignedIn;
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
        {
            _liveProvider.ResetFeedState();
            TryClear(Banners);
            TryClear(Follows);
            TryClear(HotPartitions);
        }

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
            => $"{_resourceToolkit.GetLocaleString(LanguageNames.RequestLiveFailed)}\n{errorMsg}";

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            var data = await _liveProvider.GetLiveFeedsAsync();

            if (data.Banners.Count() > 0)
            {
                data.Banners.ToList().ForEach(p =>
                {
                    var vm = Locator.Current.GetService<IBannerViewModel>();
                    vm.InjectData(p);
                    Banners.Add(vm);
                });
            }

            if (data.HotPartitions.Count() > 0)
            {
                data.HotPartitions.ToList().ForEach(p => HotPartitions.Add(p));
            }

            if (data.RecommendLives.Count() > 0)
            {
                foreach (var item in data.RecommendLives)
                {
                    var liveVM = Locator.Current.GetService<ILiveItemViewModel>();
                    liveVM.InjectData(item);
                    Items.Add(liveVM);
                }
            }

            if (data.FollowLives.Count() > 0)
            {
                foreach (var item in data.FollowLives)
                {
                    var liveVM = Locator.Current.GetService<ILiveItemViewModel>();
                    liveVM.InjectData(item);
                    Follows.Add(liveVM);
                }
            }

            IsFollowsEmpty = Follows.Count == 0;
        }

        private void SeeAllPartitions()
            => _navigationViewModel.NavigateToSecondaryView(PageIds.LivePartition);

        private void OnAuthorizeStateChanged(object sender, AuthorizeStateChangedEventArgs e)
            => IsLoggedIn = e.NewState == AuthorizeState.SignedIn;
    }
}

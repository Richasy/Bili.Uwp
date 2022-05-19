// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp;
using Splat;
using Windows.UI.Xaml;

namespace Bili.App.Controls
{
    /// <summary>
    /// 用户视图.
    /// </summary>
    public partial class UserView : CenterPopup
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(UserViewModel), typeof(UserView), new PropertyMetadata(default));

        private readonly INavigationViewModel _navigationViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserView"/> class.
        /// </summary>
        protected UserView()
        {
            InitializeComponent();
            _navigationViewModel = Splat.Locator.Current.GetService<INavigationViewModel>();
        }

        /// <summary>
        /// 单例.
        /// </summary>
        public static UserView Instance { get; } = new Lazy<UserView>(() => new UserView()).Value;

        /// <summary>
        /// 视图模型.
        /// </summary>
        public UserViewModel ViewModel
        {
            get { return (UserViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 显示.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ShowAsync(int userId)
        {
            Container.Show();
            if (ViewModel == null || ViewModel.Id != userId)
            {
                // 请求用户数据.
                ViewModel = new UserViewModel(userId);
                await ViewModel.InitializeUserDetailAsync();
            }
            else
            {
                ViewModel.Active();
            }
        }

        /// <summary>
        /// 显示.
        /// </summary>
        /// <param name="vm">用户数据模型.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task ShowAsync(UserViewModel vm)
        {
            Container.Show();
            if (ViewModel == null || ViewModel.Id != vm.Id)
            {
                // 请求用户数据.
                ViewModel = vm;
                if (!vm.IsRequested)
                {
                    await ViewModel.InitializeUserDetailAsync();
                }
            }

            ViewModel.Active();
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            if (ViewModel.IsSearching)
            {
                await ViewModel.InitializeSearchResultAsync();
            }
            else
            {
                await ViewModel.InitializeUserDetailAsync();
            }
        }

        private async void OnVideoViewRequestLoadMoreAsync(object sender, System.EventArgs e)
            => await ViewModel.DeltaRequestVideoAsync();

        private void OnVideoCardClick(object sender, VideoViewModel e) => Container.Hide();

        private async void OnFollowButtonClickAsync(object sender, RoutedEventArgs e)
            => await ViewModel.ToggleFollowStateAsync();

        private void OnFansButtonClick(object sender, RoutedEventArgs e)
        {
            _navigationViewModel.NavigateToSecondaryView(Models.Enums.PageIds.Fans, ViewModel);
            Container.Hide();
        }

        private void OnFollowUserButtonClick(object sender, RoutedEventArgs e)
        {
            _navigationViewModel.NavigateToSecondaryView(Models.Enums.PageIds.Follows, ViewModel);
            Container.Hide();
        }

        private void OnClosed(object sender, System.EventArgs e)
            => ViewModel.Deactive();

        private async void OnFixButtonClickAsync(object sender, RoutedEventArgs e)
            => await ViewModel.ToggleFixStateAsync();

        private void OnSearchButtonClick(object sender, RoutedEventArgs e)
            => ViewModel.IsSearching = true;

        private void OnExitSearchButtonClick(object sender, RoutedEventArgs e)
            => ViewModel.IsSearching = false;

        private async void OnSearchViewRequestLoadMoreAsync(object sender, EventArgs e)
            => await ViewModel.DeltaRequestSearchAsync();

        private async void OnSearchBoxQuerySubmittedAsync(Windows.UI.Xaml.Controls.AutoSuggestBox sender, Windows.UI.Xaml.Controls.AutoSuggestBoxQuerySubmittedEventArgs args)
            => await ViewModel.InitializeSearchResultAsync();
    }
}

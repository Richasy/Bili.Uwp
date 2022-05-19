// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.App.Controls;
using Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Bili.App.Pages.Desktop.Overlay
{
    /// <summary>
    /// 关注用户页面.
    /// </summary>
    public sealed partial class FollowsPage : AppPage
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(FollowsViewModel), typeof(FollowsPage), new PropertyMetadata(FollowsViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="FansPage"/> class.
        /// </summary>
        public FollowsPage()
        {
            InitializeComponent();
            Loaded += OnLoadedAsync;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public FollowsViewModel ViewModel
        {
            get { return (FollowsViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <inheritdoc/>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Tuple<int, string> userInfo)
            {
                var canRefresh = ViewModel.SetUser(userInfo.Item1, userInfo.Item2);
                if (IsLoaded && canRefresh)
                {
                    await ViewModel.InitializeRequestAsync();
                }
            }
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsRequested)
            {
                await ViewModel.InitializeRequestAsync();
            }
        }

        private async void OnFollowsRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeRequestAsync();
        }

        private async void OnViewRequestLoadMoreAsync(object sender, EventArgs e)
        {
            await ViewModel.RequestDataAsync();
        }

        private async void OnUserCardClickAsync(object sender, System.EventArgs e)
        {
            await UserView.Instance.ShowAsync((sender as UserSlimCard).ViewModel);
        }
    }
}

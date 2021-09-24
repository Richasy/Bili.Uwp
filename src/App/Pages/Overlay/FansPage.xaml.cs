// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using Richasy.Bili.App.Controls;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Richasy.Bili.App.Pages.Overlay
{
    /// <summary>
    /// 粉丝详情页面.
    /// </summary>
    public sealed partial class FansPage : Page
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(FansViewModel), typeof(FansPage), new PropertyMetadata(FansViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="FansPage"/> class.
        /// </summary>
        public FansPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoadedAsync;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public FansViewModel ViewModel
        {
            get { return (FansViewModel)GetValue(ViewModelProperty); }
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

        private async void OnFansRefreshButtonClickAsync(object sender, RoutedEventArgs e)
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

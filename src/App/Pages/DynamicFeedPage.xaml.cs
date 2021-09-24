// Copyright (c) GodLeaveMe. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.App.Controls;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页.
    /// </summary>
    public sealed partial class DynamicFeedPage : Page, IRefreshPage
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(DynamicModuleViewModel), typeof(DynamicFeedPage), new PropertyMetadata(DynamicModuleViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicFeedPage"/> class.
        /// </summary>
        public DynamicFeedPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoadedAsync;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public DynamicModuleViewModel ViewModel
        {
            get { return (DynamicModuleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <inheritdoc/>
        public Task RefreshAsync()
            => ViewModel.InitializeRequestAsync();

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsRequested)
            {
                await ViewModel.InitializeRequestAsync();
            }
        }

        private async void OnViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            await ViewModel.RequestDataAsync();
        }

        private async void OnDynamicRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await RefreshAsync();
        }

        private async void OnLoginButtonClickAsync(object sender, RoutedEventArgs e)
        {
            LoginButton.IsEnabled = false;
            await AccountViewModel.Instance.TrySignInAsync();
            if (LoginButton != null)
            {
                LoginButton.IsEnabled = true;
            }
        }
    }
}

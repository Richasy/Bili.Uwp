// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using System.Threading.Tasks;
using Richasy.Bili.App.Controls;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页.
    /// </summary>
    public sealed partial class DynamicFeedPage : AppPage, IRefreshPage
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
            InitializeComponent();
            Loaded += OnLoadedAsync;
            Unloaded += OnUnloaded;
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
            ViewModel.PropertyChanged += OnViewModelPropertyChangedAsync;
            if (!ViewModel.IsRequested)
            {
                await ViewModel.InitializeRequestAsync();
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
            => ViewModel.PropertyChanged -= OnViewModelPropertyChangedAsync;

        private async void OnViewModelPropertyChangedAsync(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.IsVideo))
            {
                if (ViewModel.IsShowLogin)
                {
                    return;
                }

                if ((ViewModel.IsVideo && ViewModel.VideoDynamicCollection.Count == 0)
                    || (!ViewModel.IsVideo && ViewModel.AllDynamicCollection.Count == 0))
                {
                    await ViewModel.InitializeRequestAsync();
                }
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

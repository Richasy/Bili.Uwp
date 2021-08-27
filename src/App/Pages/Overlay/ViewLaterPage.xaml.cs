// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Richasy.Bili.App.Pages.Overlay
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页.
    /// </summary>
    public sealed partial class ViewLaterPage : Page
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(ViewLaterViewModel), typeof(ViewLaterPage), new PropertyMetadata(ViewLaterViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewLaterPage"/> class.
        /// </summary>
        public ViewLaterPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoadedAsync;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public ViewLaterViewModel ViewModel
        {
            get { return (ViewLaterViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeRequestAsync();
        }

        private async void OnClearButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var isClear = false;
            if (ViewModel.VideoCollection.Count > 0)
            {
                // Show dialog.
            }

            if (isClear)
            {
                await ViewModel.ClearAsync();
            }
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.IsRequested)
            {
                await ViewModel.InitializeRequestAsync();
            }
        }

        private async void OnVideoViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            await ViewModel.RequestDataAsync();
        }

        private async void OnRemoveItemClickAsync(object sender, RoutedEventArgs e)
        {
            var vm = (sender as FrameworkElement).DataContext as VideoViewModel;
            await ViewModel.RemoveAsync(vm);
        }
    }
}

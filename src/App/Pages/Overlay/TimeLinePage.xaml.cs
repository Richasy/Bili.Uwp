// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Richasy.Bili.App.Pages.Overlay
{
    /// <summary>
    /// 时间线页面.
    /// </summary>
    public sealed partial class TimeLinePage : Page
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(AnimeViewModelBase), typeof(TimeLinePage), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeLinePage"/> class.
        /// </summary>
        public TimeLinePage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoadedAsync;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public AnimeViewModelBase ViewModel
        {
            get { return (AnimeViewModelBase)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is AnimeViewModelBase vm)
            {
                ViewModel = vm;
            }

            base.OnNavigatedTo(e);
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (ViewModel.FilterCollection.Count == 0)
            {
                await ViewModel.InitializeTimeLineAsync();
            }
        }

        private async void OnTimeLineRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeTimeLineAsync();
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.App.Controls;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Richasy.Bili.App.Pages.Overlay
{
    /// <summary>
    /// 播放器页面.
    /// </summary>
    public sealed partial class PlayerPage : Page
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(PlayerViewModel), typeof(PlayerPage), new PropertyMetadata(PlayerViewModel.Instance));

        private VideoViewModel _navigateVM;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPage"/> class.
        /// </summary>
        public PlayerPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            this.Loaded += OnLoadedAsync;
            this.Unloaded += OnUnloadedAsync;
            this.SizeChanged += OnSizeChanged;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public PlayerViewModel ViewModel
        {
            get { return (PlayerViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null && e.Parameter is VideoViewModel vm)
            {
                _navigateVM = vm;
            }
        }

        /// <inheritdoc/>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navigateVM = null;
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (_navigateVM != null)
            {
                await ViewModel.LoadAsync(_navigateVM);
            }
        }

        private void OnUnloadedAsync(object sender, RoutedEventArgs e)
        {
            ViewModel.ClearPlayer();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < AppViewModel.Instance.MediumWindowThresholdWidth)
            {
                if (RootGrid.Children.Contains(ContentGrid))
                {
                    this.RootGrid.Children.Remove(ContentGrid);
                    RootScrollViewer.Content = ContentGrid;
                }
            }
            else
            {
                if (RootScrollViewer.Content != null)
                {
                    RootScrollViewer.Content = null;
                    this.RootGrid.Children.Insert(0, ContentGrid);
                }
            }
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            if (_navigateVM != null)
            {
                await ViewModel.LoadAsync(_navigateVM);
            }
        }
    }
}

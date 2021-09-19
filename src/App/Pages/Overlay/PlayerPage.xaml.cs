// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.ViewManagement;
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

        private object _navigateVM;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPage"/> class.
        /// </summary>
        public PlayerPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            ViewModel.Dispatcher = Dispatcher;
            Loaded += OnLoadedAsync;
            Unloaded += OnUnloaded;
            SizeChanged += OnSizeChanged;
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
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null)
            {
                if (e.Parameter is VideoViewModel videoVM)
                {
                    _navigateVM = videoVM;
                }
                else if (e.Parameter is SeasonViewModel ssVM)
                {
                    _navigateVM = ssVM;
                }

                if (IsLoaded)
                {
                    await ViewModel.LoadAsync(_navigateVM);
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            _navigateVM = null;
            EnterDefaultModeAsync();
            AppViewModel.Instance.IsOverLayerExtendToTitleBar = false;
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
            if (_navigateVM != null)
            {
                await ViewModel.LoadAsync(_navigateVM);
            }

            CheckPlayerDisplayModeAsync();
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.PlayerDisplayMode):
                    CheckPlayerDisplayModeAsync();
                    break;
                case nameof(ViewModel.IsDetailCanLoaded):
                    FindName("ContentGrid");
                    break;
                default:
                    break;
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
            ViewModel.ClearPlayer();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width < AppViewModel.Instance.MediumWindowThresholdWidth)
            {
                if (ContentContainer.Children.Contains(ContentGrid))
                {
                    ContentContainer.Children.Remove(ContentGrid);
                    RootScrollViewer.Content = ContentGrid;
                }

                if (BiliPlayer != null)
                {
                    var maxHeight = e.NewSize.Height * 0.7;
                    BiliPlayer.MaxHeight = maxHeight;
                }
            }
            else
            {
                if (RootScrollViewer.Content != null)
                {
                    RootScrollViewer.Content = null;
                    ContentContainer.Children.Insert(0, ContentGrid);
                }

                if (BiliPlayer != null)
                {
                    BiliPlayer.MaxHeight = double.PositiveInfinity;
                }
            }
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            if (_navigateVM != null)
            {
                await ViewModel.LoadAsync(_navigateVM, true);
            }
        }

        private async void CheckPlayerDisplayModeAsync()
        {
            var appView = ApplicationView.GetForCurrentView();

            if (ViewModel.PlayerDisplayMode == PlayerDisplayMode.Default)
            {
                VisualStateManager.GoToState(this, nameof(DefaultModeState), false);
                EnterDefaultModeAsync();
                AppViewModel.Instance.IsOverLayerExtendToTitleBar = false;
            }
            else
            {
                VisualStateManager.GoToState(this, nameof(FullModeState), false);
                AppViewModel.Instance.IsOverLayerExtendToTitleBar = true;
                if (ViewModel.PlayerDisplayMode == PlayerDisplayMode.FullScreen)
                {
                    appView.TryEnterFullScreenMode();
                }
                else if (ViewModel.PlayerDisplayMode == PlayerDisplayMode.CompactOverlay)
                {
                    await appView.TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay);
                }
                else
                {
                    EnterDefaultModeAsync();
                }
            }

            CheckPlayerVisual();
        }

        private async void EnterDefaultModeAsync()
        {
            var appView = ApplicationView.GetForCurrentView();

            if (appView.ViewMode == ApplicationViewMode.CompactOverlay)
            {
                await appView.TryEnterViewModeAsync(ApplicationViewMode.Default);
            }
            else if (appView.IsFullScreenMode)
            {
                appView.ExitFullScreenMode();
            }
        }

        private void CheckPlayerVisual()
        {
            if (ViewModel.PlayerDisplayMode == PlayerDisplayMode.Default)
            {
                if (RootGrid.Children.Count > 1)
                {
                    RootGrid.Children.Remove(BiliPlayer);
                    PlayerContainer.Children.Add(BiliPlayer);
                }
            }
            else
            {
                if (PlayerContainer.Children.Count > 0)
                {
                    PlayerContainer.Children.Remove(BiliPlayer);
                    RootGrid.Children.Add(BiliPlayer);
                }
            }
        }
    }
}

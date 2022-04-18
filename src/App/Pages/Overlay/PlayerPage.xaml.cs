// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.ViewModels.Uwp;
using Richasy.Bili.ViewModels.Uwp.Common;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace Richasy.Bili.App.Pages.Overlay
{
    /// <summary>
    /// 播放器页面.
    /// </summary>
    public sealed partial class PlayerPage : AppPage
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
                _navigateVM = e.Parameter;

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
            if (ViewModel.IsShowViewLater)
            {
                foreach (var item in ViewModel.ViewLaterVideoCollection)
                {
                    item.IsSelected = false;
                }
            }

            DanmakuViewModel.Instance.Reset();
            CoreViewModel.IsOverLayerExtendToTitleBar = false;
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
            if (_navigateVM != null)
            {
                await ViewModel.LoadAsync(_navigateVM);
            }

            CheckPlayerDisplayModeAsync();

            if (ViewModel.IsDetailCanLoaded)
            {
                CheckLayout();
            }
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(ViewModel.PlayerDisplayMode):
                    CheckPlayerDisplayModeAsync();
                    break;
                case nameof(ViewModel.IsDetailCanLoaded):
                    if (ViewModel.IsDetailCanLoaded)
                    {
                        CheckLayout();
                    }

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
            if (!ViewModel.IsDetailCanLoaded)
            {
                return;
            }

            CheckLayout();
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
                EnterDefaultModeAsync();
                CoreViewModel.IsOverLayerExtendToTitleBar = false;
            }
            else
            {
                CoreViewModel.IsOverLayerExtendToTitleBar = true;
                ViewModel.BiliPlayer.IsFullWindow = true;
                if (ViewModel.PlayerDisplayMode == PlayerDisplayMode.FullScreen)
                {
                    if (appView.TryEnterFullScreenMode())
                    {
                        VisualStateManager.GoToState(this, nameof(FullPlayerState), false);
                    }
                }
                else if (ViewModel.PlayerDisplayMode == PlayerDisplayMode.FullWindow)
                {
                    if (appView.IsFullScreenMode)
                    {
                        appView.ExitFullScreenMode();
                    }

                    VisualStateManager.GoToState(this, nameof(FullPlayerState), false);
                }
                else if (ViewModel.PlayerDisplayMode == PlayerDisplayMode.CompactOverlay)
                {
                    if (await appView.TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay).AsTask())
                    {
                        VisualStateManager.GoToState(this, nameof(FullPlayerState), false);
                    }
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
            VisualStateManager.GoToState(this, nameof(StandardPlayerState), false);

            if (ViewModel.BiliPlayer != null)
            {
                ViewModel.BiliPlayer.IsFullWindow = false;
            }

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
                if (RootGrid.Children.Contains(PlayerContainer))
                {
                    RootGrid.Children.Remove(PlayerContainer);
                    ContentGrid.Children.Insert(0, PlayerContainer);
                }
            }
            else
            {
                if (ContentGrid.Children.Contains(PlayerContainer))
                {
                    ContentGrid.Children.Remove(PlayerContainer);
                    RootGrid.Children.Add(PlayerContainer);
                }
            }
        }

        private void CheckLayout()
        {
            var width = Window.Current.Bounds.Width;
            var height = Window.Current.Bounds.Height;
            if (width < CoreViewModel.MediumWindowThresholdWidth)
            {
                if (LayoutGroup.CurrentState != NarrowState)
                {
                    VisualStateManager.GoToState(this, nameof(NarrowState), false);
                }

                if (LoadContainer.Children.Contains(ContentGrid))
                {
                    LoadContainer.Children.Remove(ContentGrid);
                    ScrollGrid.Children.Add(ContentGrid);
                }

                if (BiliPlayer != null)
                {
                    if (ViewModel.PlayerDisplayMode == PlayerDisplayMode.Default)
                    {
                        var maxHeight = height * 0.7;
                        BiliPlayer.MaxHeight = maxHeight;
                    }
                    else
                    {
                        BiliPlayer.MaxHeight = double.PositiveInfinity;
                    }
                }
            }
            else
            {
                if (ScrollGrid.Children.Count > 0)
                {
                    ScrollGrid.Children.Clear();
                    LoadContainer.Children.Insert(0, ContentGrid);
                }

                if (BiliPlayer != null)
                {
                    BiliPlayer.MaxHeight = double.PositiveInfinity;
                }
            }
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Models.App.Constants;
using Bili.Models.App.Other;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Markup;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// 播放页面面板.
    /// </summary>
    [ContentProperty(Name = "Player")]
    public sealed class PlayerPagePanel : ReactiveControl<IPlayerPageViewModel>
    {
        /// <summary>
        /// <see cref="Player"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty PlayerProperty =
            DependencyProperty.Register(nameof(Player), typeof(object), typeof(PlayerPagePanel), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="Dashboard"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DashboardProperty =
            DependencyProperty.Register(nameof(Dashboard), typeof(object), typeof(PlayerPagePanel), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="SectionHeaderItemsSource"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty SectionHeaderItemsSourceProperty =
            DependencyProperty.Register(nameof(SectionHeaderItemsSource), typeof(object), typeof(PlayerPagePanel), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="SectionContent"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty SectionContentProperty =
            DependencyProperty.Register(nameof(SectionContent), typeof(object), typeof(PlayerPagePanel), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="SectionHeaderSelectedItem"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty SectionHeaderSelectedItemProperty =
            DependencyProperty.Register(nameof(SectionHeaderSelectedItem), typeof(object), typeof(PlayerPagePanel), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="Descriptor"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty DescriptorProperty =
            DependencyProperty.Register(nameof(Descriptor), typeof(object), typeof(PlayerPagePanel), new PropertyMetadata(default));

        private readonly double _mediumWindowWidth;
        private SplitView _splitView;
        private ButtonBase _expandButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPagePanel"/> class.
        /// </summary>
        public PlayerPagePanel()
        {
            DefaultStyleKey = typeof(PlayerPagePanel);
            _mediumWindowWidth = Locator.Instance.GetService<IResourceToolkit>().GetResource<double>(AppConstants.MediumWindowThresholdWidthKey);
            SizeChanged += OnSizeChanged;
            Loaded += OnLoadedAsync;
            Unloaded += OnUnloaded;
        }

        /// <summary>
        /// 区块标头被点击.
        /// </summary>
        public event EventHandler<PlayerSectionHeader> SectionHeaderItemInvoked;

        /// <summary>
        /// 播放器组件.
        /// </summary>
        public object Player
        {
            get { return (object)GetValue(PlayerProperty); }
            set { SetValue(PlayerProperty, value); }
        }

        /// <summary>
        /// 信息面板组件.
        /// </summary>
        public object Dashboard
        {
            get { return (object)GetValue(DashboardProperty); }
            set { SetValue(DashboardProperty, value); }
        }

        /// <summary>
        /// 描述组件.
        /// </summary>
        public object Descriptor
        {
            get { return (object)GetValue(DescriptorProperty); }
            set { SetValue(DescriptorProperty, value); }
        }

        /// <summary>
        /// 侧面板的头部数据源.
        /// </summary>
        public object SectionHeaderItemsSource
        {
            get { return (object)GetValue(SectionHeaderItemsSourceProperty); }
            set { SetValue(SectionHeaderItemsSourceProperty, value); }
        }

        /// <summary>
        /// 侧面板头部选中条目.
        /// </summary>
        public object SectionHeaderSelectedItem
        {
            get { return (object)GetValue(SectionHeaderSelectedItemProperty); }
            set { SetValue(SectionHeaderSelectedItemProperty, value); }
        }

        /// <summary>
        /// 侧面板的内容.
        /// </summary>
        public object SectionContent
        {
            get { return (object)GetValue(SectionContentProperty); }
            set { SetValue(SectionContentProperty, value); }
        }

        internal override void OnViewModelChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is IPlayerPageViewModel oldVM)
            {
                oldVM.MediaPlayerViewModel.PropertyChanged -= OnViewModelPropertyChangedAsync;
                oldVM.MediaPlayerViewModel.MediaPlayerChanged -= OnMediaPlayerChangedAsync;
            }

            if (e.NewValue is IPlayerPageViewModel vm)
            {
                vm.MediaPlayerViewModel.PropertyChanged -= OnViewModelPropertyChangedAsync;
                vm.MediaPlayerViewModel.PropertyChanged += OnViewModelPropertyChangedAsync;
                vm.MediaPlayerViewModel.MediaPlayerChanged += OnMediaPlayerChangedAsync;
            }
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _splitView = GetTemplateChild("RootView") as SplitView;
            _expandButton = GetTemplateChild("ExpandButton") as ButtonBase;
            var sectionView = GetTemplateChild("SectionNavigationView") as Microsoft.UI.Xaml.Controls.NavigationView;

            _expandButton.Click += OnExpandButtonClick;
            sectionView.ItemInvoked += OnSectionViewItemInvoked;
        }

        private async void OnViewModelPropertyChangedAsync(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.MediaPlayerViewModel.DisplayMode))
            {
                await ChangeVisualStateFromDisplayModeAsync();
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (ViewModel.MediaPlayerViewModel.DisplayMode != Models.Enums.PlayerDisplayMode.Default)
            {
                return;
            }

            var width = Window.Current.Bounds.Width;
            if (width >= _mediumWindowWidth)
            {
                VisualStateManager.GoToState(this, "NormalState", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "NarrowState", false);
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (ViewModel?.MediaPlayerViewModel != null)
            {
                ViewModel.MediaPlayerViewModel.PropertyChanged -= OnViewModelPropertyChangedAsync;
                ViewModel.MediaPlayerViewModel.MediaPlayerChanged -= OnMediaPlayerChangedAsync;
            }
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
            => await ChangeVisualStateFromDisplayModeAsync();

        private async void OnMediaPlayerChangedAsync(object sender, object e)
            => await ChangeVisualStateFromDisplayModeAsync();

        private async Task ChangeVisualStateFromDisplayModeAsync()
        {
            var mediaVM = ViewModel?.MediaPlayerViewModel;
            if (mediaVM == null)
            {
                return;
            }

            var appView = ApplicationView.GetForCurrentView();
            if (mediaVM.DisplayMode == Models.Enums.PlayerDisplayMode.Default)
            {
                ToggleFullPlayerState(false);
                if (appView.IsFullScreenMode)
                {
                    appView.ExitFullScreenMode();
                }
                else if (appView.ViewMode != ApplicationViewMode.Default)
                {
                    await appView.TryEnterViewModeAsync(ApplicationViewMode.Default).AsTask();
                }
            }
            else if (mediaVM.DisplayMode == Models.Enums.PlayerDisplayMode.FullWindow)
            {
                ToggleFullPlayerState(true);
                if (appView.IsFullScreenMode)
                {
                    appView.ExitFullScreenMode();
                }
            }
            else if (mediaVM.DisplayMode == Models.Enums.PlayerDisplayMode.FullScreen)
            {
                ToggleFullPlayerState(true);
                if (!appView.IsFullScreenMode)
                {
                    appView.TryEnterFullScreenMode();
                }
            }
            else if (mediaVM.DisplayMode == Models.Enums.PlayerDisplayMode.CompactOverlay)
            {
                ToggleFullPlayerState(true);
                if (appView.ViewMode != ApplicationViewMode.CompactOverlay)
                {
                    await appView.TryEnterViewModeAsync(ApplicationViewMode.CompactOverlay).AsTask();
                }
            }
        }

        /// <summary>
        /// 切换全视窗播放器状态.
        /// </summary>
        /// <param name="isFullPlayer">是否将播放器扩展到全视窗.</param>
        private void ToggleFullPlayerState(bool isFullPlayer)
        {
            if (isFullPlayer)
            {
                VisualStateManager.GoToState(this, "FullPlayerState", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "StandardState", false);
            }
        }

        private void OnExpandButtonClick(object sender, RoutedEventArgs e)
        {
            _splitView.IsPaneOpen = true;
            if (_expandButton is ToggleButton btn)
            {
                btn.IsChecked = false;
            }
        }

        private void OnSectionViewItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            var item = args.InvokedItem as PlayerSectionHeader;
            SectionHeaderItemInvoked?.Invoke(this, item);
        }
    }
}

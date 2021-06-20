// Copyright (c) Richasy. All rights reserved.

using System;
using Microsoft.UI.Xaml.Controls;
using Richasy.Bili.Models.BiliBili;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 横幅视图.
    /// </summary>
    public sealed partial class BannerView : UserControl
    {
        /// <summary>
        /// 数据源的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(BannerView), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerView"/> class.
        /// </summary>
        public BannerView()
        {
            this.InitializeComponent();
            this.SizeChanged += OnSizeChanged;
            this.Loaded += OnLoaded;
        }

        /// <summary>
        /// 横幅数据源.
        /// </summary>
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <inheritdoc/>
        protected override void OnPointerEntered(PointerRoutedEventArgs e) => CheckOffsetButtonStatus();

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            CheckOffsetButtonStatus();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CheckOffsetButtonStatus();
        }

        private void CheckOffsetButtonStatus()
        {
            if (WideScrollViewer.ExtentWidth <= WideScrollViewer.ViewportWidth)
            {
                LeftOffsetButton.Visibility = RightOffsetButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                LeftOffsetButton.Visibility = WideScrollViewer.HorizontalOffset == 0 ? Visibility.Collapsed : Visibility.Visible;
                RightOffsetButton.Visibility = WideScrollViewer.ScrollableWidth - WideScrollViewer.HorizontalOffset > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void OnLeftOffsetButtonClick(object sender, RoutedEventArgs e)
        {
            var leftOffset = WideScrollViewer.HorizontalOffset - WideScrollViewer.ViewportWidth;
            if (leftOffset < 0)
            {
                leftOffset = 0;
            }

            var options = new ScrollingScrollOptions(ScrollingAnimationMode.Enabled, ScrollingSnapPointsMode.Ignore);
            WideScrollViewer.ChangeView(leftOffset, 0, 1);
        }

        private void OnRightOffsetButtonClick(object sender, RoutedEventArgs e)
        {
            var rightOffset = WideScrollViewer.HorizontalOffset + WideScrollViewer.ViewportWidth;
            if (rightOffset > WideScrollViewer.ExtentWidth)
            {
                rightOffset = WideScrollViewer.ScrollableWidth - WideScrollViewer.HorizontalOffset;
            }

            var options = new ScrollingScrollOptions(ScrollingAnimationMode.Auto, ScrollingSnapPointsMode.Ignore);
            WideScrollViewer.ChangeView(rightOffset, 0, 1);
        }

        private async void OnBannerTappedAsync(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var context = (sender as FrameworkElement).DataContext as Banner;
            await Launcher.LaunchUriAsync(new System.Uri(context.NavigateUri));
        }

        private void OnBannerPointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var control = sender as Control;
            control.CapturePointer(e.Pointer);
            VisualStateManager.GoToState(control, "PressedState", true);
        }

        private void OnBannerPointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            var control = sender as Control;
            VisualStateManager.GoToState(sender as Control, "NormalState", true);
            control.ReleasePointerCapture(e.Pointer);
        }

        private void OnWideScrollViewerChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            CheckOffsetButtonStatus();
        }
    }
}

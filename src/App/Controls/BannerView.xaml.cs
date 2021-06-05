// Copyright (c) Richasy. All rights reserved.

using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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
        }

        /// <summary>
        /// 横幅数据源.
        /// </summary>
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            CheckOffsetButtonStatus();
        }

        private void CheckOffsetButtonStatus()
        {
            if (WideScrollView.ExtentWidth <= WideScrollView.ViewportWidth)
            {
                LeftOffsetButton.Visibility = RightOffsetButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                LeftOffsetButton.Visibility = WideScrollView.HorizontalOffset == 0 ? Visibility.Collapsed : Visibility.Visible;
                RightOffsetButton.Visibility = WideScrollView.ScrollableWidth - WideScrollView.HorizontalOffset > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void OnLeftOffsetButtonClick(object sender, RoutedEventArgs e)
        {
            var leftOffset = WideScrollView.HorizontalOffset - WideScrollView.ViewportWidth;
            if (leftOffset < 0)
            {
                leftOffset = 0;
            }

            var options = new ScrollingScrollOptions(ScrollingAnimationMode.Enabled, ScrollingSnapPointsMode.Ignore);
            WideScrollView.ScrollTo(leftOffset, 0, options);
        }

        private void OnRightOffsetButtonClick(object sender, RoutedEventArgs e)
        {
            var rightOffset = WideScrollView.HorizontalOffset + WideScrollView.ViewportWidth;
            if (rightOffset > WideScrollView.ExtentWidth)
            {
                rightOffset = WideScrollView.ScrollableWidth;
            }

            var options = new ScrollingScrollOptions(ScrollingAnimationMode.Auto, ScrollingSnapPointsMode.Ignore);
            WideScrollView.ScrollTo(rightOffset, 0, options);
        }

        private void OnWideScrollViewChanged(ScrollView sender, object args)
        {
            CheckOffsetButtonStatus();
        }
    }
}

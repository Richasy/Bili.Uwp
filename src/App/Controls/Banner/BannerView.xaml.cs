// Copyright (c) Richasy. All rights reserved.

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

            WideScrollViewer.ChangeView(leftOffset, 0, 1);
        }

        private void OnRightOffsetButtonClick(object sender, RoutedEventArgs e)
        {
            var rightOffset = WideScrollViewer.HorizontalOffset + WideScrollViewer.ViewportWidth;
            if (rightOffset > WideScrollViewer.ExtentWidth)
            {
                rightOffset = WideScrollViewer.ScrollableWidth - WideScrollViewer.HorizontalOffset;
            }

            WideScrollViewer.ChangeView(rightOffset, 0, 1);
        }

        private void OnWideScrollViewerChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            CheckOffsetButtonStatus();
        }
    }
}

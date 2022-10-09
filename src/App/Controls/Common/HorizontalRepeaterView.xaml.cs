// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.Toolkit.Interfaces;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Bili.App.Controls
{
    /// <summary>
    /// 水平列表视图.
    /// </summary>
    public sealed partial class HorizontalRepeaterView : UserControl
    {
        /// <summary>
        /// <see cref="ItemsSource"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(HorizontalRepeaterView), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="WideItemTemplate"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty WideItemTemplateProperty =
            DependencyProperty.Register(nameof(WideItemTemplate), typeof(DataTemplate), typeof(HorizontalRepeaterView), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="NarrowItemTemplate"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty NarrowItemTemplateProperty =
            DependencyProperty.Register(nameof(NarrowItemTemplate), typeof(DataTemplate), typeof(HorizontalRepeaterView), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="Header"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(string), typeof(HorizontalRepeaterView), new PropertyMetadata(string.Empty));

        /// <summary>
        /// <see cref="HeaderVisibility"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty HeaderVisibilityProperty =
            DependencyProperty.Register(nameof(HeaderVisibility), typeof(Visibility), typeof(HorizontalRepeaterView), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// <see cref="NarrowHeight"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty NarrowHeightProperty =
            DependencyProperty.Register(nameof(NarrowHeight), typeof(double), typeof(HorizontalRepeaterView), new PropertyMetadata(128d));

        /// <summary>
        /// <see cref="AdditionalContent"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty AdditionalContentProperty =
            DependencyProperty.Register(nameof(AdditionalContent), typeof(object), typeof(HorizontalRepeaterView), new PropertyMetadata(null));

        private const string MediumWindowWidthKey = "MediumWindowThresholdWidth";
        private readonly IResourceToolkit _resourceToolkit;

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerView"/> class.
        /// </summary>
        public HorizontalRepeaterView()
        {
            InitializeComponent();
            _resourceToolkit = Locator.Instance.GetService<IResourceToolkit>();
            SizeChanged += OnSizeChanged;
            Loaded += OnLoaded;
        }

        /// <summary>
        /// 数据源.
        /// </summary>
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// 宽视图下的条目模板.
        /// </summary>
        public DataTemplate WideItemTemplate
        {
            get { return (DataTemplate)GetValue(WideItemTemplateProperty); }
            set { SetValue(WideItemTemplateProperty, value); }
        }

        /// <summary>
        /// 窄视图下的条目模板.
        /// </summary>
        public DataTemplate NarrowItemTemplate
        {
            get { return (DataTemplate)GetValue(NarrowItemTemplateProperty); }
            set { SetValue(NarrowItemTemplateProperty, value); }
        }

        /// <summary>
        /// 标题.
        /// </summary>
        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// 标题可见性.
        /// </summary>
        public Visibility HeaderVisibility
        {
            get { return (Visibility)GetValue(HeaderVisibilityProperty); }
            set { SetValue(HeaderVisibilityProperty, value); }
        }

        /// <summary>
        /// 窄视图下的高度.
        /// </summary>
        public double NarrowHeight
        {
            get { return (double)GetValue(NarrowHeightProperty); }
            set { SetValue(NarrowHeightProperty, value); }
        }

        /// <summary>
        /// 顶部附加内容.
        /// </summary>
        public object AdditionalContent
        {
            get { return (object)GetValue(AdditionalContentProperty); }
            set { SetValue(AdditionalContentProperty, value); }
        }

        /// <inheritdoc/>
        protected override void OnPointerEntered(PointerRoutedEventArgs e) => CheckOffsetButtonStatus();

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            CheckOffsetButtonStatus();
            CheckLayout();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CheckOffsetButtonStatus();
            CheckLayout();
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

        private void CheckLayout()
        {
            var windowWidth = Window.Current.Bounds.Width;

            if (windowWidth < _resourceToolkit.GetResource<double>(MediumWindowWidthKey) && NarrowItemTemplate != null)
            {
                WideContainer.Visibility = Visibility.Collapsed;
                NarrowContainer.Visibility = Visibility.Visible;
            }
            else
            {
                WideContainer.Visibility = Visibility.Visible;
                NarrowContainer.Visibility = Visibility.Collapsed;
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

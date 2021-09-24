// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Collections;
using Microsoft.UI.Xaml.Controls;
using Richasy.Bili.App.Resources.Extension;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 视频视图.
    /// </summary>
    public sealed partial class VerticalRepeaterView : UserControl
    {
        /// <summary>
        /// <see cref="ItemsSource"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(VerticalRepeaterView), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="ItemOrientation"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ItemOrientationProperty =
            DependencyProperty.Register(nameof(ItemOrientation), typeof(Orientation), typeof(VerticalRepeaterView), new PropertyMetadata(default(Orientation), new PropertyChangedCallback(OnOrientationChanged)));

        /// <summary>
        /// <see cref="HeaderText"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(VerticalRepeaterView), new PropertyMetadata(string.Empty));

        /// <summary>
        /// <see cref="ItemTemplate"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(VerticalRepeaterView), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="AdditionalContent"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty AdditionalContentProperty =
            DependencyProperty.Register(nameof(AdditionalContent), typeof(object), typeof(VerticalRepeaterView), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="IsAutoFillEnable"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsAutoFillEnableProperty =
            DependencyProperty.Register(nameof(IsAutoFillEnable), typeof(bool), typeof(VerticalRepeaterView), new PropertyMetadata(true));

        /// <summary>
        /// <see cref="HeaderVisibility"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty HeaderVisibilityProperty =
            DependencyProperty.Register(nameof(HeaderVisibility), typeof(Visibility), typeof(VerticalRepeaterView), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// <see cref="MinWideItemHeight"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty MinWideItemHeightProperty =
            DependencyProperty.Register(nameof(MinWideItemHeight), typeof(double), typeof(VerticalRepeaterView), new PropertyMetadata(232d));

        /// <summary>
        /// <see cref="MinWideItemWidth"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty MinWideItemWidthProperty =
            DependencyProperty.Register(nameof(MinWideItemWidth), typeof(double), typeof(VerticalRepeaterView), new PropertyMetadata(222d));

        /// <summary>
        /// <see cref="EnableDetectParentScrollViewer"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty EnableDetectParentScrollViewerProperty =
            DependencyProperty.Register(nameof(EnableDetectParentScrollViewer), typeof(bool), typeof(VerticalRepeaterView), new PropertyMetadata(true));

        /// <summary>
        /// <see cref="IsStaggered"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsStaggeredProperty =
            DependencyProperty.Register(nameof(IsStaggered), typeof(bool), typeof(VerticalRepeaterView), new PropertyMetadata(false, new PropertyChangedCallback(OnIsStaggeredChanged)));

        private ScrollViewer _parentScrollViewer;
        private double _itemHolderHeight = 0d;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerticalRepeaterView"/> class.
        /// </summary>
        public VerticalRepeaterView()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        /// <summary>
        /// 在外部的ScrollViewer滚动到接近底部时发生.
        /// </summary>
        public event EventHandler RequestLoadMore;

        /// <summary>
        /// 条目模板.
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
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
        /// 子项的布局方式.
        /// </summary>
        public Orientation ItemOrientation
        {
            get { return (Orientation)GetValue(ItemOrientationProperty); }
            set { SetValue(ItemOrientationProperty, value); }
        }

        /// <summary>
        /// 标题文本.
        /// </summary>
        public string HeaderText
        {
            get { return (string)GetValue(HeaderTextProperty); }
            set { SetValue(HeaderTextProperty, value); }
        }

        /// <summary>
        /// 附加视觉元素，在顶部右上角.
        /// </summary>
        public object AdditionalContent
        {
            get { return (object)GetValue(AdditionalContentProperty); }
            set { SetValue(AdditionalContentProperty, value); }
        }

        /// <summary>
        /// 是否允许根据容器剩余空间自行计算视频条目容量，并主动发起请求填满整个容器.
        /// </summary>
        public bool IsAutoFillEnable
        {
            get { return (bool)GetValue(IsAutoFillEnableProperty); }
            set { SetValue(IsAutoFillEnableProperty, value); }
        }

        /// <summary>
        /// 标题的可见性.
        /// </summary>
        public Visibility HeaderVisibility
        {
            get { return (Visibility)GetValue(HeaderVisibilityProperty); }
            set { SetValue(HeaderVisibilityProperty, value); }
        }

        /// <summary>
        /// 在网格布局下，单个条目的最小宽度.
        /// </summary>
        public double MinWideItemWidth
        {
            get { return (double)GetValue(MinWideItemWidthProperty); }
            set { SetValue(MinWideItemWidthProperty, value); }
        }

        /// <summary>
        /// 在网格布局下，单个条目的最小高度.
        /// </summary>
        public double MinWideItemHeight
        {
            get { return (double)GetValue(MinWideItemHeightProperty); }
            set { SetValue(MinWideItemHeightProperty, value); }
        }

        /// <summary>
        /// 是否启用自动检测父滚动视图.
        /// </summary>
        public bool EnableDetectParentScrollViewer
        {
            get { return (bool)GetValue(EnableDetectParentScrollViewerProperty); }
            set { SetValue(EnableDetectParentScrollViewerProperty, value); }
        }

        /// <summary>
        /// 是否为流式布局.
        /// </summary>
        public bool IsStaggered
        {
            get { return (bool)GetValue(IsStaggeredProperty); }
            set { SetValue(IsStaggeredProperty, value); }
        }

        private static void OnIsStaggeredChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as VerticalRepeaterView;
            instance.CheckOrientationStatus();
        }

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as VerticalRepeaterView;
            if (e.NewValue is Orientation type)
            {
                instance.CheckOrientationStatus();
            }
        }

        private void CheckOrientationStatus()
        {
            if (IsStaggered)
            {
                ItemsRepeater.Layout = StaggeredLayout;
            }
            else
            {
                switch (ItemOrientation)
                {
                    case Orientation.Vertical:
                        ItemsRepeater.Layout = GridLayout;
                        break;
                    case Orientation.Horizontal:
                        ItemsRepeater.Layout = ListLayout;
                        break;
                    default:
                        break;
                }
            }

            ChangeInitializedItemOrientation();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            if (EnableDetectParentScrollViewer)
            {
                _parentScrollViewer = this.FindAscendantElementByType<ScrollViewer>();
                if (_parentScrollViewer != null)
                {
                    _parentScrollViewer.ViewChanged += OnParentScrollViewerViewChanged;
                }
            }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            if (_parentScrollViewer != null)
            {
                _parentScrollViewer.ViewChanged -= OnParentScrollViewerViewChanged;
                _parentScrollViewer = null;
            }
        }

        private void OnParentScrollViewerViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            if (!e.IsIntermediate && _parentScrollViewer != null)
            {
                var currentPosition = _parentScrollViewer.VerticalOffset;
                if (_parentScrollViewer.ScrollableHeight - currentPosition <= _itemHolderHeight &&
                    this.Visibility == Visibility.Visible)
                {
                    RequestLoadMore?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void ChangeInitializedItemOrientation()
        {
            if (ItemsSource is ICollection items)
            {
                for (var i = 0; i < items.Count; i++)
                {
                    var element = ItemsRepeater.TryGetElement(i);
                    if (element != null && element is IDynamicLayoutItem vi)
                    {
                        if (vi.Orientation != ItemOrientation)
                        {
                            vi.Orientation = ItemOrientation;
                        }
                    }
                }
            }
        }

        private void OnElementPrepared(ItemsRepeater sender, ItemsRepeaterElementPreparedEventArgs args)
        {
            if (args.Element != null)
            {
                if (args.Element is IDynamicLayoutItem dynamicLayoutItem)
                {
                    dynamicLayoutItem.Orientation = ItemOrientation;
                }

                if (IsAutoFillEnable &&
                    args.Element is IRepeaterItem repeaterItem &&
                    ItemsSource is ICollection collectionSource &&
                    (_parentScrollViewer != null) &&
                    args.Index >= collectionSource.Count - 1)
                {
                    var size = repeaterItem.GetHolderSize();
                    _itemHolderHeight = size.Height;
                    var viewportWidth = _parentScrollViewer.ViewportWidth;
                    var viewportHeight = _parentScrollViewer.ViewportHeight;
                    bool isNeedLoadMore;
                    if (double.IsInfinity(size.Width))
                    {
                        isNeedLoadMore = (args.Index + 1) * size.Height <= viewportHeight;
                    }
                    else
                    {
                        var rowCount = args.Index / (viewportWidth / size.Width);
                        isNeedLoadMore = rowCount * size.Height <= viewportHeight;
                    }

                    if (isNeedLoadMore)
                    {
                        RequestLoadMore?.Invoke(this, EventArgs.Empty);
                    }
                }
            }
        }
    }
}

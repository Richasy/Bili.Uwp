// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Richasy.Bili.App.Resources.Extension;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 视频视图.
    /// </summary>
    public sealed partial class VideoView : UserControl
    {
        /// <summary>
        /// <see cref="ItemsSource"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(VideoView), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="ItemOrientation"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ItemOrientationProperty =
            DependencyProperty.Register(nameof(ItemOrientation), typeof(Orientation), typeof(VideoView), new PropertyMetadata(default(Orientation), new PropertyChangedCallback(OnOrientationChanged)));

        /// <summary>
        /// <see cref="HeaderText"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(VideoView), new PropertyMetadata(string.Empty));

        /// <summary>
        /// <see cref="IsShowHeader"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsShowHeaderProperty =
            DependencyProperty.Register(nameof(IsShowHeader), typeof(bool), typeof(VideoView), new PropertyMetadata(true));

        /// <summary>
        /// <see cref="ItemTemplate"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(VideoItem), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="AdditionalContent"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty AdditionalContentProperty =
            DependencyProperty.Register(nameof(AdditionalContent), typeof(object), typeof(VideoView), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="IsAutoFillEnable"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsAutoFillEnableProperty =
            DependencyProperty.Register(nameof(IsAutoFillEnable), typeof(bool), typeof(VideoView), new PropertyMetadata(true));

        /// <summary>
        /// <see cref="HeaderVisibility"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty HeaderVisibilityProperty =
            DependencyProperty.Register(nameof(HeaderVisibility), typeof(Visibility), typeof(VideoView), new PropertyMetadata(Visibility.Visible));

        private ScrollViewer _parentScrollViewer;
        private double _itemHolderHeight = 0d;

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoView"/> class.
        /// </summary>
        public VideoView()
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
        /// 是否显示标题文本.
        /// </summary>
        public bool IsShowHeader
        {
            get { return (bool)GetValue(IsShowHeaderProperty); }
            set { SetValue(IsShowHeaderProperty, value); }
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

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as VideoView;
            if (e.NewValue is Orientation type)
            {
                instance.CheckOrientationStatus();
            }
        }

        private void CheckOrientationStatus()
        {
            switch (ItemOrientation)
            {
                case Orientation.Vertical:
                    VideoRepeater.Layout = GridLayout;
                    break;
                case Orientation.Horizontal:
                    VideoRepeater.Layout = ListLayout;
                    break;
                default:
                    break;
            }

            ChangeInitializedItemOrientation();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _parentScrollViewer = this.FindAscendantElementByType<ScrollViewer>();
            if (_parentScrollViewer != null)
            {
                _parentScrollViewer.ViewChanged += OnParentScrollViewerViewChanged;
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
            if (!e.IsIntermediate)
            {
                var currentPosition = _parentScrollViewer.VerticalOffset;
                if (_parentScrollViewer.ScrollableHeight - currentPosition <= _itemHolderHeight)
                {
                    RequestLoadMore?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void ChangeInitializedItemOrientation()
        {
            if (ItemsSource is ObservableCollection<VideoViewModel> items)
            {
                for (var i = 0; i < items.Count; i++)
                {
                    var element = VideoRepeater.TryGetElement(i);
                    if (element != null && element is VideoItem vi)
                    {
                        if (vi.Orientation != ItemOrientation)
                        {
                            vi.Orientation = ItemOrientation;
                        }
                    }
                }
            }
        }

        private void OnElementPrepared(Microsoft.UI.Xaml.Controls.ItemsRepeater sender, Microsoft.UI.Xaml.Controls.ItemsRepeaterElementPreparedEventArgs args)
        {
            if (args.Element != null && args.Element is VideoItem item)
            {
                item.Orientation = ItemOrientation;
                if (IsAutoFillEnable &&
                    ItemsSource is ObservableCollection<VideoViewModel> collectionSource &&
                    _parentScrollViewer != null &&
                    args.Index >= collectionSource.Count - 1)
                {
                    var size = item.GetHolderSize();
                    _itemHolderHeight = size.Height;
                    var isNeedLoadMore = false;
                    if (double.IsInfinity(size.Width))
                    {
                        isNeedLoadMore = (args.Index + 1) * size.Height <= _parentScrollViewer.ViewportHeight;
                    }
                    else
                    {
                        var rowCount = args.Index / (_parentScrollViewer.ViewportWidth / size.Width);
                        isNeedLoadMore = rowCount * size.Height <= _parentScrollViewer.ViewportHeight;
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

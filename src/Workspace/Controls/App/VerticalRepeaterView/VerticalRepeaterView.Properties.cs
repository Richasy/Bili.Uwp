// Copyright (c) Richasy. All rights reserved.

using System;
using Microsoft.UI.Xaml;

namespace Bili.Workspace.Controls.App
{
    /// <summary>
    /// 视频视图.
    /// </summary>
    public sealed partial class VerticalRepeaterView
    {
        /// <summary>
        /// <see cref="ItemsSource"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(VerticalRepeaterView), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="HeaderText"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(VerticalRepeaterView), new PropertyMetadata(string.Empty));

        /// <summary>
        /// <see cref="ItemTemplate"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate), typeof(object), typeof(VerticalRepeaterView), new PropertyMetadata(null));

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
        /// <see cref="VerticalCacheSize"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty VerticalCacheSizeProperty =
            DependencyProperty.Register(nameof(VerticalCacheSize), typeof(int), typeof(VerticalRepeaterView), new PropertyMetadata(10));

        /// <summary>
        /// 在外部的ScrollViewer滚动到接近底部时发生.
        /// </summary>
        public event EventHandler RequestLoadMore;

        /// <summary>
        /// 条目模板.
        /// </summary>
        public object ItemTemplate
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
        /// 垂直方向缓存大小.
        /// </summary>
        public int VerticalCacheSize
        {
            get { return (int)GetValue(VerticalCacheSizeProperty); }
            set { SetValue(VerticalCacheSizeProperty, value); }
        }
    }
}

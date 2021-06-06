// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
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
        /// Initializes a new instance of the <see cref="VideoView"/> class.
        /// </summary>
        public VideoView()
        {
            this.InitializeComponent();
        }

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
            }
        }
    }
}

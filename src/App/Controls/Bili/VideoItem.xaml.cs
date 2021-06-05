// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 视频条目.
    /// </summary>
    public sealed partial class VideoItem : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(VideoViewModel), typeof(VideoItem), new PropertyMetadata(null, new PropertyChangedCallback(OnViewModelChanged)));

        /// <summary>
        /// <see cref="Orientation"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(VideoItem), new PropertyMetadata(default(Orientation), new PropertyChangedCallback(OnOrientationChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoItem"/> class.
        /// </summary>
        public VideoItem()
        {
            this.InitializeComponent();
            this.SizeChanged += OnSizeChanged;
        }

        /// <summary>
        /// 视频视图模型.
        /// </summary>
        public VideoViewModel ViewModel
        {
            get { return (VideoViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 视频的布局方式.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as VideoItem;
            if (e.NewValue != null && e.NewValue is VideoViewModel vm)
            {
                var description = string.Empty;
                var playStr = $"播放:{vm.PlayCount}";
                var danmakuStr = $"弹幕：{vm.DanmakuCount}";
                description = playStr + " · " + danmakuStr;
                instance.DescriptionBlock.Text = description;
                instance.CheckOrientation();
            }
        }

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as VideoItem;
            if (e.NewValue is Orientation type)
            {
                switch (type)
                {
                    case Orientation.Vertical:
                        VisualStateManager.GoToState(instance, nameof(VerticalState), false);
                        break;
                    case Orientation.Horizontal:
                        VisualStateManager.GoToState(instance, nameof(HorizontalState), false);
                        break;
                    default:
                        break;
                }
            }
        }

        private void CheckOrientation()
        {
            var windowWidth = Window.Current.Bounds.Width;
            Orientation = windowWidth <= 641 ? Orientation.Horizontal : Orientation.Vertical;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            CheckOrientation();
        }

        private void OnContainerTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
        }
    }
}

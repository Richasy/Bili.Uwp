// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.Foundation;
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
        /// <see cref="IsShowPublishDateTime"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsShowPublishDateTimeProperty =
            DependencyProperty.Register(nameof(IsShowPublishDateTime), typeof(bool), typeof(VideoItem), new PropertyMetadata(false));

        /// <summary>
        /// <see cref="IsShowReplayCount"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsShowReplayCountProperty =
            DependencyProperty.Register(nameof(IsShowReplayCount), typeof(bool), typeof(VideoItem), new PropertyMetadata(false));

        /// <summary>
        /// <see cref="IsShowDanmakuCount"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsShowDanmakuCountProperty =
            DependencyProperty.Register(nameof(IsShowDanmakuCount), typeof(bool), typeof(VideoItem), new PropertyMetadata(false));

        /// <summary>
        /// <see cref="IsShowPlayCount"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsShowPlayCountProperty =
            DependencyProperty.Register(nameof(IsShowPlayCount), typeof(bool), typeof(VideoItem), new PropertyMetadata(false));

        /// <summary>
        /// <see cref="IsShowLikeCount"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsShowLikeCountProperty =
            DependencyProperty.Register(nameof(IsShowLikeCount), typeof(bool), typeof(VideoItem), new PropertyMetadata(false));

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

        /// <summary>
        /// 是否显示播放数.
        /// </summary>
        public bool IsShowPlayCount
        {
            get { return (bool)GetValue(IsShowPlayCountProperty); }
            set { SetValue(IsShowPlayCountProperty, value); }
        }

        /// <summary>
        /// 是否显示弹幕数.
        /// </summary>
        public bool IsShowDanmakuCount
        {
            get { return (bool)GetValue(IsShowDanmakuCountProperty); }
            set { SetValue(IsShowDanmakuCountProperty, value); }
        }

        /// <summary>
        /// 是否显示回复数.
        /// </summary>
        public bool IsShowReplayCount
        {
            get { return (bool)GetValue(IsShowReplayCountProperty); }
            set { SetValue(IsShowReplayCountProperty, value); }
        }

        /// <summary>
        /// 是否显示发布时间.
        /// </summary>
        public bool IsShowPublishDateTime
        {
            get { return (bool)GetValue(IsShowPublishDateTimeProperty); }
            set { SetValue(IsShowPublishDateTimeProperty, value); }
        }

        /// <summary>
        /// 是否显示点赞数.
        /// </summary>
        public bool IsShowLikeCount
        {
            get { return (bool)GetValue(IsShowLikeCountProperty); }
            set { SetValue(IsShowLikeCountProperty, value); }
        }

        /// <summary>
        /// 获取占位大小，这是一个固定值，用于预先测量.
        /// </summary>
        /// <returns><see cref="Size"/>.</returns>
        public Size GetHolderSize()
        {
            if (Orientation == Orientation.Horizontal)
            {
                return new Size(double.PositiveInfinity, 180);
            }
            else
            {
                return new Size(210, 228);
            }
        }

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as VideoItem;
            if (e.NewValue != null && e.NewValue is VideoViewModel vm)
            {
                var description = string.Empty;
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

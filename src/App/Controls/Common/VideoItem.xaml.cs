// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.App.Pages;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.ViewModels.Uwp;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 视频条目.
    /// </summary>
    public sealed partial class VideoItem : UserControl, IRepeaterItem, IDynamicLayoutItem
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(VideoViewModel), typeof(VideoItem), new PropertyMetadata(null));

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
        /// <see cref="IsShowViewerCount"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsShowViewerCountProperty =
            DependencyProperty.Register(nameof(IsShowViewerCount), typeof(bool), typeof(VideoItem), new PropertyMetadata(false));

        /// <summary>
        /// <see cref="IsShowDuration"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsShowDurationProperty =
            DependencyProperty.Register(nameof(IsShowDuration), typeof(bool), typeof(VideoItem), new PropertyMetadata(true));

        /// <summary>
        /// <see cref="IsShowAvatar"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsShowAvatarProperty =
            DependencyProperty.Register(nameof(IsShowAvatar), typeof(bool), typeof(VideoItem), new PropertyMetadata(true));

        /// <summary>
        /// <see cref="AdditionalFooterContent"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty AdditionalFooterContentProperty =
            DependencyProperty.Register(nameof(AdditionalFooterContent), typeof(object), typeof(VideoItem), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="AdditionalOverlayContent"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty AdditionalOverlayContentProperty =
            DependencyProperty.Register(nameof(AdditionalOverlayContent), typeof(object), typeof(VideoItem), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="AdditionalOverlayContentVisibility"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty AdditionalOverlayContentVisibilityProperty =
            DependencyProperty.Register(nameof(AdditionalOverlayContentVisibility), typeof(Visibility), typeof(VideoItem), new PropertyMetadata(Visibility.Collapsed));

        /// <summary>
        /// <see cref="HorizontalCoverWidth"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty HorizontalCoverWidthProperty =
            DependencyProperty.Register(nameof(HorizontalCoverWidth), typeof(double), typeof(VideoItem), new PropertyMetadata(160d));

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoItem"/> class.
        /// </summary>
        public VideoItem()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
            this.AppViewModel = AppViewModel.Instance;
        }

        /// <summary>
        /// 应用视图模型.
        /// </summary>
        public AppViewModel AppViewModel { get; private set; }

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
        /// 是否显示点赞数.
        /// </summary>
        public bool IsShowDuration
        {
            get { return (bool)GetValue(IsShowDurationProperty); }
            set { SetValue(IsShowDurationProperty, value); }
        }

        /// <summary>
        /// 是否显示观看人数.
        /// </summary>
        public bool IsShowViewerCount
        {
            get { return (bool)GetValue(IsShowViewerCountProperty); }
            set { SetValue(IsShowViewerCountProperty, value); }
        }

        /// <summary>
        /// 是否显示头像.
        /// </summary>
        public bool IsShowAvatar
        {
            get { return (bool)GetValue(IsShowAvatarProperty); }
            set { SetValue(IsShowAvatarProperty, value); }
        }

        /// <summary>
        /// 附加的底部内容，显示在内容区底部.
        /// </summary>
        public object AdditionalFooterContent
        {
            get { return (object)GetValue(AdditionalFooterContentProperty); }
            set { SetValue(AdditionalFooterContentProperty, value); }
        }

        /// <summary>
        /// 附加的Overlay内容，在竖向排版时显示在视频封面上，横向排版时显示在标题下方.
        /// </summary>
        public object AdditionalOverlayContent
        {
            get { return (object)GetValue(AdditionalOverlayContentProperty); }
            set { SetValue(AdditionalOverlayContentProperty, value); }
        }

        /// <summary>
        /// 是否显示附加的Overlay内容.
        /// </summary>
        public Visibility AdditionalOverlayContentVisibility
        {
            get { return (Visibility)GetValue(AdditionalOverlayContentVisibilityProperty); }
            set { SetValue(AdditionalOverlayContentVisibilityProperty, value); }
        }

        /// <summary>
        /// 水平状态下的封面宽度.
        /// </summary>
        public double HorizontalCoverWidth
        {
            get { return (double)GetValue(HorizontalCoverWidthProperty); }
            set { SetValue(HorizontalCoverWidthProperty, value); }
        }

        /// <inheritdoc/>
        public Size GetHolderSize()
        {
            if (Orientation == Orientation.Horizontal)
            {
                return new Size(double.PositiveInfinity, 180);
            }
            else
            {
                return new Size(210, 248);
            }
        }

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as VideoItem;
            instance.CheckOrientation();
        }

        private void OnLoaded(object sender, RoutedEventArgs e) => CheckOrientation();

        private void CheckOrientation()
        {
            switch (Orientation)
            {
                case Orientation.Vertical:
                    VisualStateManager.GoToState(this, nameof(VerticalState), false);
                    CoverContainer.Width = double.NaN;
                    break;
                case Orientation.Horizontal:
                    VisualStateManager.GoToState(this, nameof(HorizontalState), false);
                    CoverContainer.Width = this.HorizontalCoverWidth;
                    break;
                default:
                    break;
            }
        }

        private void OnContainerClickAsync(object sender, RoutedEventArgs e)
        {
            AppViewModel.OpenPlayer(ViewModel);
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 动态视频条目.
    /// </summary>
    public sealed partial class DynamicVideoCard : UserControl, IDynamicLayoutItem
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(VideoViewModel), typeof(DynamicVideoCard), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="Orientation"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(DynamicVideoCard), new PropertyMetadata(default(Orientation), new PropertyChangedCallback(OnOrientationChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicVideoCard"/> class.
        /// </summary>
        public DynamicVideoCard()
        {
            InitializeComponent();
            Loaded += OnLoaded;
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

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as DynamicVideoCard;
            instance.CheckOrientation();
        }

        private void OnLoaded(object sender, RoutedEventArgs e) => CheckOrientation();

        private void CheckOrientation()
        {
            switch (Orientation)
            {
                case Orientation.Vertical:
                    VisualStateManager.GoToState(this, nameof(VerticalState), false);
                    break;
                case Orientation.Horizontal:
                    VisualStateManager.GoToState(this, nameof(HorizontalState), false);
                    break;
                default:
                    break;
            }
        }
    }
}

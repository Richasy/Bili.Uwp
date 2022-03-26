// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 动态PGC条目.
    /// </summary>
    public sealed partial class DynamicPgcItem : UserControl, IDynamicLayoutItem
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(SeasonViewModel), typeof(DynamicPgcItem), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="Orientation"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(DynamicPgcItem), new PropertyMetadata(default(Orientation), new PropertyChangedCallback(OnOrientationChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicVideoCard"/> class.
        /// </summary>
        public DynamicPgcItem()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        /// <summary>
        /// 剧集视图模型.
        /// </summary>
        public SeasonViewModel ViewModel
        {
            get { return (SeasonViewModel)GetValue(ViewModelProperty); }
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
            var instance = d as DynamicPgcItem;
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

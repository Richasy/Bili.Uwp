// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.ViewModels.Uwp;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// PGC条目.
    /// </summary>
    public sealed partial class PgcItem : UserControl, IDynamicLayoutItem, IRepeaterItem
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(SeasonViewModel), typeof(PgcItem), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="Orientation"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register(nameof(Orientation), typeof(Orientation), typeof(VideoCard), new PropertyMetadata(default(Orientation), new PropertyChangedCallback(OnOrientationChanged)));

        /// <summary>
        /// <see cref="TagVisibility"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty TagVisibilityProperty =
            DependencyProperty.Register(nameof(TagVisibility), typeof(Visibility), typeof(PgcItem), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// <see cref="IsSlim"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsSlimProperty =
            DependencyProperty.Register(nameof(IsSlim), typeof(bool), typeof(PgcItem), new PropertyMetadata(false, new PropertyChangedCallback(OnIsSlimChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="PgcItem"/> class.
        /// </summary>
        public PgcItem()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 条目被点击时触发.
        /// </summary>
        public event EventHandler<SeasonViewModel> ItemClick;

        /// <summary>
        /// 视图模型.
        /// </summary>
        public SeasonViewModel ViewModel
        {
            get { return (SeasonViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// PGC条目的布局方式.
        /// </summary>
        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        /// <summary>
        /// 标签的可见性.
        /// </summary>
        public Visibility TagVisibility
        {
            get { return (Visibility)GetValue(TagVisibilityProperty); }
            set { SetValue(TagVisibilityProperty, value); }
        }

        /// <summary>
        /// 是否显示为紧缩模式.
        /// </summary>
        public bool IsSlim
        {
            get { return (bool)GetValue(IsSlimProperty); }
            set { SetValue(IsSlimProperty, value); }
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
                return new Size(300, 180);
            }
        }

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as PgcItem;
            instance.CheckOrientation();
        }

        private static void OnIsSlimChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as PgcItem;
            instance.CheckSlim();
        }

        private void OnRootCardClick(object sender, RoutedEventArgs e)
        {
            ItemClick?.Invoke(this, ViewModel);
            AppViewModel.Instance.OpenPlayer(ViewModel);
        }

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

        private void CheckSlim()
        {
            var stateName = IsSlim ? nameof(SlimState) : nameof(StandardState);
            VisualStateManager.GoToState(this, stateName, false);
            RootCard.IsEnableShadow = !IsSlim;
        }
    }
}

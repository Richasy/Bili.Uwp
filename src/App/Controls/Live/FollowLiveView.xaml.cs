// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 关注的直播间视图.
    /// </summary>
    public sealed partial class FollowLiveView : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(LiveModuleViewModel), typeof(FollowLiveView), new PropertyMetadata(LiveModuleViewModel.Instance));

        /// <summary>
        /// <see cref="ItemOrientation"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ItemOrientationProperty =
            DependencyProperty.Register(nameof(ItemOrientation), typeof(Orientation), typeof(FollowLiveView), new PropertyMetadata(default(Orientation), new PropertyChangedCallback(OnOrientationChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="FollowLiveView"/> class.
        /// </summary>
        public FollowLiveView()
        {
            InitializeComponent();
            Current = this;
            Loaded += OnLoaded;
        }

        public event EventHandler ItemClick;

        /// <summary>
        /// <see cref="FollowLiveView"/>的实例.
        /// </summary>
        public static FollowLiveView Current { get; private set; }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public LiveModuleViewModel ViewModel
        {
            get { return (LiveModuleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 布局方向.
        /// </summary>
        public Orientation ItemOrientation
        {
            get { return (Orientation)GetValue(ItemOrientationProperty); }
            set { SetValue(ItemOrientationProperty, value); }
        }

        private static void OnOrientationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as FollowLiveView;
            if (e.NewValue is Orientation ori)
            {
                instance.ChangeInitializedItemOrientation();
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ChangeInitializedItemOrientation();
        }

        private void ChangeInitializedItemOrientation()
        {
            switch (ItemOrientation)
            {
                case Orientation.Vertical:
                    VisualStateManager.GoToState(this, nameof(HorizontalState), false);
                    break;
                case Orientation.Horizontal:
                    VisualStateManager.GoToState(this, nameof(VerticalState), false);
                    break;
                default:
                    break;
            }

            for (var i = 0; i < ViewModel.FollowLiveRoomCollection.Count; i++)
            {
                var element = LiveRepeater.TryGetElement(i);
                if (element != null && element is FollowLiveItem vi)
                {
                    if (vi.Orientation != ItemOrientation)
                    {
                        vi.Orientation = ItemOrientation;
                    }
                }
            }
        }

        private void OnElementPrepared(Microsoft.UI.Xaml.Controls.ItemsRepeater sender, Microsoft.UI.Xaml.Controls.ItemsRepeaterElementPreparedEventArgs args)
        {
            if (args.Element != null && args.Element is FollowLiveItem item)
            {
                item.Orientation = ItemOrientation;
            }
        }

        private void OnItemClick(object sender, System.EventArgs e)
        {
            ItemClick?.Invoke(this, EventArgs.Empty);
        }
    }
}

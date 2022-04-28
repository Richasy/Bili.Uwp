// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// 播放器组件基类.
    /// </summary>
    public class PlayerComponent : UserControl
    {
        /// <summary>
        /// <see cref="VerticalScrollMode"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty VerticalScrollModeProperty =
            DependencyProperty.Register(nameof(VerticalScrollMode), typeof(ScrollMode), typeof(PlayerComponent), new PropertyMetadata(ScrollMode.Enabled));

        /// <summary>
        /// 播放器视图模型.
        /// </summary>
        public PlayerViewModel ViewModel { get; } = PlayerViewModel.Instance;

        /// <summary>
        /// 垂直滚动方式.
        /// </summary>
        public ScrollMode VerticalScrollMode
        {
            get { return (ScrollMode)GetValue(VerticalScrollModeProperty); }
            set { SetValue(VerticalScrollModeProperty, value); }
        }
    }
}

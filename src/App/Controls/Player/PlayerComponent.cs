// Copyright (c) Richasy. All rights reserved.

using Microsoft.UI.Xaml.Controls;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
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
            DependencyProperty.Register(nameof(VerticalScrollMode), typeof(ScrollingScrollMode), typeof(PlayerComponent), new PropertyMetadata(ScrollingScrollMode.Enabled));

        /// <summary>
        /// 播放器视图模型.
        /// </summary>
        public PlayerViewModel ViewModel { get; } = PlayerViewModel.Instance;

        /// <summary>
        /// 垂直滚动方式.
        /// </summary>
        public ScrollingScrollMode VerticalScrollMode
        {
            get { return (ScrollingScrollMode)GetValue(VerticalScrollModeProperty); }
            set { SetValue(VerticalScrollModeProperty, value); }
        }
    }
}

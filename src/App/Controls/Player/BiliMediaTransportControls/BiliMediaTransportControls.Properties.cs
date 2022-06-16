// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// 媒体传输控件.
    /// </summary>
    public sealed partial class BiliMediaTransportControls
    {
        /// <summary>
        /// <see cref="IsForwardSkipButtonVisible"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsForwardSkipButtonVisibleProperty =
            DependencyProperty.Register(nameof(IsForwardSkipButtonVisible), typeof(bool), typeof(BiliMediaTransportControls), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="IsPlaybackRateButtonVisible"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsPlaybackRateButtonVisibleProperty =
            DependencyProperty.Register(nameof(IsPlaybackRateButtonVisible), typeof(bool), typeof(BiliMediaTransportControls), new PropertyMetadata(default));

        /// <summary>
        /// 是否显示跳进按钮.
        /// </summary>
        public bool IsForwardSkipButtonVisible
        {
            get { return (bool)GetValue(IsForwardSkipButtonVisibleProperty); }
            set { SetValue(IsForwardSkipButtonVisibleProperty, value); }
        }

        /// <summary>
        /// 是否显示播放速率按钮.
        /// </summary>
        public bool IsPlaybackRateButtonVisible
        {
            get { return (bool)GetValue(IsPlaybackRateButtonVisibleProperty); }
            set { SetValue(IsPlaybackRateButtonVisibleProperty, value); }
        }
    }
}

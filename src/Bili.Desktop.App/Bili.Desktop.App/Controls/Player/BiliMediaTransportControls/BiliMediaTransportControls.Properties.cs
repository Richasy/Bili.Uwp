// Copyright (c) Richasy. All rights reserved.

using Microsoft.UI.Xaml;

namespace Bili.Desktop.App.Controls.Player
{
    /// <summary>
    /// 媒体传输控件.
    /// </summary>
    public sealed partial class BiliMediaTransportControls
    {
        /// <summary>
        /// <see cref="TransportVisibility"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty TransportVisibilityProperty =
            DependencyProperty.Register(nameof(TransportVisibility), typeof(Visibility), typeof(BiliMediaTransportControls), new PropertyMetadata(default, new PropertyChangedCallback(OnTransportVisibilityChanged)));

        /// <summary>
        /// 传输控件是否可见.
        /// </summary>
        public Visibility TransportVisibility
        {
            get { return (Visibility)GetValue(TransportVisibilityProperty); }
            set { SetValue(TransportVisibilityProperty, value); }
        }

        /// <summary>
        /// 弹幕输入框是否获得了焦点.
        /// </summary>
        public bool IsDanmakuBoxFocused => _danmakuBox?.IsInputFocused ?? false;
    }
}

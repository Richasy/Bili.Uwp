// Copyright (c) Richasy. All rights reserved.

using NSDanmaku.Controls;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 哔哩播放器的媒体传输控件.
    /// </summary>
    public partial class BiliPlayerTransportControls : MediaTransportControls
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BiliPlayerTransportControls"/> class.
        /// </summary>
        public BiliPlayerTransportControls()
        {
            this.DefaultStyleKey = typeof(BiliPlayerTransportControls);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _danmakuControl = GetTemplateChild("DanmakuControl") as Danmaku;
            base.OnApplyTemplate();
        }
    }
}

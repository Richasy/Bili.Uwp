// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 视频播放器.
    /// </summary>
    public partial class BiliPlayer : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BiliPlayer"/> class.
        /// </summary>
        public BiliPlayer()
        {
            this.DefaultStyleKey = typeof(BiliPlayer);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            if (ViewModel.BiliPlayer == null)
            {
                var mediaPlayerElement = GetTemplateChild("MediaPlayerElement") as MediaPlayerElement;
                ViewModel.ApplyMediaControl(mediaPlayerElement);
            }
        }
    }
}

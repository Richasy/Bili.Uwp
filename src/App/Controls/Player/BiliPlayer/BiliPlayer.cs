// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 视频播放器.
    /// </summary>
    public partial class BiliPlayer : Control
    {
        private MediaPlayerElement mediaPlayerElement;

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
            mediaPlayerElement = GetTemplateChild("MediaPlayerElement") as MediaPlayerElement;

            if (ViewModel.MediaPlayerElement == null)
            {
                ViewModel.ApplyMediaControl(this, mediaPlayerElement);
            }
        }
    }
}

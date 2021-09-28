// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.UI.Xaml;
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
                var mediaElement = GetTemplateChild("MediaElement") as MediaElement;
                ViewModel.ApplyMediaControl(mediaPlayerElement, mediaElement);
            }
        }
    }
}

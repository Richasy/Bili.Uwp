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
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            if (ViewModel.BiliPlayer == null)
            {
                var mediaPlayerElement = GetTemplateChild("MediaPlayerElement") as MediaPlayerElement;
                ViewModel.ApplyMediaControl(mediaPlayerElement);
            }

            _mediaTransport = GetTemplateChild(MTCName) as BiliPlayerTransportControls;
            CheckMTCStyle();
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.IsLive))
            {
                CheckMTCStyle();
            }
        }

        private void CheckMTCStyle()
        {
            var styleName = ViewModel.IsLive ? "LiveMTCStyle" : "DefaultMTCStyle";
            var style = ServiceLocator.Instance.GetService<IResourceToolkit>().GetResource<Style>(styleName);
            _mediaTransport.Style = style;
        }
    }
}

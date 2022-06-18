// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// 媒体传输控制器.
    /// </summary>
    public sealed partial class BiliMediaTransportControls
    {
        private const string VolumeSliderName = "VolumeSlider";
        private const string FormatListViewName = "FormatListView";

        private Slider _volumeSlider;
        private ListView _formatListView;
    }
}

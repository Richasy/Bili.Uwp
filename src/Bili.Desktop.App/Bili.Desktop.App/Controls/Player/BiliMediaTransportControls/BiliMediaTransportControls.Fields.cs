// Copyright (c) Richasy. All rights reserved.

using Microsoft.UI.Xaml.Controls;

namespace Bili.Desktop.App.Controls.Player
{
    /// <summary>
    /// 媒体传输控制器.
    /// </summary>
    public sealed partial class BiliMediaTransportControls
    {
        private const string VolumeSliderName = "VolumeSlider";
        private const string FormatListViewName = "FormatListView";
        private const string PlayPauseButtonName = "PlayPauseButton";
        private const string DanmakuBoxName = "DanmakuBox";
        private const string RootGridName = "RootGrid";

        private Slider _volumeSlider;
        private ListView _formatListView;
        private Button _playPauseButton;
        private Grid _rootGrid;
    }
}

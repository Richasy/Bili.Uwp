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
        private const string PlayPauseButtonName = "PlayPauseButton";
        private const string NormalProgressContainerName = "NormalProgressContainer";
        private const string InteractionProgressContainerName = "InteractionProgressContainer";
        private const string InteractionProgressSliderName = "InteractionProgressSlider";

        private Slider _volumeSlider;
        private ListView _formatListView;
        private Button _playPauseButton;
        private Panel _normalProgressContainer;
        private Panel _interactionProgressContainer;
        private Slider _interactionProgressSlider;

        private bool _isInteractionProgressAutoAssign;
    }
}

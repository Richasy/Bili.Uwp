// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Video;

namespace Bili.Uwp.App.Controls.Player
{
    /// <summary>
    /// 稍后再看视频列表.
    /// </summary>
    public sealed partial class VideoPlaylistView : VideoPlaylistViewBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPlaylistView"/> class.
        /// </summary>
        public VideoPlaylistView() => InitializeComponent();
    }

    /// <summary>
    /// <see cref="RelatedVideoView"/> 的基类.
    /// </summary>
    public class VideoPlaylistViewBase : ReactiveUserControl<IVideoPlayerPageViewModel>
    {
    }
}

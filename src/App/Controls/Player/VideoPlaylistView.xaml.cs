// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Video;
using ReactiveUI;
using Splat;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// 稍后再看视频列表.
    /// </summary>
    public sealed partial class VideoPlaylistView : VideoPlaylistViewBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPlaylistView"/> class.
        /// </summary>
        public VideoPlaylistView()
        {
            this.InitializeComponent();
            ViewModel = Splat.Locator.Current.GetService<VideoPlayerPageViewModel>();
            DataContext = ViewModel;
        }
    }

    /// <summary>
    /// <see cref="RelatedVideoView"/> 的基类.
    /// </summary>
    public class VideoPlaylistViewBase : ReactiveUserControl<VideoPlayerPageViewModel>
    {
    }
}

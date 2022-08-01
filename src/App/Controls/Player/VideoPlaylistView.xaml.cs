// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Video;
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
            InitializeComponent();
            ViewModel = Locator.Current.GetService<IVideoPlayerPageViewModel>();
            DataContext = ViewModel;
        }
    }

    /// <summary>
    /// <see cref="RelatedVideoView"/> 的基类.
    /// </summary>
    public class VideoPlaylistViewBase : ReactiveUserControl<IVideoPlayerPageViewModel>
    {
    }
}

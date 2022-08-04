// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Video;
using ReactiveUI;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// 关联视频视图.
    /// </summary>
    public sealed partial class RelatedVideoView : RelatedVideoViewBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedVideoView"/> class.
        /// </summary>
        public RelatedVideoView() => InitializeComponent();
    }

    /// <summary>
    /// <see cref="RelatedVideoView"/> 的基类.
    /// </summary>
    public class RelatedVideoViewBase : ReactiveUserControl<IVideoPlayerPageViewModel>
    {
    }
}

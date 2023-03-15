// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Video;

namespace Bili.Workspace.Controls.App
{
    /// <summary>
    /// 用来显示视频条目的 UI 单元，可以通过样式展现不同的布局.
    /// </summary>
    public sealed partial class VideoItem : ReactiveControl<IVideoItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoItem"/> class.
        /// </summary>
        public VideoItem() => DefaultStyleKey = typeof(VideoItem);
    }
}

// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Video;
using Bili.ViewModels.Interfaces.Video;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 可选择的视频标识符视图模型.
    /// </summary>
    public sealed class VideoIdentifierSelectableViewModel : SelectableViewModelBase<VideoIdentifier>, IVideoIdentifierSelectableViewModel
    {
        /// <summary>
        /// 索引.
        /// </summary>
        [Reactive]
        public int Index { get; set; }
    }
}

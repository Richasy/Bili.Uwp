// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Video;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 可选择的视频标识符视图模型.
    /// </summary>
    public sealed class VideoIdentifierSelectableViewModel : SelectableViewModelBase<VideoIdentifier>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoIdentifierSelectableViewModel"/> class.
        /// </summary>
        public VideoIdentifierSelectableViewModel(VideoIdentifier identifier, int index, bool isSelected)
            : base(identifier, isSelected) => Index = index;

        /// <summary>
        /// 索引.
        /// </summary>
        [Reactive]
        public int Index { get; set; }
    }
}

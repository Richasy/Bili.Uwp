// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Video;

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
        public VideoIdentifierSelectableViewModel(VideoIdentifier identifier, bool isSelected)
            : base(identifier, isSelected)
        {
        }
    }
}

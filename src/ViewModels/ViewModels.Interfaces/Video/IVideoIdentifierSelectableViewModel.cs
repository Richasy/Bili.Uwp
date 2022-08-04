// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Video;

namespace Bili.ViewModels.Interfaces.Video
{
    /// <summary>
    /// 可选择的视频标识符视图模型的接口定义.
    /// </summary>
    public interface IVideoIdentifierSelectableViewModel : ISelectableViewModel<VideoIdentifier>
    {
        /// <summary>
        /// 索引.
        /// </summary>
        int Index { get; set; }
    }
}

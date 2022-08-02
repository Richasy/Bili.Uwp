// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Video;

namespace Bili.ViewModels.Interfaces.Video
{
    /// <summary>
    /// 收藏夹分组视图模型的接口定义.
    /// </summary>
    public interface IVideoFavoriteFolderGroupViewModel : IInjectDataViewModel<VideoFavoriteFolderGroup>, IInformationFlowViewModel<IVideoFavoriteFolderViewModel>
    {
        /// <summary>
        /// 收藏夹分组下是否为空.
        /// </summary>
        public bool IsEmpty { get; }

        /// <summary>
        /// 是否还有更多内容.
        /// </summary>
        public bool HasMore { get; }
    }
}

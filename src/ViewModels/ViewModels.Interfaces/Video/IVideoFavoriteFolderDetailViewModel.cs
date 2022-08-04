// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.User;
using Bili.Models.Data.Video;

namespace Bili.ViewModels.Interfaces.Video
{
    /// <summary>
    /// 视频收藏夹详情视图模型的接口定义.
    /// </summary>
    public interface IVideoFavoriteFolderDetailViewModel : IInformationFlowViewModel<IVideoItemViewModel>, IInjectDataViewModel<VideoFavoriteFolder>
    {
        /// <summary>
        /// 收藏夹创建者信息.
        /// </summary>
        UserProfile User { get; }

        /// <summary>
        /// 收藏夹是否为空.
        /// </summary>
        bool IsEmpty { get; }
    }
}

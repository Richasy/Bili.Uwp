// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Video;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Video
{
    /// <summary>
    /// 视频收藏模块视图模型的接口定义.
    /// </summary>
    public interface IVideoFavoriteModuleViewModel : IInformationFlowViewModel<IVideoFavoriteFolderGroupViewModel>
    {
        /// <summary>
        /// 显示默认收藏夹详情的命令.
        /// </summary>
        IRelayCommand ShowDefaultFolderDetailCommand { get; }

        /// <summary>
        /// 默认收藏夹.
        /// </summary>
        VideoFavoriteFolder DefaultFolder { get; }

        /// <summary>
        /// 默认视频收藏夹是否为空.
        /// </summary>
        bool IsDefaultFolderEmpty { get; }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.Data.Community;
using Bili.ViewModels.Interfaces.Common;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Live
{
    /// <summary>
    /// 直播首页视图模型的接口定义.
    /// </summary>
    public interface ILiveFeedPageViewModel : IInformationFlowViewModel<ILiveItemViewModel>
    {
        /// <summary>
        /// 横幅集合.
        /// </summary>
        ObservableCollection<IBannerViewModel> Banners { get; }

        /// <summary>
        /// 关注的直播间集合.
        /// </summary>
        ObservableCollection<ILiveItemViewModel> Follows { get; }

        /// <summary>
        /// 热门分区.
        /// </summary>
        ObservableCollection<Partition> HotPartitions { get; }

        /// <summary>
        /// 查看全部分区的命令.
        /// </summary>
        IRelayCommand SeeAllPartitionsCommand { get; }

        /// <summary>
        /// 用户是否已登录.
        /// </summary>
        bool IsLoggedIn { get; }

        /// <summary>
        /// 关注的直播间是否为空.
        /// </summary>
        bool IsFollowsEmpty { get; }
    }
}

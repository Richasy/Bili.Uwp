// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.Data.User;
using Bili.ViewModels.Interfaces.Video;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Account
{
    /// <summary>
    /// 用户空间视图模型的接口定义.
    /// </summary>
    public interface IUserSpaceViewModel : IInformationFlowViewModel<IVideoItemViewModel>
    {
        /// <summary>
        /// 搜索命令.
        /// </summary>
        IAsyncRelayCommand SearchCommand { get; }

        /// <summary>
        /// 进入搜索模式的命令.
        /// </summary>
        IRelayCommand EnterSearchModeCommand { get; }

        /// <summary>
        /// 退出搜索模式的命令.
        /// </summary>
        IRelayCommand ExitSearchModeCommand { get; }

        /// <summary>
        /// 前往粉丝页面的命令.
        /// </summary>
        IRelayCommand GotoFansPageCommand { get; }

        /// <summary>
        /// 前往关注者页面的命令.
        /// </summary>
        IRelayCommand GotoFollowsPageCommand { get; }

        /// <summary>
        /// 固定条目的命令.
        /// </summary>
        IRelayCommand FixedCommand { get; }

        /// <summary>
        /// 搜索的视频结果.
        /// </summary>
        ObservableCollection<IVideoItemViewModel> SearchVideos { get; }

        /// <summary>
        /// 账户信息.
        /// </summary>
        IUserItemViewModel UserViewModel { get; }

        /// <summary>
        /// 是否为我自己（已登录用户）的用户空间.
        /// </summary>
        bool IsMe { get; }

        /// <summary>
        /// 是否为搜索模式.
        /// </summary>
        bool IsSearchMode { get; }

        /// <summary>
        /// 搜索关键词.
        /// </summary>
        string Keyword { get; set; }

        /// <summary>
        /// 空间视频是否为空.
        /// </summary>
        bool IsSpaceVideoEmpty { get; }

        /// <summary>
        /// 视频搜索结果是否为空.
        /// </summary>
        bool IsSearchVideoEmpty { get; }

        /// <summary>
        /// 该用户是否已经被固定在首页.
        /// </summary>
        bool IsFixed { get; }

        /// <summary>
        /// 是否正在搜索.
        /// </summary>
        bool IsSearching { get; }

        /// <summary>
        /// 是否可以搜索.
        /// </summary>
        bool CanSearch { get; }

        /// <summary>
        /// 设置用户信息.
        /// </summary>
        /// <param name="user">用户资料.</param>
        void SetUserProfile(UserProfile user);
    }
}

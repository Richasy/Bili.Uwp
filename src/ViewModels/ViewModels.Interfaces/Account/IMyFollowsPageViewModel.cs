// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.Data.Community;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Account
{
    /// <summary>
    /// 我的关注页面视图模型的接口定义.
    /// </summary>
    public interface IMyFollowsPageViewModel : IInformationFlowViewModel<IUserItemViewModel>
    {
        /// <summary>
        /// 关注分组列表.
        /// </summary>
        ObservableCollection<FollowGroup> Groups { get; }

        /// <summary>
        /// 选中分组命令.
        /// </summary>
        IRelayCommand<FollowGroup> SelectGroupCommand { get; }

        /// <summary>
        /// 当前分组.
        /// </summary>
        FollowGroup CurrentGroup { get; }

        /// <summary>
        /// 当前分组内容是否为空.
        /// </summary>
        bool IsCurrentGroupEmpty { get; }

        /// <summary>
        /// 用户名.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// 是否正在切换分组.
        /// </summary>
        bool IsSwitching { get; }
    }
}

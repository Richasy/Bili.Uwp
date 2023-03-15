// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Dynamic;
using Bili.ViewModels.Interfaces.Account;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Community
{
    /// <summary>
    /// 动态条目视图模型的接口定义.
    /// </summary>
    public interface IDynamicItemViewModel : IInjectDataViewModel<DynamicInformation>
    {
        /// <summary>
        /// 用户信息.
        /// </summary>
        IUserItemViewModel Publisher { get; }

        /// <summary>
        /// 是否已点赞.
        /// </summary>
        bool IsLiked { get; }

        /// <summary>
        /// 点赞次数的可读文本.
        /// </summary>
        string LikeCountText { get; }

        /// <summary>
        /// 评论次数文本.
        /// </summary>
        string CommentCountText { get; }

        /// <summary>
        /// 是否显示社区信息.
        /// </summary>
        bool IsShowCommunity { get; }

        /// <summary>
        /// 是否可以加入稍后再看.
        /// </summary>
        bool CanAddViewLater { get; }

        /// <summary>
        /// 点赞动态的命令.
        /// </summary>
        IAsyncRelayCommand ToggleLikeCommand { get; }

        /// <summary>
        /// 点击动态的命令.
        /// </summary>
        IAsyncRelayCommand ActiveCommand { get; }

        /// <summary>
        /// 显示用户详情的命令.
        /// </summary>
        IRelayCommand ShowUserDetailCommand { get; }

        /// <summary>
        /// 添加到稍后再看的命令.
        /// </summary>
        IRelayCommand AddToViewLaterCommand { get; }

        /// <summary>
        /// 显示评论详情的命令.
        /// </summary>
        IRelayCommand ShowCommentDetailCommand { get; }

        /// <summary>
        /// 显示评论详情的命令.
        /// </summary>
        IRelayCommand ShareCommand { get; }
    }
}

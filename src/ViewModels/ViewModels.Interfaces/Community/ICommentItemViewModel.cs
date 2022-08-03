// Copyright (c) Richasy. All rights reserved.

using System;
using System.Reactive;
using Bili.Models.Data.Community;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces.Community
{
    /// <summary>
    /// 评论条目视图模型的定义.
    /// </summary>
    public interface ICommentItemViewModel : IInjectDataViewModel<CommentInformation>
    {
        /// <summary>
        /// 点赞或取消点赞的命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> ToggleLikeCommand { get; }

        /// <summary>
        /// 显示评论详情的命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> ShowCommentDetailCommand { get; }

        /// <summary>
        /// 显示用户详情的命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> ShowUserDetailCommand { get; }

        /// <summary>
        /// 点击命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> ClickCommand { get; }

        /// <summary>
        /// 是否已点赞.
        /// </summary>
        bool IsLiked { get; }

        /// <summary>
        /// 点赞数的可读文本.
        /// </summary>
        string LikeCountText { get; }

        /// <summary>
        /// 回复数的可读文本.
        /// </summary>
        string ReplyCountText { get; set; }

        /// <summary>
        /// 发布日期的可读文本.
        /// </summary>
        string PublishDateText { get; }

        /// <summary>
        /// 是否用户高亮.
        /// </summary>
        bool IsUserHighlight { get; set; }

        /// <summary>
        /// 设置显示详情的行为.
        /// </summary>
        /// <param name="action">显示详情（展开评论）的行为.</param>
        void SetDetailAction(Action<ICommentItemViewModel> action);

        /// <summary>
        /// 设置点击的行为.
        /// </summary>
        /// <param name="action">显示详情（展开评论）的行为.</param>
        void SetClickAction(Action<ICommentItemViewModel> action);
    }
}

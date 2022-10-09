// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Models.App.Other;
using Bili.Models.Enums.Bili;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Community
{
    /// <summary>
    /// 主评论模块视图模型的接口定义.
    /// </summary>
    public interface ICommentMainModuleViewModel : IInformationFlowViewModel<ICommentItemViewModel>
    {
        /// <summary>
        /// 请求显示某评论的详情.
        /// </summary>
        event EventHandler<ICommentItemViewModel> RequestShowDetail;

        /// <summary>
        /// 排序方式集合.
        /// </summary>
        ObservableCollection<CommentSortHeader> SortCollection { get; }

        /// <summary>
        /// 改变排序方式的命令.
        /// </summary>
        IRelayCommand<CommentSortHeader> ChangeSortCommand { get; }

        /// <summary>
        /// 重置选中的评论.
        /// </summary>
        IRelayCommand ResetSelectedCommentCommand { get; }

        /// <summary>
        /// 发送评论命令.
        /// </summary>
        IAsyncRelayCommand SendCommentCommand { get; }

        /// <summary>
        /// 置顶评论.
        /// </summary>
        ICommentItemViewModel TopComment { get; }

        /// <summary>
        /// 内容是否为空.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// 当前的排序方式.
        /// </summary>
        CommentSortHeader CurrentSort { get; }

        /// <summary>
        /// 回复框提示文本.
        /// </summary>
        string ReplyTip { get; }

        /// <summary>
        /// 回复框的输入文本.
        /// </summary>
        string ReplyText { get; }

        /// <summary>
        /// 是否正在发送评论.
        /// </summary>
        bool IsSending { get; }

        /// <summary>
        /// 设置评论源Id.
        /// </summary>
        /// <param name="targetId">目标 Id.</param>
        /// <param name="type">评论区类型.</param>
        /// <param name="defaultSort">默认排序方式.</param>
        void SetTarget(string targetId, CommentType type, CommentSortType defaultSort = CommentSortType.Hot);

        /// <summary>
        /// 清除数据.
        /// </summary>
        void ClearData();
    }
}

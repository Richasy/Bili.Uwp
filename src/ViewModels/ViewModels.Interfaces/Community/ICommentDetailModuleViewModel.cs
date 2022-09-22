// Copyright (c) Richasy. All rights reserved.

using System;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Community
{
    /// <summary>
    /// 二级评论模块视图详情的接口定义.
    /// </summary>
    public interface ICommentDetailModuleViewModel : IInformationFlowViewModel<ICommentItemViewModel>
    {
        /// <summary>
        /// 请求返回到主评论视图.
        /// </summary>
        event EventHandler RequestBackToMain;

        /// <summary>
        /// 重置选中的评论.
        /// </summary>
        IRelayCommand ResetSelectedCommentCommand { get; }

        /// <summary>
        /// 发送评论命令.
        /// </summary>
        IRelayCommand SendCommentCommand { get; }

        /// <summary>
        /// 返回到上一层（主视图）的命令.
        /// </summary>
        IRelayCommand BackCommand { get; }

        /// <summary>
        /// 根评论.
        /// </summary>
        ICommentItemViewModel RootComment { get; }

        /// <summary>
        /// 是否为空.
        /// </summary>
        bool IsEmpty { get; }

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
        /// 设置根评论.
        /// </summary>
        /// <param name="rootItem">评论条目.</param>
        void SetRoot(ICommentItemViewModel rootItem);

        /// <summary>
        /// 清除数据.
        /// </summary>
        void ClearData();
    }
}

// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Bili.Models.Enums.Bili;

namespace Bili.ViewModels.Interfaces.Community
{
    /// <summary>
    /// 评论页面/模块视图模型的接口定义.
    /// </summary>
    public interface ICommentPageViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 主视图模型.
        /// </summary>
        ICommentMainModuleViewModel MainViewModel { get; }

        /// <summary>
        /// 二级视图模型.
        /// </summary>
        ICommentDetailModuleViewModel DetailViewModel { get; }

        /// <summary>
        /// 是否显示主视图.
        /// </summary>
        bool IsMainShown { get; }

        /// <summary>
        /// 是否显示二级视图.
        /// </summary>
        bool IsDetailShown { get; }

        /// <summary>
        /// 设置评论初始数据.
        /// </summary>
        /// <param name="sourceId">评论区 Id.</param>
        /// <param name="type">评论区类型.</param>
        /// <param name="sortType">评论区排序方式.</param>
        void SetData(string sourceId, CommentType type, CommentSortType sortType = CommentSortType.Hot);

        /// <summary>
        /// 清理内部数据.
        /// </summary>
        void ClearData();
    }
}

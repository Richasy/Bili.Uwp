// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Enums.Bili;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 评论页面/模块视图模型.
    /// </summary>
    public sealed partial class CommentPageViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentPageViewModel"/> class.
        /// </summary>
        /// <param name="mainViewModel">主视图模型.</param>
        /// <param name="detailViewModel">二级视图模型.</param>
        public CommentPageViewModel(
            CommentMainModuleViewModel mainViewModel,
            CommentDetailModuleViewModel detailViewModel)
        {
            MainViewModel = mainViewModel;
            DetailViewModel = detailViewModel;
            IsMainShown = true;
            IsDetailShown = false;

            MainViewModel.RequestShowDetail += OnRequestShowDetail;
            DetailViewModel.RequestBackToMain += OnRequestBackToMain;
        }

        /// <summary>
        /// 设置评论初始数据.
        /// </summary>
        /// <param name="sourceId">评论区 Id.</param>
        /// <param name="type">评论区类型.</param>
        /// <param name="sortType">评论区排序方式.</param>
        public void SetData(string sourceId, CommentType type, CommentSortType sortType = CommentSortType.Hot)
        {
            ShowMainView();
            MainViewModel.SetTarget(sourceId, type, sortType);
        }

        private void OnRequestShowDetail(object sender, CommentItemViewModel e)
        {
            IsMainShown = false;
            IsDetailShown = true;
            DetailViewModel.SetRoot(e);
        }

        private void OnRequestBackToMain(object sender, EventArgs e) => ShowMainView();

        private void ShowMainView()
        {
            IsMainShown = true;
            IsDetailShown = false;
            DetailViewModel.ClearData();
        }
    }
}

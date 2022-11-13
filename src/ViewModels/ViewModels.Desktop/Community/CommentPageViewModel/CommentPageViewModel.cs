// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Enums.Bili;
using Bili.ViewModels.Interfaces.Community;

namespace Bili.ViewModels.Desktop.Community
{
    /// <summary>
    /// 评论页面/模块视图模型.
    /// </summary>
    public sealed partial class CommentPageViewModel : ViewModelBase, ICommentPageViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentPageViewModel"/> class.
        /// </summary>
        public CommentPageViewModel(
            ICommentMainModuleViewModel mainViewModel,
            ICommentDetailModuleViewModel detailViewModel)
        {
            MainViewModel = mainViewModel;
            DetailViewModel = detailViewModel;
            IsMainShown = true;
            IsDetailShown = false;

            MainViewModel.RequestShowDetail += OnRequestShowDetail;
            DetailViewModel.RequestBackToMain += OnRequestBackToMain;
        }

        /// <inheritdoc/>
        public void SetData(string sourceId, CommentType type, CommentSortType sortType = CommentSortType.Hot)
        {
            ShowMainView();
            MainViewModel.SetTarget(sourceId, type, sortType);
        }

        /// <inheritdoc/>
        public void ClearData()
        {
            MainViewModel.ClearData();
            DetailViewModel.ClearData();
        }

        private void OnRequestShowDetail(object sender, ICommentItemViewModel e)
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

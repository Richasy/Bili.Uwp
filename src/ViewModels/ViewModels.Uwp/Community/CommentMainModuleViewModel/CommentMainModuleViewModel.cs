// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Data.Community;
using Bili.Models.Enums.Bili;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Base;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using Splat;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 主评论模块视图模型.
    /// </summary>
    public sealed partial class CommentMainModuleViewModel : InformationFlowViewModelBase<CommentItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentMainModuleViewModel"/> class.
        /// </summary>
        public CommentMainModuleViewModel(
            ICommunityProvider communityProvider,
            IResourceToolkit resourceToolkit,
            AppViewModel appViewModel,
            CoreDispatcher dispatcher)
            : base(dispatcher)
        {
            _communityProvider = communityProvider;
            _resourceToolkit = resourceToolkit;
            _appViewModel = appViewModel;

            SortCollection = new ObservableCollection<CommentSortHeader>
            {
                new CommentSortHeader(CommentSortType.Hot, _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.SortByHot)),
                new CommentSortHeader(CommentSortType.Time, _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.SortByNewest)),
            };

            CurrentSort = SortCollection.First();

            ChangeSortCommand = ReactiveCommand.Create<CommentSortHeader>(ChangeSort, outputScheduler: RxApp.MainThreadScheduler);
            ResetSelectedCommentCommand = ReactiveCommand.Create(UnselectComment, outputScheduler: RxApp.MainThreadScheduler);
            SendCommentCommand = ReactiveCommand.CreateFromTask(SendCommentAsync, outputScheduler: RxApp.MainThreadScheduler);

            _isSending = SendCommentCommand.IsExecuting.ToProperty(this, x => x.IsSending, scheduler: RxApp.MainThreadScheduler);
        }

        /// <summary>
        /// 设置评论源Id.
        /// </summary>
        /// <param name="targetId">目标 Id.</param>
        /// <param name="type">评论区类型.</param>
        /// <param name="defaultSort">默认排序方式.</param>
        internal void SetTarget(string targetId, CommentType type, CommentSortType defaultSort = CommentSortType.Hot)
        {
            Items.Clear();
            _targetId = targetId;
            _commentType = type;
            var sort = SortCollection.First(p => p.Type == defaultSort);
            CurrentSort = sort;
            InitializeCommand.Execute().Subscribe();
        }

        internal void ClearData()
        {
            Items.Clear();
            BeforeReload();
            UnselectComment();
        }

        internal void UnselectComment()
        {
            _selectedComment = null;
            ReplyTip = string.Empty;
        }

        /// <inheritdoc/>
        protected override void BeforeReload()
        {
            _isEnd = false;
            IsEmpty = false;
            TopComment = null;
            _communityProvider.ResetMainCommentsStatus();
        }

        /// <inheritdoc/>
        protected override string FormatException(string errorMsg)
            => $"{_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestReplyFailed)}\n{errorMsg}";

        /// <inheritdoc/>
        protected override async Task GetDataAsync()
        {
            if (_isEnd)
            {
                return;
            }

            var data = await _communityProvider.GetCommentsAsync(_targetId, _commentType, CurrentSort.Type);
            _isEnd = data.IsEnd;
            if (data.TopComment != null)
            {
                var top = GetItemViewModel(data.TopComment);
                TopComment = top;
            }

            foreach (var item in data.Comments)
            {
                if (!Items.Any(p => p.Information.Equals(item)))
                {
                    var vm = GetItemViewModel(item);
                    Items.Add(vm);
                }
            }

            IsEmpty = Items.Count == 0 && TopComment == null;
        }

        private void ChangeSort(CommentSortHeader sort)
        {
            CurrentSort = sort;
            ReloadCommand.Execute().Subscribe();
        }

        private async Task SendCommentAsync()
        {
            if (IsSending || string.IsNullOrEmpty(ReplyText))
            {
                return;
            }

            var content = ReplyText;
            var replyCommentId = _selectedComment == null ? "0" : _selectedComment.Information.Id;
            var result = await _communityProvider.AddCommentAsync(content, _targetId, _commentType, "0", replyCommentId);
            if (result)
            {
                ReplyText = string.Empty;
                UnselectComment();
                if (CurrentSort.Type == CommentSortType.Time)
                {
                    // 即便评论发送成功也需要等待一点时间才会显示.
                    await Task.Delay(500);
                    ReloadCommand.Execute().Subscribe();
                }
            }
            else
            {
                _appViewModel.ShowTip(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.AddReplyFailed), Models.Enums.App.InfoType.Error);
            }
        }

        private CommentItemViewModel GetItemViewModel(CommentInformation information)
        {
            var commentVM = Splat.Locator.Current.GetService<CommentItemViewModel>();
            commentVM.SetInformation(information);
            commentVM.SetDetailAction(vm =>
            {
                RequestShowDetail?.Invoke(this, vm);
            });
            commentVM.SetClickAction(vm =>
            {
                _selectedComment = vm;
                ReplyTip = string.Format(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.ReplySomeone), vm.Information.Publisher.User.Name);
            });
            return commentVM;
        }
    }
}

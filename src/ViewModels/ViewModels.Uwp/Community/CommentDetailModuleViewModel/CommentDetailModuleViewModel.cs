// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Base;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using Splat;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 二级评论模块视图详情.
    /// </summary>
    public sealed partial class CommentDetailModuleViewModel : InformationFlowViewModelBase<CommentItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentDetailModuleViewModel"/> class.
        /// </summary>
        internal CommentDetailModuleViewModel(
            ICommunityProvider communityProvider,
            IResourceToolkit resourceToolkit,
            AppViewModel appViewModel,
            CoreDispatcher dispatcher)
            : base(dispatcher)
        {
            _communityProvider = communityProvider;
            _resourceToolkit = resourceToolkit;
            _appViewModel = appViewModel;

            SendCommentCommand = ReactiveCommand.CreateFromTask(SendCommentAsync, outputScheduler: RxApp.MainThreadScheduler);
            BackCommand = ReactiveCommand.Create(Back, outputScheduler: RxApp.MainThreadScheduler);
            _isSending = SendCommentCommand.IsExecuting.ToProperty(this, x => x.IsSending, scheduler: RxApp.MainThreadScheduler);
        }

        /// <summary>
        /// 设置根评论.
        /// </summary>
        /// <param name="rootItem">评论条目.</param>
        internal void SetRoot(CommentItemViewModel rootItem)
        {
            Items.Clear();
            RootComment = GetItemViewModel(rootItem.Information);
            InitializeCommand.Execute().Subscribe();
        }

        internal void ClearData()
        {
            RootComment = null;
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
            IsEmpty = false;
            _isEnd = false;
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

            var targetId = RootComment.Information.CommentId;
            var commentType = RootComment.Information.CommentType;
            var data = await _communityProvider.GetCommentsAsync(targetId, commentType, Models.Enums.Bili.CommentSortType.Time, RootComment.Information.Id);
            _isEnd = data.IsEnd;
            foreach (var item in data.Comments)
            {
                if (!Items.Any(p => p.Information.Equals(item)))
                {
                    Items.Add(GetItemViewModel(item));
                }
            }

            IsEmpty = Items.Count == 0;
        }

        private void Back()
            => RequestBackToMain?.Invoke(this, EventArgs.Empty);

        private async Task SendCommentAsync()
        {
            if (IsSending || string.IsNullOrEmpty(ReplyText))
            {
                return;
            }

            var content = ReplyText;
            var targetId = RootComment.Information.CommentId;
            var rootId = _selectedComment == null ? RootComment.Information.Id : _selectedComment.Information.RootId;
            var replyCommentId = _selectedComment == null ? rootId : _selectedComment.Information.Id;

            var commentType = RootComment.Information.CommentType;
            var result = await _communityProvider.AddCommentAsync(content, targetId, commentType, rootId, replyCommentId);
            if (result)
            {
                ReplyText = string.Empty;
                UnselectComment();

                // 即便评论发送成功也需要等待一点时间才会显示.
                await Task.Delay(500);
                ReloadCommand.Execute().Subscribe();
            }
            else
            {
                _appViewModel.ShowTip(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.AddReplyFailed), Models.Enums.App.InfoType.Error);
            }
        }

        private CommentItemViewModel GetItemViewModel(CommentInformation information)
        {
            var vm = Splat.Locator.Current.GetService<CommentItemViewModel>();
            var highlightUserId = RootComment == null
                ? information.Publisher.User.Id
                : RootComment.Information.Publisher.User.Id;
            vm.SetInformation(information, highlightUserId);
            vm.SetClickAction(vm =>
            {
                if (vm != RootComment)
                {
                    _selectedComment = vm;
                    ReplyTip = string.Format(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.ReplySomeone), vm.Information.Publisher.User.Name);
                }
                else
                {
                    UnselectComment();
                }
            });
            vm.ReplyCountText = string.Empty;
            return vm;
        }
    }
}

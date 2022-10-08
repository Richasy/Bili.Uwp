// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Community;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Uwp.Base;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 二级评论模块视图详情.
    /// </summary>
    public sealed partial class CommentDetailModuleViewModel : InformationFlowViewModelBase<ICommentItemViewModel>, ICommentDetailModuleViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommentDetailModuleViewModel"/> class.
        /// </summary>
        public CommentDetailModuleViewModel(
            ICommunityProvider communityProvider,
            IResourceToolkit resourceToolkit,
            ICallerViewModel callerViewModel,
            CoreDispatcher dispatcher)
            : base(dispatcher)
        {
            _communityProvider = communityProvider;
            _resourceToolkit = resourceToolkit;
            _callerViewModel = callerViewModel;

            SendCommentCommand = new AsyncRelayCommand(SendCommentAsync);
            BackCommand = new RelayCommand(Back);
            ResetSelectedCommentCommand = new RelayCommand(UnselectComment);

            SendCommentCommand.IsExecuting.ToPropertyEx(this, x => x.IsSending);
        }

        /// <inheritdoc/>
        public void SetRoot(ICommentItemViewModel rootItem)
        {
            TryClear(Items);
            RootComment = GetItemViewModel(rootItem.Data);
            InitializeCommand.Execute().Subscribe();
        }

        /// <inheritdoc/>
        public void ClearData()
        {
            RootComment = null;
            TryClear(Items);
            BeforeReload();
            UnselectComment();
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

            var targetId = RootComment.Data.CommentId;
            var commentType = RootComment.Data.CommentType;
            var data = await _communityProvider.GetCommentsAsync(targetId, commentType, Models.Enums.Bili.CommentSortType.Time, RootComment.Data.Id);
            _isEnd = data.IsEnd;
            foreach (var item in data.Comments)
            {
                if (!Items.Any(p => p.Data.Equals(item)))
                {
                    Items.Add(GetItemViewModel(item));
                }
            }

            IsEmpty = Items.Count == 0;
        }

        private void Back()
            => RequestBackToMain?.Invoke(this, EventArgs.Empty);

        private void UnselectComment()
        {
            _selectedComment = null;
            ReplyTip = string.Empty;
        }

        private async Task SendCommentAsync()
        {
            if (IsSending || string.IsNullOrEmpty(ReplyText))
            {
                return;
            }

            var content = ReplyText;
            var targetId = RootComment.Data.CommentId;
            var rootId = _selectedComment == null ? RootComment.Data.Id : _selectedComment.Data.RootId;
            var replyCommentId = _selectedComment == null ? rootId : _selectedComment.Data.Id;

            var commentType = RootComment.Data.CommentType;
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
                _callerViewModel.ShowTip(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.AddReplyFailed), Models.Enums.App.InfoType.Error);
            }
        }

        private ICommentItemViewModel GetItemViewModel(CommentInformation information)
        {
            var vm = Locator.Current.GetService<ICommentItemViewModel>();
            var highlightUserId = RootComment == null
                ? information.Publisher.User.Id
                : RootComment.Data.Publisher.User.Id;
            vm.InjectData(information);
            vm.IsUserHighlight = !string.IsNullOrEmpty(highlightUserId) && information.Publisher.User.Id == highlightUserId;
            vm.SetClickAction(vm =>
            {
                if (vm != RootComment)
                {
                    _selectedComment = vm;
                    ReplyTip = string.Format(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.ReplySomeone), vm.Data.Publisher.User.Name);
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

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Bilibili.Main.Community.Reply.V1;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 评论回复单层展开详情视图模型.
    /// </summary>
    public partial class ReplyDetailViewModel : ReplyViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyDetailViewModel"/> class.
        /// </summary>
        protected ReplyDetailViewModel()
        {
            ReplyCollection = new ObservableCollection<ReplyInfo>();
            Controller.ReplyDetailIteration += OnReplyIteration;
        }

        /// <summary>
        /// 设置根评论.
        /// </summary>
        /// <param name="root">根评论.</param>
        public void SetRootReply(ReplyInfo root)
        {
            if (root != null && (RootReply == null || (RootReply != null && RootReply.Id != root.Id)))
            {
                RootReply = root;
                TargetId = ReplyModuleViewModel.Instance.TargetId;
                Type = ReplyModuleViewModel.Instance.Type;
                CurrentMode = Mode.MainListTime;
                Reset();
            }
        }

        /// <inheritdoc/>
        public override async Task RequestDataAsync()
        {
            if (!IsRequested)
            {
                await InitializeRequestAsync();
            }
            else
            {
                await DeltaRequestAsync();
            }
        }

        /// <inheritdoc/>
        public override async Task InitializeRequestAsync()
        {
            if (TargetId == 0 || _cursor == null)
            {
                throw new ArgumentException("需要先设置初始信息.");
            }

            if (!IsInitializeLoading)
            {
                IsInitializeLoading = true;
                Reset();
                _cursor.Mode = CurrentMode;
                try
                {
                    await Controller.RequestDeltailReplyListAsync(TargetId, Type, RootReply.Id, _cursor);
                    IsRequested = true;
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestReplyFailed)}\n{ex.Error?.Message ?? ex.Message}";
                }
                catch (Exception invalidEx)
                {
                    IsError = true;
                    ErrorText = invalidEx.Message;
                }

                IsInitializeLoading = false;
            }
        }

        internal async Task DeltaRequestAsync()
        {
            if (TargetId == 0 || _cursor == null)
            {
                throw new ArgumentException("需要先设置初始信息.");
            }

            if (!IsDeltaLoading && !_isCompleted)
            {
                IsDeltaLoading = true;
                await Controller.RequestDeltailReplyListAsync(TargetId, Type, RootReply.Id, _cursor);
                IsDeltaLoading = false;
            }
        }

        private void Reset()
        {
            ReplyCollection.Clear();
            _isCompleted = false;
            IsError = false;
            IsRequested = false;
            IsShowEmpty = false;
            _cursor = new CursorReq
            {
                Prev = 0,
                Next = 0,
                Mode = CurrentMode,
            };
            InitTitle();
        }

        private void OnReplyIteration(object sender, ReplyIterationEventArgs e)
        {
            if (e.TargetId == TargetId)
            {
                _cursor = new CursorReq
                {
                    Prev = 0,
                    Next = e.Cursor.Next,
                    Mode = e.Cursor.Mode,
                };

                if (e.ReplyList != null && e.ReplyList.Count > 0)
                {
                    e.ReplyList.ForEach(p => ReplyCollection.Add(p));
                }

                _isCompleted = e.Cursor.IsEnd;
                IsShowEmpty = ReplyCollection.Count == 0;
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrentMode))
            {
                InitTitle();
            }
        }
    }
}

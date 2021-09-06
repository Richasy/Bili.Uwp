// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Bilibili.Main.Community.Reply.V1;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Models.Enums.Bili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 评论回复模块视图模型.
    /// </summary>
    public partial class ReplyModuleViewModel : WebRequestViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyModuleViewModel"/> class.
        /// </summary>
        protected ReplyModuleViewModel()
        {
            ReplyCollection = new ObservableCollection<ReplyInfo>();
            TopReplyCollection = new ObservableCollection<ReplyInfo>();
            Controller.ReplyIteration += OnReplyIteration;
        }

        /// <summary>
        /// 设置初始信息.
        /// </summary>
        /// <param name="targetId">目标评论区Id.</param>
        /// <param name="type">评论区类型.</param>
        public void SetInformation(int targetId, ReplyType type)
        {
            TargetId = targetId;
            Type = type;
            Reset();
        }

        /// <summary>
        /// 请求数据.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestDataAsync()
        {
            if (_cursor.Prev == 0)
            {
                await InitializeRequstAsync();
            }
            else
            {
                await DeltaRequestAsync();
            }
        }

        /// <summary>
        /// 初始化请求.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeRequstAsync()
        {
            if (TargetId == 0 || _cursor == null)
            {
                throw new ArgumentException("需要先设置初始信息.");
            }

            if (!IsInitializeLoading)
            {
                IsInitializeLoading = true;
                _cursor.Mode = CurrentMode;
                try
                {
                    await Controller.RequestMainReplyListAsync(TargetId, Type, _cursor);
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
                IsInitializeLoading = true;
                await Controller.RequestMainReplyListAsync(TargetId, Type, _cursor);
                IsInitializeLoading = false;
            }
        }

        private void Reset()
        {
            _isCompleted = false;
            IsShowEmpty = false;
            CurrentMode = Mode.MainListHot;
            ReplyCollection.Clear();
            TopReplyCollection.Clear();
            _cursor = new CursorReq
            {
                Prev = 0,
                Next = 0,
                Mode = CurrentMode,
            };
        }

        private void OnReplyIteration(object sender, ReplyIterationEventArgs e)
        {
            if (e.TargetId == TargetId)
            {
                _cursor = new CursorReq
                {
                    Prev = e.Cursor.Prev,
                    Next = e.Cursor.Next,
                    Mode = e.Cursor.Mode,
                };

                if (e.TopReply != null)
                {
                    TopReplyCollection.Add(e.TopReply);
                }

                if (e.ReplyList != null && e.ReplyList.Count > 0)
                {
                    e.ReplyList.ForEach(p => ReplyCollection.Add(p));
                }

                _isCompleted = e.Cursor.IsEnd;
                IsShowEmpty = ReplyCollection.Count == 0;
            }
        }
    }
}

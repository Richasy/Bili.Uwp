// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
    public partial class ReplyModuleViewModel : ReplyViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyModuleViewModel"/> class.
        /// </summary>
        protected ReplyModuleViewModel()
        {
            ReplyCollection = new ObservableCollection<ReplyInfo>();
            Controller.ReplyIteration += OnReplyIteration;
            PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        /// 设置初始信息.
        /// </summary>
        /// <param name="targetId">目标评论区Id.</param>
        /// <param name="type">评论区类型.</param>
        /// <param name="currentMode">当前显示模式.</param>
        public void SetInformation(long targetId, ReplyType type, Mode currentMode = Mode.MainListHot)
        {
            TargetId = targetId;
            Type = type;
            CurrentMode = currentMode;
            Reset();
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
            if (TargetId == 0 || _cursor == null || Type == ReplyType.None)
            {
                IsShowEmpty = true;
                return;
            }

            if (!IsInitializeLoading)
            {
                IsInitializeLoading = true;
                Reset();
                IsShowEmpty = false;
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

        /// <summary>
        /// 重置.
        /// </summary>
        public void Reset()
        {
            _isCompleted = false;
            IsShowEmpty = true;
            IsRequested = false;
            IsError = false;
            ReplyCollection.Clear();
            _cursor = new CursorReq
            {
                Prev = 0,
                Next = 0,
                Mode = CurrentMode,
            };
        }

        /// <summary>
        /// 给评论点赞/取消点赞.
        /// </summary>
        /// <param name="isLike">是否点赞.</param>
        /// <param name="replyId">评论ID.</param>
        /// <returns>结果.</returns>
        public Task<bool> LikeReplyAysnc(bool isLike, long replyId)
        {
            return Controller.LikeReplyAsync(isLike, replyId, TargetId, Type);
        }

        /// <summary>
        /// 添加评论.
        /// </summary>
        /// <param name="message">评论内容.</param>
        /// <param name="rootId">根评论Id.</param>
        /// <param name="parentId">正在回复的评论Id.</param>
        /// <returns>发布结果.</returns>
        public async Task<bool> AddReplyAsync(string message, long rootId, long parentId)
        {
            var result = await Controller.AddReplyAsync(message, TargetId, Type, rootId, parentId);
            if (!result)
            {
                AppViewModel.Instance.ShowTip(ResourceToolkit.GetLocaleString(LanguageNames.AddReplyFailed), Models.Enums.App.InfoType.Error);
            }

            return result;
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
                await Controller.RequestMainReplyListAsync(TargetId, Type, _cursor);
                IsDeltaLoading = false;
            }
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

                if (e.TopReply != null && !ReplyCollection.Any(p => p.Id == e.TopReply.Id))
                {
                    ReplyCollection.Add(e.TopReply);
                }

                if (e.ReplyList != null && e.ReplyList.Count > 0)
                {
                    foreach (var item in e.ReplyList)
                    {
                        if (!ReplyCollection.Any(p => p.Id == item.Id))
                        {
                            ReplyCollection.Add(item);
                        }
                    }
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

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Models.Enums.App;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 消息模块视图模型.
    /// </summary>
    public partial class MessageModuleViewModel : WebRequestViewModelBase, IDeltaRequestViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageModuleViewModel"/> class.
        /// </summary>
        protected MessageModuleViewModel()
        {
            LikeMessageCollection = new ObservableCollection<LikeMessageItem>();
            AtMessageCollection = new ObservableCollection<AtMessageItem>();
            ReplyMessageCollection = new ObservableCollection<ReplyMessageItem>();
            Controller.MessageIteration += OnMessageIteration;
            CurrentType = MessageType.Reply;

            Reset(true);
        }

        /// <inheritdoc/>
        public async Task InitializeRequestAsync()
        {
            if (!IsInitializeLoading)
            {
                Reset();
                IsInitializeLoading = true;
                try
                {
                    await RequestMessageForCurrentTypeAsync();
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{ResourceToolkit.GetLocaleString(LanguageNames.RequestMessageFailed)}\n{ex.Error?.Message ?? ex.Message}";
                }
                catch (Exception invalidEx)
                {
                    IsError = true;
                    ErrorText = invalidEx.Message;
                }

                IsInitializeLoading = false;
            }
        }

        /// <inheritdoc/>
        public async Task RequestDataAsync()
        {
            var cursor = GetCursor();
            if (cursor.Time == 0)
            {
                await InitializeRequestAsync();
            }
            else
            {
                await DeltaReqeustAsync();
            }
        }

        /// <summary>
        /// 当前消息类型是否已经请求过.
        /// </summary>
        /// <returns>是否已请求.</returns>
        public bool IsCurrentTypeRequested()
        {
            var cursor = GetCursor();
            return cursor.Id != 0;
        }

        internal async Task DeltaReqeustAsync()
        {
            var cursor = GetCursor();
            if (!IsDeltaLoading && !cursor.IsEnd)
            {
                IsDeltaLoading = true;
                await RequestMessageForCurrentTypeAsync();
                IsDeltaLoading = false;
            }
        }

        private void Reset(bool isClearAll = false)
        {
            if (CurrentType == MessageType.Like || isClearAll)
            {
                LikeMessageCollection.Clear();
                _likeCursor = new MessageCursor { Id = 0, Time = 0, IsEnd = false };
                IsShowLikeEmpty = false;
            }

            if (CurrentType == MessageType.At || isClearAll)
            {
                AtMessageCollection.Clear();
                _atCursor = new MessageCursor { Id = 0, Time = 0, IsEnd = false };
                IsShowAtEmpty = false;
            }

            if (CurrentType == MessageType.Reply || isClearAll)
            {
                ReplyMessageCollection.Clear();
                _replyCursor = new MessageCursor { Id = 0, Time = 0, IsEnd = false };
                IsShowReplyEmpty = false;
            }

            IsError = false;
            IsRequested = false;
            IsDeltaLoading = false;
            IsInitializeLoading = false;
        }

        private MessageCursor GetCursor()
        {
            switch (CurrentType)
            {
                case MessageType.Like:
                    return _likeCursor;
                case MessageType.At:
                    return _atCursor;
                case MessageType.Reply:
                    return _replyCursor;
                default:
                    break;
            }

            return null;
        }

        private async Task RequestMessageForCurrentTypeAsync()
        {
            long cursorId = 0;
            long cursorTime = 0;
            switch (CurrentType)
            {
                case MessageType.Like:
                    cursorId = _likeCursor.Id;
                    cursorTime = _likeCursor.Time;
                    break;
                case MessageType.At:
                    cursorId = _atCursor.Id;
                    cursorTime = _atCursor.Time;
                    break;
                case MessageType.Reply:
                    cursorId = _replyCursor.Id;
                    cursorTime = _replyCursor.Time;
                    break;
                default:
                    break;
            }

            await Controller.RequestMessageAsync(CurrentType, cursorId, cursorTime);
        }

        private void OnMessageIteration(object sender, MessageIterationEventArgs e)
        {
            switch (e.Type)
            {
                case MessageType.Like:
                    _likeCursor = e.Cursor;
                    foreach (var item in e.Items)
                    {
                        LikeMessageCollection.Add(item as LikeMessageItem);
                    }

                    IsShowLikeEmpty = LikeMessageCollection.Count == 0;
                    break;
                case MessageType.At:
                    _atCursor = e.Cursor;
                    foreach (var item in e.Items)
                    {
                        AtMessageCollection.Add(item as AtMessageItem);
                    }

                    IsShowAtEmpty = AtMessageCollection.Count == 0;
                    break;
                case MessageType.Reply:
                    _replyCursor = e.Cursor;
                    foreach (var item in e.Items)
                    {
                        ReplyMessageCollection.Add(item as ReplyMessageItem);
                    }

                    IsShowReplyEmpty = ReplyMessageCollection.Count == 0;
                    break;
                default:
                    break;
            }
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums.App;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 消息模块视图模型.
    /// </summary>
    public partial class MessageModuleViewModel
    {
        private MessageCursor _likeCursor;
        private MessageCursor _atCursor;
        private MessageCursor _replyCursor;

        /// <summary>
        /// 实例.
        /// </summary>
        public static MessageModuleViewModel Instance { get; } = new Lazy<MessageModuleViewModel>(() => new MessageModuleViewModel()).Value;

        /// <summary>
        /// 当前显示的消息类型.
        /// </summary>
        [Reactive]
        public MessageType CurrentType { get; set; }

        /// <summary>
        /// 点赞消息集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<LikeMessageItem> LikeMessageCollection { get; set; }

        /// <summary>
        /// @我的消息集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<AtMessageItem> AtMessageCollection { get; set; }

        /// <summary>
        /// 回复消息集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<ReplyMessageItem> ReplyMessageCollection { get; set; }

        /// <summary>
        /// 是否显示无点赞提示.
        /// </summary>
        [Reactive]
        public bool IsShowLikeEmpty { get; set; }

        /// <summary>
        /// 是否显示无At提示.
        /// </summary>
        [Reactive]
        public bool IsShowAtEmpty { get; set; }

        /// <summary>
        /// 是否显示无回复提示.
        /// </summary>
        [Reactive]
        public bool IsShowReplyEmpty { get; set; }

        /// <summary>
        /// 新的点赞消息数.
        /// </summary>
        [Reactive]
        public int NewLikeMessageCount { get; set; }

        /// <summary>
        /// 新的@消息数.
        /// </summary>
        [Reactive]
        public int NewAtMessageCount { get; set; }

        /// <summary>
        /// 新的回复消息数.
        /// </summary>
        [Reactive]
        public int NewReplyMessageCount { get; set; }
    }
}

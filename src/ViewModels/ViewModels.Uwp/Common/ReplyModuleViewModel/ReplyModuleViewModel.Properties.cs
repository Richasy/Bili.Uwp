// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bilibili.Main.Community.Reply.V1;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.Enums.Bili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 评论回复模块视图模型.
    /// </summary>
    public partial class ReplyModuleViewModel
    {
        private CursorReq _cursor;
        private bool _isCompleted;

        /// <summary>
        /// 实例.
        /// </summary>
        public static ReplyModuleViewModel Instance { get; } = new Lazy<ReplyModuleViewModel>(() => new ReplyModuleViewModel()).Value;

        /// <summary>
        /// 目标Id.
        /// </summary>
        public int TargetId { get; set; }

        /// <summary>
        /// 评论区类型.
        /// </summary>
        public ReplyType Type { get; set; }

        /// <summary>
        /// 当前排序模式.
        /// </summary>
        [Reactive]
        public Mode CurrentMode { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <summary>
        /// 评论集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<ReplyInfo> ReplyCollection { get; set; }

        /// <summary>
        /// 置顶评论集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<ReplyInfo> TopReplyCollection { get; set; }

        /// <summary>
        /// 是否显示为空.
        /// </summary>
        [Reactive]
        public bool IsShowEmpty { get; set; }
    }
}

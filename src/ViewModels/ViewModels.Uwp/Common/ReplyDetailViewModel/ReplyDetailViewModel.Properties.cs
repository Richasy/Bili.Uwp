// Copyright (c) Richasy. All rights reserved.

using System;
using Bilibili.Main.Community.Reply.V1;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 评论回复单层展开详情视图模型.
    /// </summary>
    public partial class ReplyDetailViewModel
    {
        private CursorReq _cursor;
        private bool _isCompleted;

        /// <summary>
        /// 实例.
        /// </summary>
        public static ReplyDetailViewModel Instance { get; } = new Lazy<ReplyDetailViewModel>(() => new ReplyDetailViewModel()).Value;

        /// <summary>
        /// 根评论.
        /// </summary>
        [Reactive]
        public ReplyInfo RootReply { get; set; }
    }
}

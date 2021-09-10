// Copyright (c) Richasy. All rights reserved.

using System;
using Bilibili.Main.Community.Reply.V1;

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
    }
}

// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Bilibili.Main.Community.Reply.V1;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 评论回复更新事件参数.
    /// </summary>
    public class ReplyIterationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyIterationEventArgs"/> class.
        /// </summary>
        /// <param name="reply">评论响应结果.</param>
        /// <param name="targetId">目标区Id.</param>
        public ReplyIterationEventArgs(MainListReply reply, long targetId)
        {
            TargetId = targetId;
            Cursor = reply.Cursor;
            ReplyList = reply.Replies.ToList();
            TopReply = reply.UpTop ?? reply.VoteTop ?? reply.AdminTop;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReplyIterationEventArgs"/> class.
        /// </summary>
        /// <param name="reply">评论响应结果.</param>
        /// <param name="targetId">目标区Id.</param>
        public ReplyIterationEventArgs(DetailListReply reply, long targetId)
        {
            TargetId = targetId;
            Cursor = reply.Cursor;
            ReplyList = reply.Root.Replies.ToList();
            RootId = reply.Root.Id;
        }

        /// <summary>
        /// 目标区Id.
        /// </summary>
        public long TargetId { get; set; }

        /// <summary>
        /// 游标.
        /// </summary>
        public CursorReply Cursor { get; set; }

        /// <summary>
        /// 评论列表.
        /// </summary>
        public List<ReplyInfo> ReplyList { get; set; }

        /// <summary>
        /// 置顶评论.
        /// </summary>
        public ReplyInfo TopReply { get; set; }

        /// <summary>
        /// 根评论Id.
        /// </summary>
        public long RootId { get; set; }
    }
}

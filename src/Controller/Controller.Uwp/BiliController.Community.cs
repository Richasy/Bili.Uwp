﻿// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bilibili.Main.Community.Reply.V1;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.Enums.Bili;

namespace Richasy.Bili.Controller.Uwp
{
    /// <summary>
    /// 社区交互部分.
    /// </summary>
    public partial class BiliController
    {
        /// <summary>
        /// 请求评论列表.
        /// </summary>
        /// <param name="targetId">目标评论区Id.</param>
        /// <param name="type">评论区类型.</param>
        /// <param name="cursor">游标.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestMainReplyListAsync(int targetId, ReplyType type, CursorReq cursor)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var response = await _communityProvider.GetReplyMainListAsync(targetId, type, cursor);
                var args = new ReplyIterationEventArgs(response, targetId);
                ReplyIteration?.Invoke(this, args);
            }
            catch (System.Exception)
            {
                if (cursor.Next == 0)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 获取单层评论详情列表.
        /// </summary>
        /// <param name="targetId">目标评论区Id.</param>
        /// <param name="type">评论区类型.</param>
        /// <param name="rootId">根评论Id.</param>
        /// <param name="cursor">游标.</param>
        /// <returns>评论列表响应.</returns>
        public async Task<DetailListReply> RequestDeltailReplyListAsync(int targetId, ReplyType type, long rootId, CursorReq cursor)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var response = await _communityProvider.GetReplyDetailListAsync(targetId, type, rootId, cursor);
                var args = new ReplyIterationEventArgs(response, targetId);
                ReplyDetailIteration?.Invoke(this, args);
            }
            catch (System.Exception)
            {
                if (cursor.Next == 0)
                {
                    throw;
                }
            }

            return null;
        }
    }
}
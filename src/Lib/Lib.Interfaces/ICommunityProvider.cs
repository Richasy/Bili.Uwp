// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bilibili.Main.Community.Reply.V1;
using Richasy.Bili.Models.Enums.Bili;

namespace Richasy.Bili.Lib.Interfaces
{
    /// <summary>
    /// 社区交互处理工具.
    /// </summary>
    public interface ICommunityProvider
    {
        /// <summary>
        /// 获取评论列表.
        /// </summary>
        /// <param name="targetId">目标Id.</param>
        /// <param name="type">评论区类型.</param>
        /// <param name="cursor">游标.</param>
        /// <returns>评论列表响应.</returns>
        Task<MainListReply> GetReplyMainListAsync(int targetId, ReplyType type, CursorReq cursor);

        /// <summary>
        /// 获取单层评论详情列表.
        /// </summary>
        /// <param name="targetId">目标评论区Id.</param>
        /// <param name="type">评论区类型.</param>
        /// <param name="rootId">根评论Id.</param>
        /// <param name="cursor">游标.</param>
        /// <returns>评论列表响应.</returns>
        Task<DetailListReply> GetReplyDetailListAsync(int targetId, ReplyType type, long rootId, CursorReq cursor);
    }
}

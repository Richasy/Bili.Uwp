// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.Enums.Bili;
using Bilibili.Main.Community.Reply.V1;

namespace Bili.Controller.Uwp
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
        public async Task RequestMainReplyListAsync(long targetId, CommentType type, CursorReq cursor)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var response = await _communityProvider.GetReplyMainListAsync(targetId, type, cursor);
                var args = new ReplyIterationEventArgs(response, targetId);
                ReplyIteration?.Invoke(this, args);
            }
            catch (System.Exception ex)
            {
                _loggerModule.LogError(ex, cursor.Next > 0);
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
        public async Task<DetailListReply> RequestDeltailReplyListAsync(long targetId, CommentType type, long rootId, CursorReq cursor)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                var response = await _communityProvider.GetReplyDetailListAsync(targetId, type, rootId, cursor);
                var args = new ReplyIterationEventArgs(response, targetId);
                ReplyDetailIteration?.Invoke(this, args);
            }
            catch (System.Exception ex)
            {
                _loggerModule.LogError(ex, cursor.Next > 0);
                if (cursor.Next == 0)
                {
                    throw;
                }
            }

            return null;
        }

        /// <summary>
        /// 给评论点赞/取消点赞.
        /// </summary>
        /// <param name="isLike">是否点赞.</param>
        /// <param name="replyId">评论Id.</param>
        /// <param name="targetId">目标评论区Id.</param>
        /// <param name="type">评论区类型.</param>
        /// <returns>结果.</returns>
        public async Task<bool> LikeReplyAsync(bool isLike, long replyId, long targetId, CommentType type)
        {
            try
            {
                return await _communityProvider.LikeReplyAsync(isLike, replyId, targetId, type);
            }
            catch (System.Exception ex)
            {
                _loggerModule.LogError(ex, true);
                return false;
            }
        }

        /// <summary>
        /// 添加评论.
        /// </summary>
        /// <param name="message">评论内容.</param>
        /// <param name="targetId">评论区Id.</param>
        /// <param name="type">评论区类型.</param>
        /// <param name="rootId">根评论Id.</param>
        /// <param name="parentId">正在回复的评论Id.</param>
        /// <returns>发布结果.</returns>
        public async Task<bool> AddReplyAsync(string message, long targetId, CommentType type, long rootId, long parentId)
        {
            try
            {
                return await _communityProvider.AddReplyAsync(message, targetId, type, rootId, parentId);
            }
            catch (System.Exception ex)
            {
                _loggerModule.LogError(ex, true);
                return false;
            }
        }
    }
}

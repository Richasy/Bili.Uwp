// Copyright (c) Richasy. All rights reserved.

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
        public async Task RequestMainReplyListAsync(long targetId, ReplyType type, CursorReq cursor)
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
        public async Task<DetailListReply> RequestDeltailReplyListAsync(long targetId, ReplyType type, long rootId, CursorReq cursor)
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
        public async Task<bool> LikeReplyAsync(bool isLike, long replyId, long targetId, ReplyType type)
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
        public async Task<bool> AddReplyAsync(string message, long targetId, ReplyType type, long rootId, long parentId)
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

        /// <summary>
        /// 请求动态视频列表.
        /// </summary>
        /// <param name="offset">偏移值.</param>
        /// <param name="baseLine">基线值.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestDynamicVideoListAsync(string offset, string baseLine)
        {
            ThrowWhenNetworkUnavaliable();
            try
            {
                var reply = await _communityProvider.GetDynamicVideoListAsync(offset, baseLine);
                var args = new DynamicVideoIterationEventArgs(reply);
                DynamicVideoIteration?.Invoke(this, args);
            }
            catch (System.Exception ex)
            {
                _loggerModule.LogError(ex, !string.IsNullOrEmpty(offset));
                if (string.IsNullOrEmpty(offset))
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 点赞/取消点赞动态.
        /// </summary>
        /// <param name="dynamicId">动态Id.</param>
        /// <param name="isLike">是否点赞.</param>
        /// <param name="userId">用户Id.</param>
        /// <param name="rid">扩展数据标识.</param>
        /// <returns>是否操作成功.</returns>
        public Task<bool> LikeDynamicAsync(string dynamicId, bool isLike, long userId, string rid)
            => _communityProvider.LikeDynamicAsync(dynamicId, isLike, userId, rid);
    }
}

// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bili.Models.Data.Community;
using Bili.Models.Data.Dynamic;
using Bili.Models.Enums.Bili;

namespace Bili.Lib.Interfaces
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
        /// <param name="sort">排序方式.</param>
        /// <returns>评论列表响应.</returns>
        Task<CommentView> GetCommentsAsync(string targetId, CommentType type, CommentSortType sort);

        /// <summary>
        /// 获取单层评论详情列表.
        /// </summary>
        /// <param name="targetId">目标评论区Id.</param>
        /// <param name="type">评论区类型.</param>
        /// <param name="sort">排序方式.</param>
        /// <param name="rootId">根评论Id.</param>
        /// <returns>评论列表响应.</returns>
        Task<CommentView> GetCommentsAsync(string targetId, CommentType type, CommentSortType sort, string rootId);

        /// <summary>
        /// 给评论点赞/取消点赞.
        /// </summary>
        /// <param name="isLike">是否点赞.</param>
        /// <param name="replyId">评论Id.</param>
        /// <param name="targetId">目标评论区Id.</param>
        /// <param name="type">评论区类型.</param>
        /// <returns>结果.</returns>
        Task<bool> LikeCommentAsync(bool isLike, string replyId, string targetId, CommentType type);

        /// <summary>
        /// 添加评论.
        /// </summary>
        /// <param name="message">评论内容.</param>
        /// <param name="targetId">评论区Id.</param>
        /// <param name="type">评论区类型.</param>
        /// <param name="rootId">根评论Id.</param>
        /// <param name="parentId">正在回复的评论Id.</param>
        /// <returns>发布结果.</returns>
        Task<bool> AddCommentAsync(string message, string targetId, CommentType type, string rootId, string parentId);

        /// <summary>
        /// 获取综合动态列表.
        /// </summary>
        /// <returns>综合动态响应.</returns>
        Task<DynamicView> GetDynamicComprehensiveListAsync();

        /// <summary>
        /// 获取视频动态列表.
        /// </summary>
        /// <returns>视频动态响应.</returns>
        Task<DynamicView> GetDynamicVideoListAsync();

        /// <summary>
        /// 点赞/取消点赞动态.
        /// </summary>
        /// <param name="dynamicId">动态Id.</param>
        /// <param name="isLike">是否点赞.</param>
        /// <param name="userId">用户Id.</param>
        /// <param name="rid">扩展数据标识.</param>
        /// <returns>是否操作成功.</returns>
        Task<bool> LikeDynamicAsync(string dynamicId, bool isLike, string userId, string rid);

        /// <summary>
        /// 重置视频动态请求状态.
        /// </summary>
        void ResetVideoDynamicStatus();

        /// <summary>
        /// 重置综合动态请求状态.
        /// </summary>
        void ResetComprehensiveDynamicStatus();

        /// <summary>
        /// 清除评论区请求状态.
        /// </summary>
        void ResetMainCommentsStatus();

        /// <summary>
        /// 清除评论详情请求状态.
        /// </summary>
        void ResetDetailCommentsStatus();
    }
}

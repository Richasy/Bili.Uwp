// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Community;
using Bilibili.Main.Community.Reply.V1;

namespace Bili.Adapter.Interfaces
{
    /// <summary>
    /// 评论数据适配器的接口定义.
    /// </summary>
    public interface ICommentAdapter
    {
        /// <summary>
        /// 将回复内容 <see cref="ReplyInfo"/> 转换为评论信息.
        /// </summary>
        /// <param name="info">回复内容.</param>
        /// <returns><see cref="CommentInformation"/>.</returns>
        CommentInformation ConvertToCommentInformation(ReplyInfo info);

        /// <summary>
        /// 将主评论响应 <see cref="MainListReply"/> 转换为评论视图.
        /// </summary>
        /// <param name="reply">主评论响应.</param>
        /// <param name="targetId">评论区Id.</param>
        /// <returns><see cref="CommentView"/>.</returns>
        CommentView ConvertToCommentView(MainListReply reply, string targetId);

        /// <summary>
        /// 将二级评论响应 <see cref="DetailListReply"/> 转换为评论视图.
        /// </summary>
        /// <param name="reply">二级评论响应.</param>
        /// <param name="targetId">目标评论Id.</param>
        /// <returns><see cref="CommentView"/>.</returns>
        CommentView ConvertToCommentView(DetailListReply reply, string targetId);
    }
}

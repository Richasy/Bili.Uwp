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
    }
}

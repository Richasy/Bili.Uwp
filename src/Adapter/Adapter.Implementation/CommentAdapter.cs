// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using Bili.Adapter.Interfaces;
using Bili.Models.Data.Community;
using Bilibili.Main.Community.Reply.V1;

namespace Bili.Adapter
{
    /// <summary>
    /// 评论数据适配器.
    /// </summary>
    public sealed class CommentAdapter : ICommentAdapter
    {
        private readonly IUserAdapter _userAdapter;
        private readonly IImageAdapter _imageAdapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommentAdapter"/> class.
        /// </summary>
        /// <param name="userAdapter">用户数据适配器.</param>
        /// <param name="imageAdapter">图片数据适配器.</param>
        public CommentAdapter(
            IUserAdapter userAdapter,
            IImageAdapter imageAdapter)
        {
            _userAdapter = userAdapter;
            _imageAdapter = imageAdapter;
        }

        /// <inheritdoc/>
        public CommentInformation ConvertToCommentInformation(ReplyInfo info)
        {
            var id = info.Id.ToString();
            var rootId = info.Root.ToString();
            var isTop = info.ReplyControl.IsAdminTop || info.ReplyControl.IsUpTop;
            var publishTime = DateTimeOffset.FromUnixTimeSeconds(info.Ctime).DateTime;
            var user = _userAdapter.ConvertToAccountInformation(info.Member);
            var communityInfo = new CommentCommunityInformation(id, info.Like, Convert.ToInt32(info.Count), info.ReplyControl.Action == 1);
            var content = _imageAdapter.ConvertToEmoteText(info.Content);
            return new CommentInformation(id, content, rootId, isTop, user, publishTime, communityInfo);
        }

        /// <inheritdoc/>
        public CommentView ConvertToCommentView(MainListReply reply, string targetId)
        {
            var comments = reply.Replies.Select(p => ConvertToCommentInformation(p)).ToList();
            var top = reply.UpTop ?? reply.VoteTop;
            var topComment = top == null
                ? null
                : ConvertToCommentInformation(top);
            var isEnd = reply.Cursor.IsEnd;
            return new CommentView(comments, targetId, topComment, isEnd);
        }

        /// <inheritdoc/>
        public CommentView ConvertToCommentView(DetailListReply reply, string targetId)
        {
            var comments = reply.Root.Replies.Select(p => ConvertToCommentInformation(p)).ToList();
            var topComment = ConvertToCommentInformation(reply.Root);
            var isEnd = reply.Cursor.IsEnd;
            return new CommentView(comments, targetId, topComment, isEnd);
        }
    }
}

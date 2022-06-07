// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Adapter.Interfaces;
using Bili.Models.Data.Community;

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
        public CommentInformation ConvertToCommentInformation(Bilibili.Main.Community.Reply.V1.ReplyInfo info)
        {
            var id = info.Id.ToString();
            var rootId = info.Root.ToString();
            var isTop = info.ReplyControl.IsAdminTop || info.ReplyControl.IsUpTop;
            var publishTime = DateTimeOffset.FromUnixTimeSeconds(info.Ctime).ToLocalTime().DateTime;
            var user = _userAdapter.ConvertToAccountInformation(info.Member);
            var communityInfo = new CommentCommunityInformation(id, info.Like, Convert.ToInt32(info.Count), info.ReplyControl.Action == 1);
            var content = _imageAdapter.ConvertToEmoteText(info.Content);
            return new CommentInformation(id, content, rootId, isTop, user, publishTime, communityInfo);
        }
    }
}

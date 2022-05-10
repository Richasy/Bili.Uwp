// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Adapter.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.User;
using Bili.Models.Enums.App;
using Bilibili.App.Archive.V1;
using Bilibili.App.View.V1;

namespace Bili.Adapter
{
    /// <summary>
    /// 用户资料适配器，将来自 BiliBili 的用户数据转换为 <see cref="UserProfile"/> , <see cref="PublisherProfile"/> 或 <see cref="AccountInformation"/>.
    /// </summary>
    public sealed class UserAdapter : IUserAdapter
    {
        private readonly IImageAdapter _imageAdapter;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAdapter"/> class.
        /// </summary>
        /// <param name="imageAdapter">图片适配器.</param>
        public UserAdapter(IImageAdapter imageAdapter)
            => _imageAdapter = imageAdapter;

        /// <inheritdoc/>
        public AccountInformation ConvertToAccountInformation(MyInfo myInfo, AvatarSize avatarSize)
        {
            var user = ConvertToUserProfile(myInfo.Mid, myInfo.Name, myInfo.Avatar, avatarSize);
            return new AccountInformation(user, myInfo.Sign, myInfo.Level, myInfo.VIP.Status == 1);
        }

        /// <inheritdoc/>
        public AccountInformation ConvertToAccountInformation(Mine myInfo, AvatarSize avatarSize)
        {
            var user = ConvertToUserProfile(myInfo.Mid, myInfo.Name, myInfo.Avatar, avatarSize);
            return new AccountInformation(user, string.Empty, myInfo.Level, false);
        }

        /// <inheritdoc/>
        public AccountInformation ConvertToAccountInformation(UserSpaceInformation spaceInfo, AvatarSize avatarSize)
        {
            var user = ConvertToUserProfile(Convert.ToInt32(spaceInfo.UserId), spaceInfo.UserName, spaceInfo.Avatar, avatarSize);
            return new AccountInformation(user, spaceInfo.Sign, spaceInfo.LevelInformation.CurrentLevel, spaceInfo.Vip.Status == 1);
        }

        /// <inheritdoc/>
        public PublisherProfile ConvertToPublisherProfile(PublisherInfo publisher, AvatarSize avatarSize)
        {
            var user = ConvertToUserProfile(publisher.Mid, publisher.Publisher, publisher.PublisherAvatar, avatarSize);
            return new PublisherProfile(user);
        }

        /// <inheritdoc/>
        public PublisherProfile ConvertToPublisherProfile(Staff staff, AvatarSize avatarSize)
        {
            var user = ConvertToUserProfile(Convert.ToInt32(staff.Mid), staff.Name, staff.Face, avatarSize);
            return new PublisherProfile(user, staff.Title);
        }

        /// <inheritdoc/>
        public PublisherProfile ConvertToPublisherProfile(RecommendAvatar avatar, AvatarSize avatarSize = AvatarSize.Size48)
        {
            var user = ConvertToUserProfile(avatar.UserId, avatar.UserName, avatar.Cover, avatarSize);
            return new PublisherProfile(user);
        }

        /// <inheritdoc/>
        public PublisherProfile ConvertToPublisherProfile(Author author, AvatarSize avatarSize = AvatarSize.Size32)
        {
            var user = ConvertToUserProfile(Convert.ToInt32(author.Mid), author.Name, author.Face, avatarSize);
            return new PublisherProfile(user);
        }

        /// <inheritdoc/>
        public UserProfile ConvertToUserProfile(int userId, string userName, string avatar, AvatarSize avatarSize)
        {
            var size = int.Parse(avatarSize.ToString().Replace("Size", string.Empty));
            var image = _imageAdapter.ConvertToImage(avatar, size, size);
            var profile = new UserProfile(userId.ToString(), userName, image);
            return profile;
        }
    }
}

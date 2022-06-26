// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Text.RegularExpressions;
using Bili.Adapter.Interfaces;
using Bili.Models.BiliBili;
using Bili.Models.Data.User;
using Bili.Models.Enums.App;
using Bili.Toolkit.Interfaces;
using Bilibili.App.Archive.V1;
using Bilibili.App.View.V1;
using Bilibili.Main.Community.Reply.V1;

namespace Bili.Adapter
{
    /// <summary>
    /// 用户资料适配器，将来自 BiliBili 的用户数据转换为 <see cref="UserProfile"/> , <see cref="RoleProfile"/> 或 <see cref="AccountInformation"/>.
    /// </summary>
    public sealed class UserAdapter : IUserAdapter
    {
        private readonly IImageAdapter _imageAdapter;
        private readonly ICommunityAdapter _communityAdapter;
        private readonly ITextToolkit _textToolkit;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserAdapter"/> class.
        /// </summary>
        /// <param name="imageAdapter">图片适配器.</param>
        /// <param name="communityAdapter">社区数据适配器.</param>
        /// <param name="textToolkit">文本工具.</param>
        public UserAdapter(
            IImageAdapter imageAdapter,
            ICommunityAdapter communityAdapter,
            ITextToolkit textToolkit)
        {
            _imageAdapter = imageAdapter;
            _communityAdapter = communityAdapter;
            _textToolkit = textToolkit;
        }

        /// <inheritdoc/>
        public AccountInformation ConvertToAccountInformation(MyInfo myInfo, AvatarSize avatarSize)
        {
            var user = ConvertToUserProfile(myInfo.Mid, myInfo.Name, myInfo.Avatar, avatarSize);
            var communityInfo = _communityAdapter.ConvertToUserCommunityInformation(myInfo);
            return new AccountInformation(
                user,
                myInfo.Sign,
                myInfo.Level,
                myInfo.VIP.Status == 1,
                communityInfo);
        }

        /// <inheritdoc/>
        public AccountInformation ConvertToAccountInformation(Mine myInfo, AvatarSize avatarSize)
        {
            var user = ConvertToUserProfile(myInfo.Mid, myInfo.Name, myInfo.Avatar, avatarSize);
            var communityInfo = _communityAdapter.ConvertToUserCommunityInformation(myInfo);
            return new AccountInformation(
                user,
                string.Empty,
                myInfo.Level,
                false,
                communityInfo);
        }

        /// <inheritdoc/>
        public AccountInformation ConvertToAccountInformation(UserSpaceInformation spaceInfo, AvatarSize avatarSize)
        {
            var user = ConvertToUserProfile(Convert.ToInt32(spaceInfo.UserId), spaceInfo.UserName, spaceInfo.Avatar, avatarSize);
            var communityInfo = _communityAdapter.ConvertToUserCommunityInformation(spaceInfo);
            return new AccountInformation(
                user,
                spaceInfo.Sign,
                spaceInfo.LevelInformation.CurrentLevel,
                spaceInfo.Vip.Status == 1,
                communityInfo);
        }

        /// <inheritdoc/>
        public AccountInformation ConvertToAccountInformation(RelatedUser user, AvatarSize avatarSize = AvatarSize.Size64)
        {
            var profile = ConvertToUserProfile(user.Mid, user.Name, user.Avatar, avatarSize);
            var communityInfo = _communityAdapter.ConvertToUserCommunityInformation(user);
            return new AccountInformation(
                profile,
                user.Sign,
                -1,
                user.Vip.Status == 1,
                communityInfo);
        }

        /// <inheritdoc/>
        public AccountInformation ConvertToAccountInformation(UserSearchItem item, AvatarSize avatarSize = AvatarSize.Size64)
        {
            var profile = ConvertToUserProfile(item.UserId, Regex.Replace(item.Title, "<[^>]+>", string.Empty), item.Cover, avatarSize);
            var communityInfo = _communityAdapter.ConvertToUserCommunityInformation(item);
            return new AccountInformation(
                profile,
                item.Sign,
                item.Level,
                item.Vip.Status == 1,
                communityInfo);
        }

        /// <inheritdoc/>
        public AccountInformation ConvertToAccountInformation(Member member, AvatarSize avatarSize = AvatarSize.Size64)
        {
            var profile = ConvertToUserProfile(Convert.ToInt32(member.Mid), member.Name, member.Face, avatarSize);
            return new AccountInformation(profile, default, Convert.ToInt32(member.Level), default);
        }

        /// <inheritdoc/>
        public RoleProfile ConvertToRoleProfile(PublisherInfo publisher, AvatarSize avatarSize)
        {
            var user = ConvertToUserProfile(publisher.Mid, publisher.Publisher, publisher.PublisherAvatar, avatarSize);
            return new RoleProfile(user);
        }

        /// <inheritdoc/>
        public RoleProfile ConvertToRoleProfile(Staff staff, AvatarSize avatarSize)
        {
            var user = ConvertToUserProfile(Convert.ToInt32(staff.Mid), staff.Name, staff.Face, avatarSize);
            return new RoleProfile(
                user,
                _textToolkit.ConvertToTraditionalChineseIfNeeded(staff.Title));
        }

        /// <inheritdoc/>
        public RoleProfile ConvertToRoleProfile(RecommendAvatar avatar, AvatarSize avatarSize = AvatarSize.Size48)
        {
            var user = ConvertToUserProfile(avatar.UserId, avatar.UserName, avatar.Cover, avatarSize);
            return new RoleProfile(user);
        }

        /// <inheritdoc/>
        public RoleProfile ConvertToRoleProfile(Author author, AvatarSize avatarSize = AvatarSize.Size32)
        {
            var user = ConvertToUserProfile(Convert.ToInt32(author.Mid), author.Name, author.Face, avatarSize);
            return new RoleProfile(user);
        }

        /// <inheritdoc/>
        public RoleProfile ConvertToRoleProfile(PgcCelebrity celebrity, AvatarSize avatarSize = AvatarSize.Size96)
        {
            var user = ConvertToUserProfile(celebrity.Id, celebrity.Name, celebrity.Avatar, avatarSize);
            return new RoleProfile(
                user,
                _textToolkit.ConvertToTraditionalChineseIfNeeded(celebrity.ShortDescription));
        }

        /// <inheritdoc/>
        public UserProfile ConvertToUserProfile(int userId, string userName, string avatar, AvatarSize avatarSize)
        {
            var size = int.Parse(avatarSize.ToString().Replace("Size", string.Empty));
            var image = _imageAdapter.ConvertToImage(avatar, size, size);
            var profile = new UserProfile(userId.ToString(), userName, image);
            return profile;
        }

        /// <inheritdoc/>
        public RelationView ConvertToRelationView(RelatedUserResponse response)
        {
            var count = response.TotalCount;
            var accounts = response.UserList.Select(p => ConvertToAccountInformation(p)).ToList();
            return new RelationView(accounts, count);
        }
    }
}

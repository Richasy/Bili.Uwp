// Copyright (c) Richasy. All rights reserved.

using Bili.Models.BiliBili;
using Bili.Models.Data.User;
using Bili.Models.Enums.App;
using Bilibili.App.Archive.V1;
using Bilibili.App.View.V1;
using Bilibili.Main.Community.Reply.V1;

namespace Bili.Adapter.Interfaces
{
    /// <summary>
    /// 用户资料适配器接口，将来自源网站的用户数据转换为 <see cref="UserProfile"/> , <see cref="RoleProfile"/> 或 <see cref="AccountInformation"/>.
    /// </summary>
    public interface IUserAdapter
    {
        /// <summary>
        /// 将数据整合为用户资料.
        /// </summary>
        /// <param name="userId">用户Id.</param>
        /// <param name="userName">用户名.</param>
        /// <param name="avatar">封面.</param>
        /// <param name="avatarSize">头像尺寸.</param>
        /// <returns><see cref="UserProfile"/>.</returns>
        UserProfile ConvertToUserProfile(int userId, string userName, string avatar, AvatarSize avatarSize);

        /// <summary>
        /// 将 <see cref="PublisherInfo"/> 转换为发布者资料.
        /// </summary>
        /// <param name="publisher">BiliBili的视频发布者信息.</param>
        /// <param name="avatarSize">头像尺寸.</param>
        /// <returns><see cref="RoleProfile"/>.</returns>
        RoleProfile ConvertToRoleProfile(PublisherInfo publisher, AvatarSize avatarSize);

        /// <summary>
        /// 将视频合作者信息 <see cref="Staff"/> 转换为发布者资料.
        /// </summary>
        /// <param name="staff">视频合作者信息.</param>
        /// <param name="avatarSize">头像尺寸.</param>
        /// <returns><see cref="RoleProfile"/>.</returns>
        RoleProfile ConvertToRoleProfile(Staff staff, AvatarSize avatarSize);

        /// <summary>
        /// 将作者信息 <see cref="Author"/> 转换为发布者资料.
        /// </summary>
        /// <param name="author">作者信息.</param>
        /// <param name="avatarSize">头像大小.</param>
        /// <returns><see cref="RoleProfile"/>.</returns>
        RoleProfile ConvertToRoleProfile(Author author, AvatarSize avatarSize = AvatarSize.Size32);

        /// <summary>
        /// 将个人信息 <see cref="MyInfo"/> 转换为 <see cref="AccountInformation"/>.
        /// </summary>
        /// <param name="myInfo">我的资料.</param>
        /// <param name="avatarSize">头像尺寸.</param>
        /// <returns><see cref="AccountInformation"/>.</returns>
        AccountInformation ConvertToAccountInformation(MyInfo myInfo, AvatarSize avatarSize);

        /// <summary>
        /// 将用户搜索条目 <see cref="UserSearchItem"/> 转换为 <see cref="AccountInformation"/>.
        /// </summary>
        /// <param name="item">用户搜索条目.</param>
        /// <param name="avatarSize">头像尺寸.</param>
        /// <returns><see cref="AccountInformation"/>.</returns>
        AccountInformation ConvertToAccountInformation(UserSearchItem item, AvatarSize avatarSize = AvatarSize.Size64);

        /// <summary>
        /// 将个人信息 <see cref="Mine"/> 转换为 <see cref="AccountInformation"/>.
        /// </summary>
        /// <param name="myInfo">我的资料.</param>
        /// <param name="avatarSize">头像尺寸.</param>
        /// <returns><see cref="AccountInformation"/>.</returns>
        AccountInformation ConvertToAccountInformation(Mine myInfo, AvatarSize avatarSize);

        /// <summary>
        /// 将用户空间资料 <see cref="UserSpaceInformation"/> 转换为 <see cref="AccountInformation"/>.
        /// </summary>
        /// <param name="spaceInfo">用户空间资料.</param>
        /// <param name="avatarSize">头像尺寸.</param>
        /// <returns><see cref="AccountInformation"/>.</returns>
        AccountInformation ConvertToAccountInformation(UserSpaceInformation spaceInfo, AvatarSize avatarSize);

        /// <summary>
        /// 将关系用户信息 <see cref="RelatedUser"/> 转换为 <see cref="AccountInformation"/>.
        /// </summary>
        /// <param name="user">关系用户信息.</param>
        /// <param name="avatarSize">头像大小.</param>
        /// <returns><see cref="AccountInformation"/>.</returns>
        AccountInformation ConvertToAccountInformation(RelatedUser user, AvatarSize avatarSize = AvatarSize.Size64);

        /// <summary>
        /// 将用户信息 <see cref="Member"/> 转换为 <see cref="AccountInformation"/>.
        /// </summary>
        /// <param name="member">用户信息.</param>
        /// <param name="avatarSize">头像尺寸.</param>
        /// <returns><see cref="AccountInformation"/>.</returns>
        AccountInformation ConvertToAccountInformation(Member member, AvatarSize avatarSize = AvatarSize.Size64);

        /// <summary>
        /// 将推荐卡片的头像信息 <see cref="RecommendAvatar"/> 转换为角色资料.
        /// </summary>
        /// <param name="avatar">推荐卡片的头像信息.</param>
        /// <param name="avatarSize">头像尺寸.</param>
        /// <returns><see cref="RoleProfile"/>.</returns>
        RoleProfile ConvertToRoleProfile(RecommendAvatar avatar, AvatarSize avatarSize = AvatarSize.Size48);

        /// <summary>
        /// 将明星信息 <see cref="PgcCelebrity"/> 转换为角色资料.
        /// </summary>
        /// <param name="celebrity">明星信息.</param>
        /// <param name="avatarSize">头像大小.</param>
        /// <returns><see cref="RoleProfile"/>.</returns>
        RoleProfile ConvertToRoleProfile(PgcCelebrity celebrity, AvatarSize avatarSize = AvatarSize.Size96);

        /// <summary>
        /// 将关系用户响应结果 <see cref="UserRelationResponse"/> 转换为 <see cref="RelationView"/>.
        /// </summary>
        /// <param name="response">关系用户响应结果.</param>
        /// <returns><see cref="RelationView"/>.</returns>
        RelationView ConvertToRelationView(RelatedUserResponse response);
    }
}

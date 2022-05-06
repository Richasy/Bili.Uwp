// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Community
{
    /// <summary>
    /// 视频发布者信息.
    /// </summary>
    /// <remarks>
    /// 该类是 <see cref="UserProfile"/> 的包装，增加了 <see cref="Role"/> 属性表示在视频中的职能.
    /// </remarks>
    public sealed class PublisherProfile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublisherProfile"/> class.
        /// </summary>
        public PublisherProfile()
            => Role = "Publisher";

        /// <summary>
        /// Initializes a new instance of the <see cref="PublisherProfile"/> class.
        /// </summary>
        /// <param name="user">用户信息.</param>
        public PublisherProfile(UserProfile user)
            : this() => User = user;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublisherProfile"/> class.
        /// </summary>
        /// <param name="user">用户信息.</param>
        /// <param name="role">角色.</param>
        public PublisherProfile(UserProfile user, string role)
            : this(user) => Role = role;

        /// <summary>
        /// 用户信息.
        /// </summary>
        public UserProfile User { get; set; }

        /// <summary>
        /// 视频中所扮演的角色.
        /// </summary>
        /// <remarks>
        /// 通常来说，对于视频发布者独立制作的视频，该属性默认为 Publisher.
        /// 但是当该视频为多人合作发布时，不同的制作者在视频制作期间担任的角色不同，这里的属性可以用以区分.
        /// </remarks>
        public string Role { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is PublisherProfile profile && EqualityComparer<UserProfile>.Default.Equals(User, profile.User);

        /// <inheritdoc/>
        public override int GetHashCode() => User.GetHashCode();
    }
}

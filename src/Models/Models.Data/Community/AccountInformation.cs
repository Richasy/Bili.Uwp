// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Community
{
    /// <summary>
    /// 账户信息.
    /// </summary>
    /// <remarks>
    /// 区别于 <see cref="UserProfile"/>，账户信息包含更多的信息，一般用于显示用户详情时展示完整的用户资料.
    /// </remarks>
    public sealed class AccountInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccountInformation"/> class.
        /// </summary>
        /// <param name="user">用户资料.</param>
        public AccountInformation(UserProfile user) => User = user;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountInformation"/> class.
        /// </summary>
        /// <param name="user">用户资料.</param>
        /// <param name="intro">自我介绍或签名.</param>
        /// <param name="level">等级.</param>
        /// <param name="isVip">是否为高级会员.</param>
        public AccountInformation(UserProfile user, string intro, int level, bool isVip)
            : this(user)
        {
            Introduce = intro;
            Level = level;
            IsVip = isVip;
        }

        /// <summary>
        /// 用户资料.
        /// </summary>
        public UserProfile User { get; set; }

        /// <summary>
        /// 个人介绍.
        /// </summary>
        public string Introduce { get; set; }

        /// <summary>
        /// 账户等级.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 是否为高级会员.
        /// </summary>
        public bool IsVip { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is AccountInformation information && EqualityComparer<UserProfile>.Default.Equals(User, information.User);

        /// <inheritdoc/>
        public override int GetHashCode() => User.GetHashCode();
    }
}

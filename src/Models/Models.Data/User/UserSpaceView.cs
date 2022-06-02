// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Video;

namespace Bili.Models.Data.User
{
    /// <summary>
    /// 用户空间视图.
    /// </summary>
    public sealed class UserSpaceView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSpaceView"/> class.
        /// </summary>
        /// <param name="account">用户信息.</param>
        /// <param name="videoSet">视频列表.</param>
        public UserSpaceView(
            AccountInformation account,
            VideoSet videoSet = default)
        {
            Account = account;
            VideoSet = videoSet;
        }

        /// <summary>
        /// 用户账户信息.
        /// </summary>
        public AccountInformation Account { get; }

        /// <summary>
        /// 该用户发布的视频列表.
        /// </summary>
        public VideoSet VideoSet { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is UserSpaceView view && EqualityComparer<AccountInformation>.Default.Equals(Account, view.Account);

        /// <inheritdoc/>
        public override int GetHashCode() => Account.GetHashCode();
    }
}

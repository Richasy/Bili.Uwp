// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Community;

namespace Bili.Models.Data.User
{
    /// <summary>
    /// 用户信息.
    /// </summary>
    public sealed class UserInformation
    {
        /// <summary>
        /// 用户资料.
        /// </summary>
        public UserProfile Profile { get; }

        /// <summary>
        /// 社交信息.
        /// </summary>
        public UserCommunityInformation CommunityInformation { get; }
    }
}

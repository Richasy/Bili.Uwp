// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Appearance;

namespace Bili.Models.Data.User
{
    /// <summary>
    /// 用户资料.
    /// </summary>
    public sealed class UserProfile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfile"/> class.
        /// </summary>
        /// <param name="id">用户 Id.</param>
        public UserProfile(string id) => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfile"/> struct.
        /// </summary>
        /// <param name="id">用户Id.</param>
        /// <param name="avatar">用户头像.</param>
        /// <param name="name">用户名.</param>
        public UserProfile(string id, string name, Image avatar)
            : this(id)
        {
            Avatar = avatar;
            Name = name;
        }

        /// <summary>
        /// 用户头像.
        /// </summary>
        public Image Avatar { get; set; }

        /// <summary>
        /// 用户 Id.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 用户名.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 是否为完全的用户资料，这表示目前我们获得了用户的基本信息.
        /// </summary>
        /// <remarks>
        /// 有时候我们并不能完全了解用户信息，只能获取一个用户 Id，那么就需要进一步的资料请求.
        /// </remarks>
        /// <returns><c>True</c> 表示资料完备，<c>False</c> 表示需要进一步请求.</returns>
        public bool IsFullProfile()
            => string.IsNullOrEmpty(Avatar?.Uri)
            && !string.IsNullOrEmpty(Id)
            && !string.IsNullOrEmpty(Name);

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is UserProfile profile && Id == profile.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => 2108858624 + EqualityComparer<string>.Default.GetHashCode(Id);
    }
}

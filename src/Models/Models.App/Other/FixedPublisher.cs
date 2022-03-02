// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Richasy.Bili.Models.App
{
    /// <summary>
    /// 固定的UP主.
    /// </summary>
    public class FixedPublisher
    {
        /// <summary>
        /// 用户名.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户头像.
        /// </summary>
        public string AvatarPath { get; set; }

        /// <summary>
        /// 用户Id.
        /// </summary>
        public string UserId { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is FixedPublisher publisher && UserId == publisher.UserId;

        /// <inheritdoc/>
        public override int GetHashCode() => 2139390487 + EqualityComparer<string>.Default.GetHashCode(UserId);
    }
}

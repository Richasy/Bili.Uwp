// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Richasy.Bili.Models.App
{
    /// <summary>
    /// 视频标签.
    /// </summary>
    public class VideoTag
    {
        /// <summary>
        /// 标签Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 标签名.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 标签地址.
        /// </summary>
        public string Uri { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is VideoTag tag && Id == tag.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => 2108858624 + EqualityComparer<string>.Default.GetHashCode(Id);
    }
}

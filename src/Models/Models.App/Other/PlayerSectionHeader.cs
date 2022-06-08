// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Enums.Bili;

namespace Bili.Models.App.Other
{
    /// <summary>
    /// 播放时的关联区块标头.
    /// </summary>
    public sealed class PlayerSectionHeader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerSectionHeader"/> class.
        /// </summary>
        /// <param name="type">区块类型.</param>
        /// <param name="title">标题.</param>
        public PlayerSectionHeader(PlayerSectionType type, string title)
        {
            Type = type;
            Title = title;
        }

        /// <summary>
        /// 区块类型.
        /// </summary>
        public PlayerSectionType Type { get; }

        /// <summary>
        /// 区块标题.
        /// </summary>
        public string Title { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is PlayerSectionHeader header && Type == header.Type;

        /// <inheritdoc/>
        public override int GetHashCode() => 2049151605 + Type.GetHashCode();
    }
}

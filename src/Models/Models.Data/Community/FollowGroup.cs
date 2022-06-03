// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Community
{
    /// <summary>
    /// 关注分组.
    /// </summary>
    public sealed class FollowGroup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FollowGroup"/> class.
        /// </summary>
        /// <param name="id">组标识.</param>
        /// <param name="name">组名.</param>
        /// <param name="totalCount">人数.</param>
        public FollowGroup(string id, string name, int totalCount)
        {
            Id = id;
            Name = name;
            TotalCount = totalCount;
        }

        /// <summary>
        /// 组Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 组名.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分组下的条目数.
        /// </summary>
        public int TotalCount { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is FollowGroup group && Id == group.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}

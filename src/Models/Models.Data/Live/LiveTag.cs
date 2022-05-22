// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Live
{
    /// <summary>
    /// 直播分类标签.
    /// </summary>
    public sealed class LiveTag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveTag"/> class.
        /// </summary>
        /// <param name="id">标识符.</param>
        /// <param name="name">名称.</param>
        /// <param name="sortType">排序方式.</param>
        public LiveTag(string id, string name, string sortType)
        {
            Id = id;
            Name = name;
            SortType = sortType;
        }

        /// <summary>
        /// 标签Id.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 标签名.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 排序方式.
        /// </summary>
        public string SortType { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is LiveTag tag && Id == tag.Id && SortType == tag.SortType;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode() + SortType.GetHashCode();
    }
}

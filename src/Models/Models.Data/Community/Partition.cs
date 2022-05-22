// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Appearance;

namespace Bili.Models.Data.Community
{
    /// <summary>
    /// 视频类型分区.
    /// </summary>
    public sealed class Partition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Partition"/> class.
        /// </summary>
        /// <param name="id">标识符.</param>
        /// <param name="name">分区名.</param>
        /// <param name="image">Logo.</param>
        /// <param name="children">子项.</param>
        /// <param name="parentId">父分区 Id.</param>
        public Partition(
            string id,
            string name,
            Image image = default,
            IEnumerable<Partition> children = default,
            string parentId = default)
        {
            Id = id;
            Name = name;
            Image = image;
            Children = children;
            ParentId = parentId;
        }

        /// <summary>
        /// 标识符.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 分区名称.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 分区Logo.
        /// </summary>
        public Image Image { get; set; }

        /// <summary>
        /// 父分区标识符.
        /// </summary>
        public string ParentId { get; set; }

        /// <summary>
        /// 子分区列表.
        /// </summary>
        public IEnumerable<Partition> Children { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Partition partition && Id == partition.Id && Name == partition.Name;

        /// <inheritdoc/>
        public override int GetHashCode()
            => Id.GetHashCode() + Name.GetHashCode();
    }
}

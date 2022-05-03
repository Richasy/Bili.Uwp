// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Community
{
    /// <summary>
    /// 对内容打上的标签，可用于关联搜索以及对内容的快速甄别.
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tag"/> class.
        /// </summary>
        public Tag()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Tag"/> class.
        /// </summary>
        /// <param name="id">标识符.</param>
        /// <param name="name">名称.</param>
        /// <param name="uri">目标链接.</param>
        public Tag(string id, string name, string uri = "")
        {
            Id = id;
            Name = name;
            Uri = uri;
        }

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
        public override bool Equals(object obj) => obj is Tag tag && Id == tag.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}

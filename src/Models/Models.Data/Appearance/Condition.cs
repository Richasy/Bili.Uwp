// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Data.Appearance
{
    /// <summary>
    /// 键值对式的条件类型.
    /// </summary>
    public sealed class Condition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Condition"/> class.
        /// </summary>
        /// <param name="name">名称.</param>
        /// <param name="id">标识符.</param>
        public Condition(string name, string id)
        {
            Name = name;
            Id = id;
        }

        /// <summary>
        /// 显示的名称.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 标识符.
        /// </summary>
        public string Id { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Condition condition && Id == condition.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => Name;
    }
}

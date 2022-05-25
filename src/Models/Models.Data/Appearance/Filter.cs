// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Appearance
{
    /// <summary>
    /// 条件筛选，可用于生成一个选择器.
    /// </summary>
    public sealed class Filter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Filter"/> class.
        /// </summary>
        /// <param name="name">显示名称.</param>
        /// <param name="id">标识符.</param>
        /// <param name="conditions">条件列表.</param>
        public Filter(string name, string id, IEnumerable<Condition> conditions)
        {
            Name = name;
            Id = id;
            Conditions = conditions;
        }

        /// <summary>
        /// 筛选条件的显示名称.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 条件标识符.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 备选值列表.
        /// </summary>
        public IEnumerable<Condition> Conditions { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is Filter filter && Id == filter.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}

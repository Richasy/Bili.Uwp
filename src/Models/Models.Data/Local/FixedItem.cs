﻿// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Enums.App;

namespace Bili.Models.Data.Local
{
    /// <summary>
    /// 固定在首页的条目.
    /// </summary>
    public class FixedItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FixedItem"/> class.
        /// </summary>
        public FixedItem()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedItem"/> class.
        /// </summary>
        public FixedItem(string cover, string title, string id, FixedType type)
        {
            Cover = cover;
            Title = title;
            Id = id;
            Type = type;
        }

        /// <summary>
        /// 封面.
        /// </summary>
        public string Cover { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 标识符.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 类型.
        /// </summary>
        public FixedType Type { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is FixedItem item && Id == item.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => Id.GetHashCode();
    }
}

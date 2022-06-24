// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Local
{
    /// <summary>
    /// 内容集合，包含内容列表和总数.
    /// </summary>
    public class ContentSet
    {
        /// <summary>
        /// 内容总数.
        /// </summary>
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// 指定类型的内容集合.
    /// </summary>
    /// <typeparam name="T">指定的类型.</typeparam>
    public class ContentSet<T> : ContentSet
        where T : class
    {
        /// <summary>
        /// 条目列表.
        /// </summary>
        public IEnumerable<T> Items { get; set; }
    }
}

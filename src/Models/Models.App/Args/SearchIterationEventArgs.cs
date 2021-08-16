// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 搜索迭代事件基类.
    /// </summary>
    /// <typeparam name="T">内容类型.</typeparam>
    public class SearchIterationEventArgs<T> : EventArgs
    {
        /// <summary>
        /// 是否加载完成.
        /// </summary>
        public bool HasMore { get; set; }

        /// <summary>
        /// 下一页页码（如果没有下一页，则返回-1）.
        /// </summary>
        public int NextPageNumber { get; set; }

        /// <summary>
        /// 搜索结果.
        /// </summary>
        public List<T> List { get; set; }

        /// <summary>
        /// 关键词.
        /// </summary>
        public string Keyword { get; set; }
    }
}

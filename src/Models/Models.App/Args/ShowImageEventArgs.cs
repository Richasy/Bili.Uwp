// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;

namespace Bili.Models.App.Args
{
    /// <summary>
    /// 显示图片事件参数.
    /// </summary>
    public class ShowImageEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShowImageEventArgs"/> class.
        /// </summary>
        public ShowImageEventArgs()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowImageEventArgs"/> class.
        /// </summary>
        /// <param name="urls">图片地址列表.</param>
        /// <param name="firstIndex">初始索引.</param>
        public ShowImageEventArgs(List<string> urls, int firstIndex = 0)
        {
            ImageUrls = urls;
            ShowIndex = firstIndex;
        }

        /// <summary>
        /// 图片地址.
        /// </summary>
        public List<string> ImageUrls { get; set; }

        /// <summary>
        /// 初始显示图片索引.
        /// </summary>
        public int ShowIndex { get; set; }
    }
}

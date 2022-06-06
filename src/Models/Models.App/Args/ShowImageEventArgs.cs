// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Bili.Models.Data.Appearance;

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
        /// <param name="images">图片地址列表.</param>
        /// <param name="firstIndex">初始索引.</param>
        public ShowImageEventArgs(IEnumerable<Image> images, int firstIndex = 0)
        {
            Images = images;
            ShowIndex = firstIndex;
        }

        /// <summary>
        /// 图片地址.
        /// </summary>
        public IEnumerable<Image> Images { get; set; }

        /// <summary>
        /// 初始显示图片索引.
        /// </summary>
        public int ShowIndex { get; set; }
    }
}

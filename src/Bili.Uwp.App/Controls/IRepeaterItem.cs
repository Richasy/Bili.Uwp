// Copyright (c) Richasy. All rights reserved.

using Windows.Foundation;

namespace Bili.Uwp.App.Controls
{
    /// <summary>
    /// 用于ItemsRepeater的条目.
    /// </summary>
    public interface IRepeaterItem
    {
        /// <summary>
        /// 获取占位大小.
        /// </summary>
        /// <returns><see cref="Windows.Foundation.Size"/>.</returns>
        Size GetHolderSize();
    }
}

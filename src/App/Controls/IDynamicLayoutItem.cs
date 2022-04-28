// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// 动态调整布局的条目.
    /// </summary>
    public interface IDynamicLayoutItem
    {
        /// <summary>
        /// 布局方式.
        /// </summary>
        Orientation Orientation { get; set; }
    }
}

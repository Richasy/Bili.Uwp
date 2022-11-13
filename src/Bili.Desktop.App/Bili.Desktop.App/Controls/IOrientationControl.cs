// Copyright (c) Richasy. All rights reserved.

using Microsoft.UI.Xaml.Controls;

namespace Bili.Desktop.App.Controls
{
    /// <summary>
    /// 可以根据指定方向调整布局的控件.
    /// </summary>
    internal interface IOrientationControl
    {
        /// <summary>
        /// 根据方向改变布局.
        /// </summary>
        /// <param name="orientation">指定的方向.</param>
        void ChangeLayout(Orientation orientation);
    }
}

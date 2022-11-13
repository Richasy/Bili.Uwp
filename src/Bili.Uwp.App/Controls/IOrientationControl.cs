// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml.Controls;

namespace Bili.Uwp.App.Controls
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

// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 带进度环和完成动画的按钮.
    /// </summary>
    public sealed class ProgressButton : Button
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressButton"/> class.
        /// </summary>
        public ProgressButton()
        {
            this.DefaultStyleKey = typeof(ProgressButton);
        }
    }
}

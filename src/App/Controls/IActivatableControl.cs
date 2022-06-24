// Copyright (c) Richasy. All rights reserved.

using System;

namespace Bili.App.Controls
{
    /// <summary>
    /// 包含单个可操作动作的控件，比如包含一个按钮的面板等.
    /// </summary>
    public interface IActivatableControl
    {
        /// <summary>
        /// 动作被触发（比如按钮被点击）.
        /// </summary>
        event EventHandler Activated;
    }
}

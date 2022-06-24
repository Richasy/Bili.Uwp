// Copyright (c) Richasy. All rights reserved.

using System;

namespace Bili.App.Controls
{
    /// <summary>
    /// 卡片面板状态更改事件参数.
    /// </summary>
    public class CardPanelStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CardPanelStateChangedEventArgs"/> class.
        /// </summary>
        /// <param name="isPointerOver">是否处于PointerOver状态.</param>
        /// <param name="isPressed">是否处于按压状态.</param>
        public CardPanelStateChangedEventArgs(bool isPointerOver, bool isPressed)
        {
            IsPointerOver = isPointerOver;
            IsPressed = isPressed;
        }

        /// <summary>
        /// 是否处于PointerOver状态.
        /// </summary>
        public bool IsPointerOver { get; }

        /// <summary>
        /// 是否处于按压状态.
        /// </summary>
        public bool IsPressed { get; }
    }
}

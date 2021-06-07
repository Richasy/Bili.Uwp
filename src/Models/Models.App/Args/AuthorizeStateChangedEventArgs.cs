// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 授权验证的事件参数.
    /// </summary>
    public class AuthorizeStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 创建一个 <see cref="ProviderStateChangedEventArgs"/> 类型的实例.
        /// </summary>
        /// <param name="oldState">前一个<see cref="AuthorizeState"/>.</param>
        /// <param name="newState">当前的<see cref="AuthorizeState"/>.</param>
        public AuthorizeStateChangedEventArgs(AuthorizeState oldState, AuthorizeState newState)
        {
            OldState = oldState;
            NewState = newState;
        }

        /// <summary>
        /// 获取前一个授权状态.
        /// </summary>
        public AuthorizeState OldState { get; private set; }

        /// <summary>
        /// 获取新的授权状态.
        /// </summary>
        public AuthorizeState NewState { get; private set; }
    }
}

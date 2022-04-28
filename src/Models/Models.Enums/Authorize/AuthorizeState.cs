// Copyright (c) Richasy. All rights reserved.

namespace Bili.Models.Enums
{
    /// <summary>
    /// <see cref="AuthorizeState"/>表明当前的授权状态.
    /// </summary>
    public enum AuthorizeState
    {
        /// <summary>
        /// 正在请求授权，等待授权结果.
        /// </summary>
        Loading,

        /// <summary>
        /// 用户已退出.
        /// </summary>
        SignedOut,

        /// <summary>
        /// 用户已登录.
        /// </summary>
        SignedIn,
    }
}

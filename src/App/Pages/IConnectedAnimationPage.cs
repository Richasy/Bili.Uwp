// Copyright (c) Richasy. All rights reserved.

namespace Bili.App.Pages
{
    /// <summary>
    /// 该页包含连接动画相关的操作.
    /// </summary>
    public interface IConnectedAnimationPage
    {
        /// <summary>
        /// 尝试检查有效的连接动画并启动.
        /// </summary>
        void TryStartConnectedAnimation();
    }
}

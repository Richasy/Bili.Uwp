// Copyright (c) Richasy. All rights reserved.

using Windows.UI;
using Windows.UI.Composition;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 视觉扩展.
    /// </summary>
    public static class VisualExtensions
    {
        /// <summary>
        /// 绑定大小.
        /// </summary>
        /// <param name="target">目标元素.</param>
        /// <param name="source">来源视图.</param>
        public static void BindSize(this Visual target, Visual source)
        {
            var exp = target.Compositor.CreateExpressionAnimation("host.Size");
            exp.SetReferenceParameter("host", source);
            target.StartAnimation("Size", exp);
        }

        /// <summary>
        /// 绑定中心点.
        /// </summary>
        /// <param name="target">目标视图.</param>
        public static void BindCenterPoint(this Visual target)
        {
            var exp = target.Compositor.CreateExpressionAnimation("Vector3(this.Target.Size.X / 2, this.Target.Size.Y / 2, 0f)");
            target.StartAnimation("CenterPoint", exp);
        }
    }
}

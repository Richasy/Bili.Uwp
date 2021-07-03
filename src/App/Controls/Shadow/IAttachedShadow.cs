// Copyright (c) Richasy. All rights reserved.

using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;

namespace Richasy.Bili.App.Controls.Shadow
{
    /// <summary>
    /// 附加阴影接口定义.
    /// </summary>
    public interface IAttachedShadow
    {
        /// <summary>
        /// 虚化半径.
        /// </summary>
        double BlurRadius { get; set; }

        /// <summary>
        /// 半透明度.
        /// </summary>
        double Opacity { get; set; }

        /// <summary>
        /// 偏移值.
        /// </summary>
        Vector3 Offset { get; set; }

        /// <summary>
        /// 阴影颜色.
        /// </summary>
        Color Color { get; set; }

        /// <summary>
        /// 是否支持 <c>SizeChanged</c> 事件.
        /// </summary>
        bool SupportsOnSizeChangedEvent { get; }

        /// <summary>
        /// 与UI元素链接.
        /// </summary>
        /// <param name="element">需要附加阴影的UI元素.</param>
        void ConnectElement(FrameworkElement element);

        /// <summary>
        /// 与UI元素解除链接.
        /// </summary>
        /// <param name="element">附加阴影的UI元素.</param>
        void DisconnectElement(FrameworkElement element);

        /// <summary>
        /// 当元素上下文准备就绪时调用.
        /// </summary>
        /// <param name="context">附加阴影的元素上下文.</param>
        void OnElementContextInitialized(AttachedShadowElementContext context);

        /// <summary>
        /// 当元素上下文销毁时调用.
        /// </summary>
        /// <param name="context">附加阴影的元素上下文.</param>
        void OnElementContextUninitialized(AttachedShadowElementContext context);

        /// <summary>
        /// 当元素大小改变时调用.
        /// </summary>
        /// <param name="context">附加阴影的元素上下文.</param>
        /// <param name="newSize">元素的新尺寸.</param>
        /// <param name="oldSize">元素的旧尺寸.</param>
        void OnSizeChanged(AttachedShadowElementContext context, Size newSize, Size oldSize);

        /// <summary>
        /// 获取元素的上下文.
        /// </summary>
        /// <param name="element">附加阴影的元素.</param>
        /// <returns><see cref="AttachedShadowElementContext"/>.</returns>
        AttachedShadowElementContext GetElementContext(FrameworkElement element);
    }
}

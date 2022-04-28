// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Bili.App.Resources.Extension
{
    /// <summary>
    /// 连接动画扩展类.
    /// </summary>
    public static class ConnectedAnimationExtension
    {
        /// <summary>
        /// 尝试启动连接动画.
        /// </summary>
        /// <param name="connectedAnimationService">连接动画服务类.</param>
        /// <param name="animationKey">动画名.</param>
        /// <param name="element">目标元素.</param>
        public static void TryStartAnimation(this ConnectedAnimationService connectedAnimationService, string animationKey, UIElement element)
        {
            var animation = connectedAnimationService.GetAnimation(animationKey);
            if (animation != null)
            {
                animation.TryStart(element);
            }
        }
    }
}

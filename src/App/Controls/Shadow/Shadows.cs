// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml;

namespace Richasy.Bili.App.Controls.Shadow
{
    /// <summary>
    /// 阴影的静态类.
    /// </summary>
    public static class Shadows
    {
        /// <summary>
        /// <see cref="IAttachedShadow"/>的附加属性.
        /// </summary>
        public static DependencyProperty AttachedShadowProperty { get; } =
            DependencyProperty.RegisterAttached(
                "AttachedShadow",
                typeof(IAttachedShadow),
                typeof(Shadows),
                new PropertyMetadata(null, OnAttachedShadowChanged));

        /// <summary>
        /// 获取附加的阴影.
        /// </summary>
        /// <param name="obj"><see cref="DependencyObject"/>.</param>
        /// <returns><see cref="IAttachedShadow"/>.</returns>
        public static IAttachedShadow GetAttachedShadow(DependencyObject obj)
        {
            return (IAttachedShadow)obj.GetValue(AttachedShadowProperty);
        }

        /// <summary>
        /// 设置附加的阴影.
        /// </summary>
        /// <param name="obj"><see cref="DependencyObject"/>.</param>
        /// <param name="value"><see cref="IAttachedShadow"/>.</param>
        public static void SetAttachedShadow(DependencyObject obj, IAttachedShadow value)
        {
            obj.SetValue(AttachedShadowProperty, value);
        }

        private static void OnAttachedShadowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is FrameworkElement element))
            {
                return;
            }

            if (e.OldValue is IAttachedShadow oldShadow)
            {
                oldShadow.DisconnectElement(element);
            }

            if (e.NewValue is IAttachedShadow newShadow)
            {
                newShadow.ConnectElement(element);
            }
        }
    }
}

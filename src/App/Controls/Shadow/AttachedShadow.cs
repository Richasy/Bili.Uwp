// Copyright (c) Richasy. All rights reserved.

using System.Numerics;
using System.Runtime.CompilerServices;
using Microsoft.Graphics.Canvas.Effects;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Richasy.Bili.App.Controls.Shadow
{
    /// <summary>
    /// 附加阴影的基类.
    /// </summary>
    public abstract class AttachedShadow : DependencyObject, IAttachedShadow
    {
        /// <summary>
        /// <see cref="BlurRadius"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty BlurRadiusProperty =
            DependencyProperty.Register(nameof(BlurRadius), typeof(double), typeof(AttachedShadow), new PropertyMetadata(12d, OnDependencyPropertyChanged));

        /// <summary>
        /// <see cref="Color"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(nameof(Color), typeof(Color), typeof(AttachedShadow), new PropertyMetadata(Colors.Black, OnDependencyPropertyChanged));

        /// <summary>
        /// <see cref="Offset"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty OffsetProperty =
            DependencyProperty.Register(
                nameof(Offset),
                typeof(Vector3),
                typeof(AttachedShadow),
                new PropertyMetadata(Vector3.Zero, OnDependencyPropertyChanged));

        /// <summary>
        /// <see cref="Opacity"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty OpacityProperty =
            DependencyProperty.Register(nameof(Opacity), typeof(double), typeof(AttachedShadow), new PropertyMetadata(1d, OnDependencyPropertyChanged));

        private const string AlphaMaskSourceKey = "AttachedShadowAlphaMask";
        private const string SpriteVisualSourceKey = "AttachedShadowSpriteVisual";

        private static readonly string AlphaMaskEffectResourceKey = "AttachedShadowSpriteVisualAlphaMaskEffect";
        private static readonly string OpacityMaskEffectBrushResourceKey = "AttachedShadowSpriteVisualEffectBrush";
        private static readonly string OpacityMaskResourceKey = "AttachedShadowSpriteVisualOpacityMask";
        private static readonly string OpacityMaskVisualResourceKey = "AttachedShadowSpriteVisualOpacityMaskVisual";
        private static readonly string OpacityMaskVisualSurfaceResourceKey = "AttachedShadowSpriteVisualOpacityMaskSurface";
        private static readonly string OpacityMaskSurfaceBrushResourceKey = "AttachedShadowSpriteVisualOpacityMaskSurfaceBrush";

        /// <summary>
        /// 标识当前平台是否支持<see cref="AttachedShadow"/>.
        /// </summary>
        public abstract bool IsSupported { get; }

        /// <inheritdoc/>
        public double BlurRadius
        {
            get => (double)GetValue(BlurRadiusProperty);
            set => SetValue(BlurRadiusProperty, value);
        }

        /// <inheritdoc/>
        public double Opacity
        {
            get => (double)GetValue(OpacityProperty);
            set => SetValue(OpacityProperty, value);
        }

        /// <inheritdoc/>
        public Vector3 Offset
        {
            get => (Vector3)GetValue(OffsetProperty);
            set => SetValue(OffsetProperty, value);
        }

        /// <inheritdoc/>
        public Color Color
        {
            get => (Color)GetValue(ColorProperty);
            set => SetValue(ColorProperty, value);
        }

        /// <inheritdoc/>
        public abstract bool SupportsOnSizeChangedEvent { get; }

        /// <summary>
        /// 返回<see cref="AttachedShadow"/>连接到的UI元素的上下文集合.
        /// </summary>
        protected ConditionalWeakTable<FrameworkElement, AttachedShadowElementContext> ShadowElementContextTable { get; private set; }

        /// <inheritdoc/>
        void IAttachedShadow.ConnectElement(FrameworkElement element)
        {
            if (!IsSupported)
            {
                return;
            }

            ShadowElementContextTable = ShadowElementContextTable ?? new ConditionalWeakTable<FrameworkElement, AttachedShadowElementContext>();
            if (ShadowElementContextTable.TryGetValue(element, out var context))
            {
                return;
            }

            context = new AttachedShadowElementContext();
            context.ConnectToElement(this, element);
            ShadowElementContextTable.Add(element, context);
        }

        /// <inheritdoc/>
        void IAttachedShadow.DisconnectElement(FrameworkElement element)
        {
            if (ShadowElementContextTable == null)
            {
                return;
            }

            if (ShadowElementContextTable.TryGetValue(element, out var context))
            {
                context.DisconnectFromElement();
                ShadowElementContextTable.Remove(element);
            }
        }

        /// <inheritdoc/>
        public virtual void OnElementContextInitialized(AttachedShadowElementContext context)
        {
            OnPropertyChanged(context, OpacityProperty, Opacity, Opacity);
            OnPropertyChanged(context, BlurRadiusProperty, BlurRadius, BlurRadius);
            OnPropertyChanged(context, ColorProperty, Color, Color);
            OnPropertyChanged(context, OffsetProperty, Offset, Offset);
            UpdateShadowClip(context);
            UpdateShadowMask(context);
            UpdateShadowOpacityMask(context);
            SetElementChildVisual(context);
        }

        /// <inheritdoc/>
        public virtual void OnElementContextUninitialized(AttachedShadowElementContext context)
        {
            context.RemoveAndDisposeResource(OpacityMaskResourceKey);
            context.RemoveAndDisposeResource(OpacityMaskVisualResourceKey);
            context.RemoveAndDisposeResource(OpacityMaskVisualSurfaceResourceKey);
            context.RemoveAndDisposeResource(OpacityMaskSurfaceBrushResourceKey);
            context.RemoveAndDisposeResource(OpacityMaskEffectBrushResourceKey);
            context.RemoveAndDisposeResource(AlphaMaskEffectResourceKey);
            ElementCompositionPreview.SetElementChildVisual(context.Element, null);
        }

        /// <inheritdoc/>
        void IAttachedShadow.OnSizeChanged(AttachedShadowElementContext context, Size newSize, Size oldSize)
        {
            OnSizeChanged(context, newSize, oldSize);
        }

        /// <inheritdoc/>
        public AttachedShadowElementContext GetElementContext(FrameworkElement element)
        {
            if (ShadowElementContextTable != null && ShadowElementContextTable.TryGetValue(element, out var context))
            {
                return context;
            }

            return null;
        }

        /// <summary>
        /// 在依赖属性更改时发生.
        /// </summary>
        /// <param name="sender">事件发送者.</param>
        /// <param name="args">事件参数.</param>
        protected static void OnDependencyPropertyChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as AttachedShadow)?.CallPropertyChangedForEachElement(args.Property, args.OldValue, args.NewValue);
        }

        /// <summary>
        /// 获取当前元素图形的 <see cref="CompositionBrush"/> 以待转换为阴影.
        /// </summary>
        /// <param name="context">附加阴影的数据上下文.</param>
        /// <returns><see cref="CompositionBrush"/>.</returns>
        protected virtual CompositionBrush GetShadowMask(AttachedShadowElementContext context)
        {
            return null;
        }

        /// <summary>
        /// 获取 <see cref="CompositionBrush"/> 用作阴影图层的不透明蒙版.
        /// </summary>
        /// <param name="context">附加阴影的数据上下文.</param>
        /// <returns><see cref="CompositionBrush"/>.</returns>
        protected virtual CompositionBrush GetShadowOpacityMask(AttachedShadowElementContext context)
        {
            return null;
        }

        /// <summary>
        /// 获取 <see cref="CompositionClip"/> 用作图层阴影.
        /// </summary>
        /// <param name="context">附加阴影的数据上下文.</param>
        /// <returns><see cref="CompositionClip"/>.</returns>
        protected virtual CompositionClip GetShadowClip(AttachedShadowElementContext context)
        {
            return null;
        }

        /// <summary>
        /// 更新阴影图层蒙版.
        /// </summary>
        /// <param name="context">附加阴影的数据上下文.</param>
        protected void UpdateShadowMask(AttachedShadowElementContext context)
        {
            if (!context.IsInitialized)
            {
                return;
            }

            context.Shadow.Mask = GetShadowMask(context);
        }

        /// <summary>
        /// 更新阴影图层的布局.
        /// </summary>
        /// <param name="context">附加阴影的元素上下文.</param>
        protected void UpdateShadowClip(AttachedShadowElementContext context)
        {
            if (!context.IsInitialized)
            {
                return;
            }

            context.SpriteVisual.Clip = GetShadowClip(context);
        }

        /// <summary>
        /// 更新阴影图层的不透明蒙版.
        /// </summary>
        /// <param name="context">附加阴影的数据上下文.</param>
        protected void UpdateShadowOpacityMask(AttachedShadowElementContext context)
        {
            if (!context.IsInitialized)
            {
                return;
            }

            var brush = GetShadowOpacityMask(context);
            if (brush != null)
            {
                context.AddResource(OpacityMaskResourceKey, brush);
            }
            else
            {
                context.RemoveAndDisposeResource(OpacityMaskResourceKey);
            }
        }

        /// <summary>
        /// 当依赖属性改变时调用该方法.
        /// </summary>
        /// <param name="context">附加阴影的元素上下文.</param>
        /// <param name="property">触发的依赖属性.</param>
        /// <param name="oldValue">旧值.</param>
        /// <param name="newValue">新值.</param>
        protected virtual void OnPropertyChanged(AttachedShadowElementContext context, DependencyProperty property, object oldValue, object newValue)
        {
            if (!context.IsInitialized)
            {
                return;
            }

            if (property == BlurRadiusProperty)
            {
                context.Shadow.BlurRadius = (float)(double)newValue;
            }
            else if (property == OpacityProperty)
            {
                context.Shadow.Opacity = (float)(double)newValue;
            }
            else if (property == ColorProperty)
            {
                context.Shadow.Color = (Color)newValue;
            }
            else if (property == OffsetProperty)
            {
                context.Shadow.Offset = (Vector3)newValue;
            }
        }

        /// <summary>
        /// 在阴影依赖的UI元素大小改变时发生.
        /// </summary>
        /// <param name="context">附加阴影的元素上下文.</param>
        /// <param name="newSize">新的尺寸.</param>
        /// <param name="oldSize">旧的尺寸.</param>
        protected virtual void OnSizeChanged(AttachedShadowElementContext context, Size newSize, Size oldSize)
        {
        }

        private void SetElementChildVisual(AttachedShadowElementContext context)
        {
            if (context.TryGetResource<CompositionEffectBrush>(OpacityMaskResourceKey, out var opacityMask))
            {
                var visualSurface = context.GetResource<CompositionVisualSurface>(OpacityMaskVisualSurfaceResourceKey) ?? context.AddResource(
                    OpacityMaskVisualSurfaceResourceKey,
                    context.Compositor.CreateVisualSurface());
                visualSurface.SourceVisual = context.SpriteVisual;
                visualSurface.StartAnimation(nameof(visualSurface.SourceSize), context.Compositor.CreateExpressionAnimation("this.SourceVisual.Size"));

                var surfaceBrush = context.GetResource<CompositionSurfaceBrush>(OpacityMaskSurfaceBrushResourceKey) ?? context.AddResource(
                    OpacityMaskSurfaceBrushResourceKey,
                    context.Compositor.CreateSurfaceBrush(visualSurface));
                var alphaMask = context.GetResource<AlphaMaskEffect>(AlphaMaskEffectResourceKey) ?? context.AddResource(AlphaMaskEffectResourceKey, new AlphaMaskEffect());
                alphaMask.Source = new CompositionEffectSourceParameter(SpriteVisualSourceKey);
                alphaMask.AlphaMask = new CompositionEffectSourceParameter(AlphaMaskSourceKey);

                using (var factory = context.Compositor.CreateEffectFactory(alphaMask))
                {
                    context.RemoveAndDisposeResource(OpacityMaskEffectBrushResourceKey);
                    var brush = context.AddResource(OpacityMaskEffectBrushResourceKey, factory.CreateBrush());
                    brush.SetSourceParameter(SpriteVisualSourceKey, surfaceBrush);
                    brush.SetSourceParameter(AlphaMaskSourceKey, opacityMask);

                    var visual = context.GetResource<SpriteVisual>(OpacityMaskVisualResourceKey) ?? context.AddResource(
                        OpacityMaskVisualResourceKey,
                        context.Compositor.CreateSpriteVisual());
                    visual.RelativeSizeAdjustment = Vector2.One;
                    visual.Brush = brush;
                    ElementCompositionPreview.SetElementChildVisual(context.Element, visual);
                }
            }
            else
            {
                ElementCompositionPreview.SetElementChildVisual(context.Element, context.SpriteVisual);
            }
        }

        private void CallPropertyChangedForEachElement(DependencyProperty property, object oldValue, object newValue)
        {
            if (ShadowElementContextTable == null)
            {
                return;
            }

            foreach (var context in ShadowElementContextTable)
            {
                if (context.Value.IsInitialized)
                {
                    OnPropertyChanged(context.Value, property, oldValue, newValue);
                }
            }
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Numerics;
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
        private const string AlphaMaskSourceKey = "AttachedShadowAlphaMask";
        private const string SpriteVisualSourceKey = "AttachedShadowSpriteVisual";

        private static readonly TypedResourceKey<AlphaMaskEffect> AlphaMaskEffectResourceKey = "AttachedShadowSpriteVisualAlphaMaskEffect";
        private static readonly TypedResourceKey<CompositionEffectBrush> OpacityMaskEffectBrushResourceKey = "AttachedShadowSpriteVisualEffectBrush";
        private static readonly TypedResourceKey<CompositionBrush> OpacityMaskResourceKey = "AttachedShadowSpriteVisualOpacityMask";
        private static readonly TypedResourceKey<SpriteVisual> OpacityMaskVisualResourceKey = "AttachedShadowSpriteVisualOpacityMaskVisual";
        private static readonly TypedResourceKey<CompositionVisualSurface> OpacityMaskVisualSurfaceResourceKey = "AttachedShadowSpriteVisualOpacityMaskSurface";
        private static readonly TypedResourceKey<CompositionSurfaceBrush> OpacityMaskSurfaceBrushResourceKey =
            "AttachedShadowSpriteVisualOpacityMaskSurfaceBrush";

        public static readonly DependencyProperty BlurRadiusProperty =
            DependencyProperty.Register(nameof(BlurRadius), typeof(double), typeof(AttachedShadow), new PropertyMetadata(12d, OnDependencyPropertyChanged));

        public static readonly DependencyProperty ColorProperty =
            DependencyProperty.Register(nameof(Color), typeof(Color), typeof(AttachedShadow), new PropertyMetadata(Colors.Black, OnDependencyPropertyChanged));

        public static readonly DependencyProperty OffsetProperty =
            DependencyProperty.Register(
                nameof(Offset),
                typeof(Vector3),
                typeof(AttachedShadow),
                new PropertyMetadata(Vector3.Zero, OnDependencyPropertyChanged));

        public static readonly DependencyProperty OpacityProperty =
            DependencyProperty.Register(nameof(Opacity), typeof(double), typeof(AttachedShadow), new PropertyMetadata(1d, OnDependencyPropertyChanged));

        /// <summary>
        /// Returns whether or not this <see cref="AttachedShadow"/> implementation is supported on the current platform
        /// </summary>
        public abstract bool IsSupported { get; }

        /// <summary>
        /// Returns the collection of <see cref="AttachedShadowElementContext"/> for each element this <see cref="AttachedShadow"/> is connected to
        /// </summary>
        protected ConditionalWeakTable<FrameworkElement, AttachedShadowElementContext> ShadowElementContextTable { get; private set; }

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

        /// <summary>
        /// Returns whether or not OnSizeChanged should be called when <see cref="FrameworkElement.SizeChanged"/> is fired.
        /// </summary>
        public abstract bool SupportsOnSizeChangedEvent { get; }

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
            context.RemoveResource(OpacityMaskResourceKey)?.Dispose();
            context.RemoveResource(OpacityMaskVisualResourceKey)?.Dispose();
            context.RemoveResource(OpacityMaskVisualSurfaceResourceKey)?.Dispose();
            context.RemoveResource(OpacityMaskSurfaceBrushResourceKey)?.Dispose();
            context.RemoveResource(OpacityMaskEffectBrushResourceKey)?.Dispose();
            context.RemoveResource(AlphaMaskEffectResourceKey)?.Dispose();
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

        private void SetElementChildVisual(AttachedShadowElementContext context)
        {
            if (context.TryGetResource(OpacityMaskResourceKey, out var opacityMask))
            {
                var visualSurface = context.GetResource(OpacityMaskVisualSurfaceResourceKey) ?? context.AddResource(
                    OpacityMaskVisualSurfaceResourceKey,
                    context.Compositor.CreateVisualSurface());
                visualSurface.SourceVisual = context.SpriteVisual;
                visualSurface.StartAnimation(nameof(visualSurface.SourceSize), context.Compositor.CreateExpressionAnimation("this.SourceVisual.Size"));

                var surfaceBrush = context.GetResource(OpacityMaskSurfaceBrushResourceKey) ?? context.AddResource(
                    OpacityMaskSurfaceBrushResourceKey,
                    context.Compositor.CreateSurfaceBrush(visualSurface));
                var alphaMask = context.GetResource(AlphaMaskEffectResourceKey) ?? context.AddResource(AlphaMaskEffectResourceKey, new AlphaMaskEffect());
                alphaMask.Source = new CompositionEffectSourceParameter(SpriteVisualSourceKey);
                alphaMask.AlphaMask = new CompositionEffectSourceParameter(AlphaMaskSourceKey);

                using (var factory = context.Compositor.CreateEffectFactory(alphaMask))
                {
                    context.RemoveResource(OpacityMaskEffectBrushResourceKey)?.Dispose();
                    var brush = context.AddResource(OpacityMaskEffectBrushResourceKey, factory.CreateBrush());
                    brush.SetSourceParameter(SpriteVisualSourceKey, surfaceBrush);
                    brush.SetSourceParameter(AlphaMaskSourceKey, opacityMask);

                    var visual = context.GetResource(OpacityMaskVisualResourceKey) ?? context.AddResource(
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

        /// <summary>
        /// Use this method as the <see cref="PropertyChangedCallback"/> for <see cref="DependencyProperty">DependencyProperties</see> in derived classes
        /// </summary>
        protected static void OnDependencyPropertyChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            (sender as AttachedShadow)?.CallPropertyChangedForEachElement(args.Property, args.OldValue, args.NewValue);
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

        /// <summary>
        /// Get a <see cref="CompositionBrush"/> in the shape of the element that is casting the shadow
        /// </summary>
        protected virtual CompositionBrush GetShadowMask(AttachedShadowElementContext context)
        {
            return null;
        }

        /// <summary>
        /// Get a <see cref="CompositionBrush"/> that serves as an opacity mask for the shadow's <see cref="SpriteVisual"/>
        /// </summary>
        protected virtual CompositionBrush GetShadowOpacityMask(AttachedShadowElementContext context)
        {
            return null;
        }

        /// <summary>
        /// Get the <see cref="CompositionClip"/> for the shadow's <see cref="SpriteVisual"/>
        /// </summary>
        protected virtual CompositionClip GetShadowClip(AttachedShadowElementContext context)
        {
            return null;
        }

        /// <summary>
        /// Update the mask that gives the shadow its shape
        /// </summary>
        protected void UpdateShadowMask(AttachedShadowElementContext context)
        {
            if (!context.IsInitialized)
            {
                return;
            }

            context.Shadow.Mask = GetShadowMask(context);
        }

        /// <summary>
        /// Update the clipping on the shadow's <see cref="SpriteVisual"/>
        /// </summary>
        protected void UpdateShadowClip(AttachedShadowElementContext context)
        {
            if (!context.IsInitialized)
            {
                return;
            }

            context.SpriteVisual.Clip = GetShadowClip(context);
        }

        /// <summary>
        /// Update the opacity mask for the shadow's <see cref="SpriteVisual"/>
        /// </summary>
        /// <param name="context"></param>
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
                context.RemoveResource(OpacityMaskResourceKey)?.Dispose();
            }
        }

        /// <summary>
        /// This method is called when a DependencyProperty is changed
        /// </summary>
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
        /// This method is called when the element size changes, and <see cref="SupportsOnSizeChangedEvent"/> = <see cref="bool">true</see>
        /// </summary>
        /// <param name="context"></param>
        /// <param name="newSize"></param>
        /// <param name="previousSize"></param>
        protected virtual void OnSizeChanged(AttachedShadowElementContext context, Size newSize, Size previousSize)
        {
        }
    }
}

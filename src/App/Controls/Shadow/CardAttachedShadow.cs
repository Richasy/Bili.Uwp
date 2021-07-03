// Copyright (c) Richasy. All rights reserved.

using System.Numerics;
using Microsoft.Graphics.Canvas.Geometry;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;

namespace Richasy.Bili.App.Controls.Shadow
{
    /// <summary>
    /// 卡片的附加阴影，提供圆角.
    /// </summary>
    public class CardAttachedShadow : AttachedShadow
    {
        /// <summary>
        /// Dependency property of <see cref="CornerRadius"/>.
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(double),
                typeof(CardAttachedShadow),
                new PropertyMetadata(8d, OnDependencyPropertyChanged));

        private const float MaxBlurRadius = 72;
        private static readonly string ClipResourceKey = "Clip";

        private static readonly string PathGeometryResourceKey = "PathGeometry";
        private static readonly string RoundedRectangleGeometryResourceKey = "RoundedGeometry";
        private static readonly string ShapeResourceKey = "Shape";
        private static readonly string ShapeVisualResourceKey = "ShapeVisual";
        private static readonly string SurfaceBrushResourceKey = "SurfaceBrush";
        private static readonly string VisualSurfaceResourceKey = "VisualSurface";

        /// <summary>
        /// Get or set the roundness of the shadow's corners.
        /// </summary>
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <inheritdoc/>
        public override bool IsSupported => ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 8);

        /// <inheritdoc/>
        public override bool SupportsOnSizeChangedEvent => true;

        /// <inheritdoc/>
        public override void OnElementContextUninitialized(AttachedShadowElementContext context)
        {
            context.ClearAndDisposeResources();
            base.OnElementContextUninitialized(context);
        }

        /// <inheritdoc/>
        protected override void OnPropertyChanged(AttachedShadowElementContext context, DependencyProperty property, object oldValue, object newValue)
        {
            if (property == CornerRadiusProperty)
            {
                var geometry = context.GetResource<CompositionRoundedRectangleGeometry>(RoundedRectangleGeometryResourceKey);
                if (geometry != null)
                {
                    geometry.CornerRadius = new Vector2((float)(double)newValue);
                }

                UpdateShadowClip(context);
            }
            else
            {
                base.OnPropertyChanged(context, property, oldValue, newValue);
            }
        }

        /// <inheritdoc/>
        protected override CompositionBrush GetShadowMask(AttachedShadowElementContext context)
        {
            // Create rounded rectangle geometry and add it to a shape
            var geometry = context.GetResource<CompositionRoundedRectangleGeometry>(RoundedRectangleGeometryResourceKey) ?? context.AddResource(
                RoundedRectangleGeometryResourceKey,
                context.Compositor.CreateRoundedRectangleGeometry());
            geometry.CornerRadius = new Vector2((float)CornerRadius);

            var shape = context.GetResource<CompositionSpriteShape>(ShapeResourceKey) ?? context.AddResource(ShapeResourceKey, context.Compositor.CreateSpriteShape(geometry));
            shape.FillBrush = context.Compositor.CreateColorBrush(Colors.Black);

            // Create a ShapeVisual so that our geometry can be rendered to a visual
            var shapeVisual = context.GetResource<ShapeVisual>(ShapeVisualResourceKey) ??
                              context.AddResource(ShapeVisualResourceKey, context.Compositor.CreateShapeVisual());
            shapeVisual.Shapes.Add(shape);

            // Create a CompositionVisualSurface, which renders our ShapeVisual to a texture
            var visualSurface = context.GetResource<CompositionVisualSurface>(VisualSurfaceResourceKey) ??
                                context.AddResource(VisualSurfaceResourceKey, context.Compositor.CreateVisualSurface());
            visualSurface.SourceVisual = shapeVisual;

            // Create a CompositionSurfaceBrush to render our CompositionVisualSurface to a brush.
            // Now we have a rounded rectangle brush that can be used on as the mask for our shadow.
            var surfaceBrush = context.GetResource<CompositionSurfaceBrush>(SurfaceBrushResourceKey) ?? context.AddResource(
                SurfaceBrushResourceKey,
                context.Compositor.CreateSurfaceBrush(visualSurface));

            geometry.Size = visualSurface.SourceSize = shapeVisual.Size = context.Element.RenderSize.ToVector2();

            return surfaceBrush;
        }

        /// <inheritdoc/>
        protected override CompositionClip GetShadowClip(AttachedShadowElementContext context)
        {
            var pathGeom = context.GetResource<CompositionPathGeometry>(PathGeometryResourceKey) ??
                           context.AddResource(PathGeometryResourceKey, context.Compositor.CreatePathGeometry());
            var clip = context.GetResource<CompositionGeometricClip>(ClipResourceKey) ?? context.AddResource(ClipResourceKey, context.Compositor.CreateGeometricClip(pathGeom));

            // Create rounded rectangle geometry at a larger size that compensates for the size of the stroke,
            // as we want the inside edge of the stroke to match the edges of the element.
            // Additionally, the inside edge of the stroke will have a smaller radius than the radius we specified.
            // Using "(StrokeThickness / 2) + Radius" as our rectangle's radius will give us an inside stroke radius that matches the radius we want.
            var canvasRectangle = CanvasGeometry.CreateRoundedRectangle(
                null,
                -MaxBlurRadius / 2,
                -MaxBlurRadius / 2,
                (float)context.Element.ActualWidth + MaxBlurRadius,
                (float)context.Element.ActualHeight + MaxBlurRadius,
                (MaxBlurRadius / 2) + (float)CornerRadius,
                (MaxBlurRadius / 2) + (float)CornerRadius);

            var canvasStroke = canvasRectangle.Stroke(MaxBlurRadius);

            pathGeom.Path = new CompositionPath(canvasStroke);

            return clip;
        }

        /// <inheritdoc/>
        protected override void OnSizeChanged(AttachedShadowElementContext context, Size newSize, Size previousSize)
        {
            var sizeAsVec2 = newSize.ToVector2();

            var geometry = context.GetResource<CompositionRoundedRectangleGeometry>(RoundedRectangleGeometryResourceKey);
            if (geometry != null)
            {
                geometry.Size = sizeAsVec2;
            }

            var visualSurface = context.GetResource<CompositionVisualSurface>(VisualSurfaceResourceKey);
            if (geometry != null)
            {
                visualSurface.SourceSize = sizeAsVec2;
            }

            var shapeVisual = context.GetResource<ShapeVisual>(ShapeVisualResourceKey);
            if (geometry != null)
            {
                shapeVisual.Size = sizeAsVec2;
            }

            UpdateShadowClip(context);

            base.OnSizeChanged(context, newSize, previousSize);
        }
    }
}

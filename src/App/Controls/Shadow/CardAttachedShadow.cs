using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Richasy.Bili.App.Controls.Shadow
{
    public class RoundedRectangleAttachedShadow : AttachedShadow
    {
        private const float MaxBlurRadius = 72;
        private static readonly TypedResourceKey<CompositionGeometricClip> ClipResourceKey = "Clip";

        private static readonly TypedResourceKey<CompositionPathGeometry> PathGeometryResourceKey = "PathGeometry";
        private static readonly TypedResourceKey<CompositionRoundedRectangleGeometry> RoundedRectangleGeometryResourceKey = "RoundedGeometry";
        private static readonly TypedResourceKey<CompositionSpriteShape> ShapeResourceKey = "Shape";
        private static readonly TypedResourceKey<ShapeVisual> ShapeVisualResourceKey = "ShapeVisual";
        private static readonly TypedResourceKey<CompositionSurfaceBrush> SurfaceBrushResourceKey = "SurfaceBrush";
        private static readonly TypedResourceKey<CompositionVisualSurface> VisualSurfaceResourceKey = "VisualSurface";

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(
                nameof(CornerRadius),
                typeof(double),
                typeof(RoundedRectangleAttachedShadow),
                new PropertyMetadata(8d, OnDependencyPropertyChanged));

        /// <summary>
        /// Get or set the roundness of the shadow's corners
        /// </summary>
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public override bool IsSupported => ApiInformationHelper.Is19H1ContractPresent;

        public override bool SupportsOnSizeChangedEvent => true;

        public override void OnElementContextUninitialized(AttachedShadowElementContext context)
        {
            context.ClearAndDisposeResources();
            base.OnElementContextUninitialized(context);
        }

        protected override void OnPropertyChanged(AttachedShadowElementContext context, DependencyProperty property, object oldValue, object newValue)
        {
            if (property == CornerRadiusProperty)
            {
                var geometry = context.GetResource(RoundedRectangleGeometryResourceKey);
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

        protected override CompositionBrush GetShadowMask(AttachedShadowElementContext context)
        {
            // Create rounded rectangle geometry and add it to a shape
            var geometry = context.GetResource(RoundedRectangleGeometryResourceKey) ?? context.AddResource(
                RoundedRectangleGeometryResourceKey,
                context.Compositor.CreateRoundedRectangleGeometry());
            geometry.CornerRadius = new Vector2((float)CornerRadius);

            var shape = context.GetResource(ShapeResourceKey) ?? context.AddResource(ShapeResourceKey, context.Compositor.CreateSpriteShape(geometry));
            shape.FillBrush = context.Compositor.CreateColorBrush(Colors.Black);

            // Create a ShapeVisual so that our geometry can be rendered to a visual
            var shapeVisual = context.GetResource(ShapeVisualResourceKey) ??
                              context.AddResource(ShapeVisualResourceKey, context.Compositor.CreateShapeVisual());
            shapeVisual.Shapes.Add(shape);

            // Create a CompositionVisualSurface, which renders our ShapeVisual to a texture
            var visualSurface = context.GetResource(VisualSurfaceResourceKey) ??
                                context.AddResource(VisualSurfaceResourceKey, context.Compositor.CreateVisualSurface());
            visualSurface.SourceVisual = shapeVisual;

            // Create a CompositionSurfaceBrush to render our CompositionVisualSurface to a brush.
            // Now we have a rounded rectangle brush that can be used on as the mask for our shadow.
            var surfaceBrush = context.GetResource(SurfaceBrushResourceKey) ?? context.AddResource(
                SurfaceBrushResourceKey,
                context.Compositor.CreateSurfaceBrush(visualSurface));

            geometry.Size = visualSurface.SourceSize = shapeVisual.Size = context.Element.RenderSize.ToVector2();

            return surfaceBrush;
        }

        protected override CompositionClip GetShadowClip(AttachedShadowElementContext context)
        {
            var pathGeom = context.GetResource(PathGeometryResourceKey) ??
                           context.AddResource(PathGeometryResourceKey, context.Compositor.CreatePathGeometry());
            var clip = context.GetResource(ClipResourceKey) ?? context.AddResource(ClipResourceKey, context.Compositor.CreateGeometricClip(pathGeom));

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

        protected override void OnSizeChanged(AttachedShadowElementContext context, Size newSize, Size previousSize)
        {
            var sizeAsVec2 = newSize.ToVector2();

            var geometry = context.GetResource(RoundedRectangleGeometryResourceKey);
            if (geometry != null)
            {
                geometry.Size = sizeAsVec2;
            }

            var visualSurface = context.GetResource(VisualSurfaceResourceKey);
            if (geometry != null)
            {
                visualSurface.SourceSize = sizeAsVec2;
            }

            var shapeVisual = context.GetResource(ShapeVisualResourceKey);
            if (geometry != null)
            {
                shapeVisual.Size = sizeAsVec2;
            }

            UpdateShadowClip(context);

            base.OnSizeChanged(context, newSize, previousSize);
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Numerics;
using Windows.Foundation;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using AnimationContext = Microsoft.UI.Xaml.Controls.AnimationContext;
using ElementAnimator = Microsoft.UI.Xaml.Controls.ElementAnimator;

namespace Richasy.Bili.App.Controls.Animator
{
#pragma warning disable CS8305 // Type is for evaluation purposes only and is subject to change or removal in future updates.
    /// <summary>
    /// Card Collection Animator.
    /// </summary>
    public class CardCollectionAnimator : ElementAnimator
    {
        private const double DefaultAnimationDurationInMs = 200.0;
        private const double OffsetAnimationDurationInMs = 250.0;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardCollectionAnimator"/> class.
        /// </summary>
        public CardCollectionAnimator()
        {
            AnimationSlowdownFactor = 1.0;
        }

        /// <summary>
        /// Animation slow down factor.
        /// </summary>
        public static double AnimationSlowdownFactor { get; set; }

        /// <inheritdoc/>
        protected override bool HasShowAnimationCore(UIElement element, AnimationContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        protected override bool HasHideAnimationCore(UIElement element, AnimationContext context)
        {
            return true;
        }

        /// <inheritdoc/>
        protected override bool HasBoundsChangeAnimationCore(
            UIElement element,
            AnimationContext context,
            Rect oldBounds,
            Rect newBounds)
        {
            return true;
        }

        /// <inheritdoc/>
        protected override void StartShowAnimation(UIElement element, AnimationContext context)
        {
            var visual = ElementCompositionPreview.GetElementVisual(element);
            var compositor = visual.Compositor;

            var fadeInAnimation = compositor.CreateScalarKeyFrameAnimation();
            fadeInAnimation.InsertKeyFrame(0.0f, 0.0f);

            if (this.HasBoundsChangeAnimationsPending && this.HasHideAnimationsPending)
            {
                fadeInAnimation.InsertKeyFrame(0.66f, 0.0f);
            }
            else if (this.HasBoundsChangeAnimationsPending || this.HasHideAnimationsPending)
            {
                fadeInAnimation.InsertKeyFrame(0.5f, 0.0f);
            }

            fadeInAnimation.InsertKeyFrame(1.0f, 1.0f);
            fadeInAnimation.Duration = TimeSpan.FromMilliseconds(
                DefaultAnimationDurationInMs * ((this.HasHideAnimationsPending ? 1 : 0) + (this.HasBoundsChangeAnimationsPending ? 1 : 0) + 1) * AnimationSlowdownFactor);

            var batch = compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            visual.StartAnimation("Opacity", fadeInAnimation);
            batch.End();
            batch.Completed += (sender, args) => { this.OnShowAnimationCompleted(element); };
        }

        /// <inheritdoc/>
        protected override void StartHideAnimation(UIElement element, AnimationContext context)
        {
            var visual = ElementCompositionPreview.GetElementVisual(element);
            var compositor = visual.Compositor;

            var fadeOutAnimation = compositor.CreateScalarKeyFrameAnimation();
            fadeOutAnimation.InsertExpressionKeyFrame(0.0f, "this.CurrentValue");
            fadeOutAnimation.InsertKeyFrame(1.0f, 0.0f);
            fadeOutAnimation.Duration = TimeSpan.FromMilliseconds(DefaultAnimationDurationInMs * AnimationSlowdownFactor);

            var batch = compositor.CreateScopedBatch(CompositionBatchTypes.Animation);
            visual.StartAnimation("Opacity", fadeOutAnimation);
            batch.End();
            batch.Completed += (sender, args) =>
            {
                visual.Opacity = 1.0f;
                this.OnHideAnimationCompleted(element);
            };
        }

        /// <inheritdoc/>
        protected override void StartBoundsChangeAnimation(UIElement element, AnimationContext context, Rect oldBounds, Rect newBounds)
        {
            var visual = ElementCompositionPreview.GetElementVisual(element);
            var compositor = visual.Compositor;
            var offsetBatch = compositor.CreateScopedBatch(CompositionBatchTypes.Animation);

            // Animate offset.
            if (oldBounds.X != newBounds.X ||
                oldBounds.Y != newBounds.Y)
            {
                this.AnimateOffset(element, visual, compositor, oldBounds, newBounds);
            }

            offsetBatch.End();
            offsetBatch.Completed += (sender, args) =>
            {
                this.OnBoundsChangeAnimationCompleted(element);
            };
        }

        private void AnimateOffset(UIElement element, Visual visual, Compositor compositor, Rect oldBounds, Rect newBounds)
        {
            var offsetAnimation = compositor.CreateVector2KeyFrameAnimation();

            var startX = 0f;
            var startY = 0f;
            if (oldBounds.Y != newBounds.Y)
            {
                if (oldBounds.X != newBounds.X)
                {
                    var offset = (float)(oldBounds.Width / 2);
                    startX = oldBounds.X == 0 ? offset : (-1) * offset;
                }
                else
                {
                    startY = (float)(oldBounds.Y - newBounds.Y);
                }
            }
            else
            {
                startX = (float)(oldBounds.X - newBounds.X);
            }

            offsetAnimation.SetVector2Parameter("start", new Vector2(startX, startY));
            offsetAnimation.SetVector2Parameter("final", default);
            offsetAnimation.InsertExpressionKeyFrame(0.0f, "start");
            offsetAnimation.InsertExpressionKeyFrame(1.0f, "final");
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(
                OffsetAnimationDurationInMs * AnimationSlowdownFactor);

            visual.StartAnimation("TransformMatrix._41_42", offsetAnimation);
        }
    }
#pragma warning restore CS8305 // Type is for evaluation purposes only and is subject to change or removal in future updates.
}

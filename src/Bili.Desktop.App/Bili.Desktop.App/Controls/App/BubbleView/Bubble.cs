// Copyright (c) Richasy. All rights reserved.

using System;
using System.Numerics;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Composition;

namespace Bili.Desktop.App.Controls
{
    /// <summary>
    /// 气泡.
    /// </summary>
    public class Bubble : IDisposable
    {
        private static readonly Random _rnd = new Random();
        private static CompositionEasingFunction _easing;

        private readonly Compositor _compositor;
        private readonly CanvasDevice _canvasDevice;
        private readonly CompositionGraphicsDevice _graphicsDevice;
        private SpriteVisual _visual;
        private CompositionAnimationGroup _animations;
        private Vector2 _size;
        private Vector3 _offset;
        private CompositionDrawingSurface _surface;
        private CompositionSurfaceBrush _brush;

        /// <summary>
        /// Initializes a new instance of the <see cref="Bubble"/> class.
        /// </summary>
        /// <param name="compositor"><see cref="Compositor"/>.</param>
        /// <param name="canvasDevice"><see cref="CanvasDevice"/>.</param>
        /// <param name="graphicsDevice"><see cref="CompositionGraphicsDevice"/>.</param>
        /// <param name="targetSize">目标大小.</param>
        /// <param name="color">气泡颜色.</param>
        /// <param name="duration">发散时长.</param>
        /// <param name="onTop">是否置顶.</param>
        /// <param name="size">大小.</param>
        /// <param name="isFill">是否填充.</param>
        public Bubble(
            Compositor compositor,
            CanvasDevice canvasDevice,
            CompositionGraphicsDevice graphicsDevice,
            Size targetSize,
            Color color,
            TimeSpan duration,
            bool onTop,
            Size? size = null,
            bool? isFill = null)
        {
            _compositor = compositor;
            _canvasDevice = canvasDevice;
            _graphicsDevice = graphicsDevice;
            _visual = compositor.CreateSpriteVisual();

            if (!isFill.HasValue)
            {
                var tmp = _rnd.Next(2);
                isFill = tmp > 0;
            }

            if (size.HasValue)
            {
                _size = size.Value.ToVector2();
            }
            else
            {
                var maxRadius = (int)Math.Min(targetSize.Width, targetSize.Height);
                if (isFill.Value)
                {
                    _size = new Vector2(_rnd.Next(maxRadius / 7, maxRadius / 4));
                }
                else
                {
                    _size = new Vector2(_rnd.Next(maxRadius / 6, maxRadius / 3));
                }
            }

            Draw(isFill.Value, color);

            _offset = new Vector3((float)targetSize.Width / 2, (float)targetSize.Height / 2, 0f);
            _visual.Size = _size;
            _visual.Offset = _offset;
            _visual.Scale = Vector3.Zero;
            CreateAnimation(targetSize, _visual.Offset, onTop, duration);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Bubble"/> class.
        /// </summary>
        ~Bubble()
        {
            Dispose(false);
        }

        /// <summary>
        /// 添加.
        /// </summary>
        /// <param name="container">目标容器.</param>
        public void AddTo(ContainerVisual container)
        {
            container.Children.InsertAtBottom(_visual);
        }

        /// <summary>
        /// 开始播放.
        /// </summary>
        public void Start()
        {
            if (_animations != null)
            {
                _visual.StopAnimationGroup(_animations);
                _visual.StartAnimationGroup(_animations);
            }
        }

        /// <inheritdoc/>
        public void Dispose() => Dispose(true);

        private void Draw(bool isFill, Color color)
        {
            _surface = _graphicsDevice.CreateDrawingSurface(_size.ToSize(), Windows.Graphics.DirectX.DirectXPixelFormat.B8G8R8A8UIntNormalized, Windows.Graphics.DirectX.DirectXAlphaMode.Premultiplied);
            using (var dc = CanvasComposition.CreateDrawingSession(_surface))
            {
                dc.Clear(Colors.Transparent);
                if (isFill)
                {
                    dc.FillCircle(_size / 2, _size.X / 2, color);
                }
                else
                {
                    dc.DrawCircle(_size / 2, (_size.X / 2) - 2, color, 2f);
                }

                dc.Flush();
            }

            _brush = _compositor.CreateSurfaceBrush(_surface);
            _visual.Brush = _brush;
        }

        private void CreateAnimation(Size targetSize, Vector3 startOffset, bool onTop, TimeSpan duration)
        {
            if (_easing == null)
            {
                _easing = _compositor.CreateCubicBezierEasingFunction(new Vector2(0.17f, 0.67f), new Vector2(0.44f, 0.999f));
            }

            _animations = _compositor.CreateAnimationGroup();

            var scalean = _compositor.CreateVector3KeyFrameAnimation();
            scalean.InsertKeyFrame(0f, Vector3.Zero);
            scalean.InsertKeyFrame(0.2f, Vector3.One, _easing);
            scalean.InsertKeyFrame(1f, new Vector3(0.06f, 0.06f, 1f), _easing);
            scalean.Duration = duration;
            scalean.Target = "Scale";
            scalean.StopBehavior = AnimationStopBehavior.SetToInitialValue;

            var offsetan = _compositor.CreateVector3KeyFrameAnimation();
            offsetan.InsertKeyFrame(0f, startOffset, _easing);
            if (onTop)
            {
                // 在上半部分
                offsetan.InsertKeyFrame(1f, new Vector3(_rnd.Next(-(int)targetSize.Width, (int)targetSize.Width) + ((int)targetSize.Width / 2), _rnd.Next(-(int)(targetSize.Height * 1.5), 0) + ((int)targetSize.Height / 2), 0f), _easing);
            }
            else
            {
                // 在下半部分
                offsetan.InsertKeyFrame(1f, new Vector3(_rnd.Next(-(int)targetSize.Width, (int)targetSize.Width) + ((int)targetSize.Width / 2), _rnd.Next(0, (int)(targetSize.Height * 1.5)) + ((int)targetSize.Height / 2), 0f), _easing);
            }

            offsetan.Duration = duration;
            offsetan.Target = "Offset";
            offsetan.StopBehavior = AnimationStopBehavior.SetToInitialValue;

            _animations.Add(scalean);
            _animations.Add(offsetan);
        }

        private void Dispose(bool isDisposing)
        {
            _visual?.Dispose();
            _visual = null;

            _brush?.Dispose();
            _brush = null;

            _surface?.Dispose();
            _surface = null;

            _animations?.Dispose();
            _animations = null;

            if (isDisposing)
            {
                GC.SuppressFinalize(this);
            }
        }
    }
}

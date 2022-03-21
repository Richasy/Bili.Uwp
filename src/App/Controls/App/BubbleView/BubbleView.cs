// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.UI.Composition;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 气泡发生器.
    /// </summary>
    public partial class BubbleView : Control
    {
        /// <summary>
        /// <see cref="IsDisposeWhenUnloaded"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsDisposeWhenUnloadedProperty =
            DependencyProperty.Register(nameof(IsDisposeWhenUnloaded), typeof(bool), typeof(BubbleView), new PropertyMetadata(false));

        /// <summary>
        /// Initializes a new instance of the <see cref="BubbleView"/> class.
        /// </summary>
        public BubbleView()
        {
            DefaultStyleKey = typeof(BubbleView);
            _foregroundPropertyChangedToken = RegisterPropertyChangedCallback(ForegroundProperty, ForegroundPropertyChanged);
            Unloaded += OnBubbleViewUnloaded;
            Loaded += OnBubbleViewLoaded;
        }

        /// <summary>
        /// 是否在控件卸载时释放.
        /// </summary>
        public bool IsDisposeWhenUnloaded
        {
            get { return (bool)GetValue(IsDisposeWhenUnloadedProperty); }
            set { SetValue(IsDisposeWhenUnloadedProperty, value); }
        }

        /// <summary>
        /// 发射气泡.
        /// </summary>
        public void ShowBubbles()
        {
            CreateBubbles();
            if (_bubbles != null)
            {
                foreach (var bubble in _bubbles)
                {
                    bubble.Start();
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _bubbleHost = GetTemplateChild("BubbleHost") as Rectangle;
            _bubbleHost.SizeChanged += OnBubbleHostSizeChanged;
            _isApplied = true;
            Setup();
        }

        private void Setup()
        {
            if (_isLoaded && _isApplied)
            {
                SetupComposition();
                SetupDevices();
            }
        }

        private void OnBubbleViewLoaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;
            Setup();
        }

        private void SetupComposition()
        {
            _hostVisual = ElementCompositionPreview.GetElementVisual(_bubbleHost);
            _compositor = _hostVisual.Compositor;

            _bubblesVisual = _compositor.CreateContainerVisual();
            _bubblesVisual.BindSize(_hostVisual);

            ElementCompositionPreview.SetElementChildVisual(_bubbleHost, _bubblesVisual);
        }

        /// <summary>
        /// 初始化CanvasDevice和GraphicsDevice.
        /// </summary>
        private void SetupDevices()
        {
            DisplayInformation.DisplayContentsInvalidated += OnDisplayInformationDisplayContentsInvalidated;

            _canvasDevice = CanvasDevice.GetSharedDevice();
            _graphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(_compositor, _canvasDevice);

            _canvasDevice.DeviceLost += OnCanvasDeviceDeviceLost;
            _graphicsDevice.RenderingDeviceReplaced += OnGraphicsDeviceRenderingDeviceReplaced;
        }

        /// <summary>
        /// 重新设置CanvasDevice和GraphicsDevice.
        /// </summary>
        /// <param name="isDeviceLost">设备是否丢失.</param>
        private void ResetDevices(bool isDeviceLost)
        {
            try
            {
                if (isDeviceLost)
                {
                    _canvasDevice.DeviceLost -= OnCanvasDeviceDeviceLost;
                    _canvasDevice = CanvasDevice.GetSharedDevice();
                    _canvasDevice.DeviceLost += OnCanvasDeviceDeviceLost;
                }

                // 重新设置GraphicsDevice，在CanvasDevice丢失时会触发异常
                CanvasComposition.SetCanvasDevice(_graphicsDevice, _canvasDevice);
            }
            catch (Exception e) when (_canvasDevice != null && _canvasDevice.IsDeviceLost(e.HResult))
            {
                // 通知设备已丢失，并触发CanvasDevice.DeviceLost
                _canvasDevice.RaiseDeviceLost();
            }
        }

        private void ClearBubbles()
        {
            if (_bubbles != null)
            {
                foreach (var bubble in _bubbles)
                {
                    bubble.Dispose();
                }

                _bubbles.Clear();
                _bubbles = null;
            }

            if (_bubblesVisual != null)
            {
                _bubblesVisual.Children.RemoveAll();
            }
        }

        private void CreateBubbles()
        {
            ClearBubbles();
            if (DesignMode.DesignModeEnabled)
            {
                return;
            }
            else if (_canvasDevice == null || _graphicsDevice == null)
            {
                return;
            }

            if (_foregroundColor != Colors.Transparent && ActualWidth > 0 && ActualHeight > 0)
            {
                var count = 20;
                _bubbles = new List<Bubble>();
                var duration = TimeSpan.FromSeconds(1d);

                for (var i = 0; i < count; i++)
                {
                    var bubble = new Bubble(
                        _compositor,
                        _canvasDevice,
                        _graphicsDevice,
                        new Size(ActualWidth, ActualHeight),
                        _foregroundColor,
                        duration,
                        true);
                    bubble.AddTo(_bubblesVisual);
                    _bubbles.Add(bubble);
                }

                for (var i = 0; i < count; i++)
                {
                    var bubble = new Bubble(
                        _compositor,
                        _canvasDevice,
                        _graphicsDevice,
                        new Size(ActualWidth, ActualHeight),
                        _foregroundColor,
                        duration,
                        false);
                    bubble.AddTo(_bubblesVisual);
                    _bubbles.Add(bubble);
                }
            }
        }

        private void OnDisplayInformationDisplayContentsInvalidated(DisplayInformation sender, object args)
        {
            ResetDevices(false);
        }

        private void OnCanvasDeviceDeviceLost(CanvasDevice sender, object args)
        {
            ResetDevices(true);
        }

        private void OnGraphicsDeviceRenderingDeviceReplaced(CompositionGraphicsDevice sender, RenderingDeviceReplacedEventArgs args)
        {
            CreateBubbles();
        }

        private void OnBubbleHostSizeChanged(object sender, SizeChangedEventArgs e)
        {
            CreateBubbles();
        }

        private void OnBubbleViewUnloaded(object sender, RoutedEventArgs e)
        {
            if (IsDisposeWhenUnloaded)
            {
                ClearBubbles();
                _bubblesVisual?.Dispose();
                _bubblesVisual = null;
                _canvasDevice = null;
                _graphicsDevice?.Dispose();
                _graphicsDevice = null;
            }
        }

        private void ForegroundPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            if (Foreground is SolidColorBrush brush)
            {
                _foregroundColor = brush.Color;
            }

            CreateBubbles();
        }
    }
}

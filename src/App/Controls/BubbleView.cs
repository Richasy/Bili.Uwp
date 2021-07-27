// Copyright (c) Richasy. All rights reserved.

using Microsoft.Graphics.Canvas.UI.Composition;
using Microsoft.Graphics.Canvas;
using System.Collections.Generic;
using System;
using Windows.ApplicationModel;
using Windows.Graphics.Display;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 气泡发生器.
    /// </summary>
    public partial class BubbleView : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BubbleView"/> class.
        /// </summary>
        public BubbleView()
        {
            this.DefaultStyleKey = typeof(BubbleView);
            _foregroundPropertyChangedToken = RegisterPropertyChangedCallback(ForegroundProperty, ForegroundPropertyChanged);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            BubbleHost = GetTemplateChild("BubbleHost") as Rectangle;
            BubbleHost.SizeChanged += BubbleHost_SizeChanged;
            SetupComposition();
            SetupDevices();
        }

        private void SetupComposition()
        {
            _HostVisual = ElementCompositionPreview.GetElementVisual(BubbleHost);
            _Compositor = _HostVisual.Compositor;

            _BubblesVisual = _Compositor.CreateContainerVisual();
            _BubblesVisual.BindSize(_HostVisual);

            ElementCompositionPreview.SetElementChildVisual(BubbleHost, _BubblesVisual);
}

//初始化CanvasDevice和GraphicsDevice
        private void SetupDevices()
        {
            DisplayInformation.DisplayContentsInvalidated += DisplayInformation_DisplayContentsInvalidated;

            _canvasDevice = CanvasDevice.GetSharedDevice();
            _graphicsDevice = CanvasComposition.CreateCompositionGraphicsDevice(_Compositor, _canvasDevice);

            _canvasDevice.DeviceLost += _canvasDevice_DeviceLost;
            _graphicsDevice.RenderingDeviceReplaced += _graphicsDevice_RenderingDeviceReplaced;
        }

        //重新设置CanvasDevice和GraphicsDevice
        private void ResetDevices(bool IsDeviceLost)
        {
            try
            {
                if (IsDeviceLost)
                {
                    _canvasDevice.DeviceLost -= _canvasDevice_DeviceLost;
                    _canvasDevice = CanvasDevice.GetSharedDevice();
                    _canvasDevice.DeviceLost += _canvasDevice_DeviceLost;
                }
                //重新设置GraphicsDevice，在CanvasDevice丢失时会触发异常
                CanvasComposition.SetCanvasDevice(_graphicsDevice, _canvasDevice);
            }
            catch (Exception e) when (_canvasDevice != null && _canvasDevice.IsDeviceLost(e.HResult))
            {
                //通知设备已丢失，并触发CanvasDevice.DeviceLost
                _canvasDevice.RaiseDeviceLost();
            }
        }

        private void ClearBubbles()
        {
            if (Bubbles != null)
            {
                foreach (var bubble in Bubbles)
                {
                    bubble.Dispose();
                }
                Bubbles.Clear();
                Bubbles = null;
            }

            if (_BubblesVisual != null)
            {
                _BubblesVisual.Children.RemoveAll();
            }
        }

        private void CreateBubbles()
        {
            ClearBubbles();
            if (DesignMode.DesignModeEnabled)
                return;
            if (_canvasDevice == null || _graphicsDevice == null)
                return;
            if (ForegroundColor != Colors.Transparent && this.ActualWidth > 0 && this.ActualHeight > 0)
{
var count = 20;

                var _Bubbles = new List<Bubble>();

                var duration = TimeSpan.FromSeconds(1d);

                for (int i = 0; i < count; i++)
                {
                    var bubble = new Bubble(_Compositor, _canvasDevice, _graphicsDevice, new Size(this.ActualWidth, this.ActualHeight), ForegroundColor, duration, true);
bubble.AddTo(_BubblesVisual);
                    _Bubbles.Add(bubble);
                }
                for (int i = 0; i < count; i++)
                {
                    var bubble = new Bubble(_Compositor, _canvasDevice, _graphicsDevice, new Size(this.ActualWidth, this.ActualHeight), ForegroundColor, duration, false);
                    bubble.AddTo(_BubblesVisual);
                    _Bubbles.Add(bubble);
                }
                Bubbles = _Bubbles;
            }
        }

        public void ShowBubbles()
        {
            if (Bubbles != null)
            {
                foreach (var bubble in Bubbles)
                {
                    bubble.Start();
                }
            }
        }

        //当显示需要重绘时，尝试重新给GraphicsDevice设置CanvasDevice，如果抛出异常，则说明CanvasDevice已丢失
        private void DisplayInformation_DisplayContentsInvalidated(DisplayInformation sender, object args)
        {
            System.Diagnostics.Debug.WriteLine("Display Contents Invalidated");
            ResetDevices(false);
        }

        //当设备丢失时，重新设置设备
        private void _canvasDevice_DeviceLost(CanvasDevice sender, object args)
        {
            ResetDevices(true);
        }

        //GraphicsDevice的绘制设备重置时，即触发CanvasComposition.SetCanvasDevice时，重新绘制气泡
        private void _graphicsDevice_RenderingDeviceReplaced(CompositionGraphicsDevice sender, RenderingDeviceReplacedEventArgs args)
        {
            CreateBubbles();
        }

        private void BubbleHost_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            CreateBubbles();
        }

        private void BubbleView_Unloaded(object sender, RoutedEventArgs e)
        {
            ClearBubbles();
            _BubblesVisual?.Dispose();
            _BubblesVisual = null;
            _canvasDevice = null;
            _graphicsDevice?.Dispose();
            _graphicsDevice = null;
        }

        private void ForegroundPropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            if (Foreground is SolidColorBrush brush)
            {
                ForegroundColor = brush.Color;
            }
            CreateBubbles();
        }
    }
}

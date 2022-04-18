// Copyright (c) Richasy. All rights reserved.

using System;
using Atelier39;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 弹幕控件.
    /// </summary>
    public sealed partial class DanmakuView : Control
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Danmaku"/> class.
        /// </summary>
        public DanmakuView()
        {
            DefaultStyleKey = typeof(DanmakuView);
            DanmakuBold = false;
            DanmakuFontFamily = string.Empty;
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _rootGrid = GetTemplateChild(RootGridName) as Grid;

            if (_danmakuController == null)
            {
                InitializeController();
            }

            _isApplyTemplate = true;
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
            => base.MeasureOverride(availableSize);

        private static void OnDanmakuDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = Convert.ToInt32(e.NewValue);
            if (value <= 0)
            {
                value = 1;
            }

            var instance = (DanmakuView)d;
            instance.DanmakuDuration = value;
            instance._danmakuController?.SetRollingSpeed(value * 5);
        }

        private static void OnDanmakuAreaChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = Convert.ToDouble(e.NewValue);
            if (value <= 0)
            {
                value = 0.1;
            }

            if (value > 1)
            {
                value = 1;
            }

            var instance = (DanmakuView)d;
            instance.DanmakuArea = value;
            instance._danmakuController?.SetRollingAreaRatio(Convert.ToInt32(value * 10));
        }

        private void InitializeController()
        {
            _rootGrid.Children.Clear();
            _canvas = new CanvasAnimatedControl();
            _rootGrid.Children.Add(_canvas);
            _danmakuController = new DanmakuFrostMaster(_canvas);

            _danmakuController.SetAutoControlDensity(false);
            _danmakuController.SetRollingDensity(-1);
            _danmakuController.SetBorderColor(Colors.Gray);
            _danmakuController.SetRollingAreaRatio(Convert.ToInt32(DanmakuArea * 10));
        }
    }
}

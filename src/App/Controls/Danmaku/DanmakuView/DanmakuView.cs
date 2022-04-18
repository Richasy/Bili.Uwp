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
        protected override async void OnApplyTemplate()
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
            var instance = (DanmakuView)d;
            var speed = (double)e.NewValue * 5;
            instance._danmakuController?.SetRollingSpeed(Convert.ToInt32(speed));
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

        private static void OnDanmakuSizeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as DanmakuView;
            instance._danmakuController?.SetDanmakuFontSizeOffset(instance.GetFontSize((double)e.NewValue));
        }

        private static void OnDanmakuFontFamilyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as DanmakuView;
            instance._danmakuController?.SetFontFamilyName(e.NewValue?.ToString() ?? "Segoe UI");
        }

        private static void OnDanmakuBoldChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as DanmakuView;
            instance._danmakuController?.SetIsTextBold(Convert.ToBoolean(e.NewValue));
        }

        private static void OnIsDanmakuLimitChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as DanmakuView;
            instance._danmakuController?.SetAutoControlDensity((bool)e.NewValue);
        }

        private DanmakuFontSize GetFontSize(double fontSize)
        {
            switch (fontSize)
            {
                case 0.5:
                    return DanmakuFontSize.Smallest;
                case 1:
                    return DanmakuFontSize.Smaller;
                case 1.5:
                default:
                    return DanmakuFontSize.Normal;
                case 2.0:
                    return DanmakuFontSize.Larger;
                case 2.5:
                    return DanmakuFontSize.Largest;
            }
        }

        private void InitializeController()
        {
            _rootGrid.Children.Clear();
            _canvas = new CanvasAnimatedControl();
            _rootGrid.Children.Add(_canvas);
            _danmakuController = new DanmakuFrostMaster(_canvas);

            _danmakuController.SetAutoControlDensity(IsDanmakuLimit);
            _danmakuController.SetRollingDensity(-1);
            _danmakuController.SetBorderColor(Colors.Gray);
            _danmakuController.SetRollingAreaRatio(Convert.ToInt32(DanmakuArea * 10));
            _danmakuController.SetDanmakuFontSizeOffset(GetFontSize(DanmakuSize));
            _danmakuController.SetFontFamilyName(DanmakuFontFamily ?? "Segoe UI");
            _danmakuController.SetRollingSpeed(Convert.ToInt32(DanmakuDuration * 5));
            _danmakuController.SetIsTextBold(DanmakuBold);
        }
    }
}

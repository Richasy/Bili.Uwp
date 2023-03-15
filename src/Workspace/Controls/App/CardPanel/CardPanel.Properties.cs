// Copyright (c) Richasy. All rights reserved.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace Bili.Workspace.Controls
{
    /// <summary>
    /// 卡片面板.
    /// </summary>
    public partial class CardPanel
    {
#pragma warning disable SA1600 // Elements should be documented
        public static readonly DependencyProperty IsEnableCheckProperty =
            DependencyProperty.Register(nameof(IsEnableCheck), typeof(bool), typeof(CardPanel), new PropertyMetadata(false));

        public static readonly DependencyProperty PointerOverBorderBrushProperty =
            DependencyProperty.Register(nameof(PointerOverBorderBrush), typeof(Brush), typeof(CardPanel), new PropertyMetadata(default));

        public static readonly DependencyProperty PointerOverBackgroundProperty =
            DependencyProperty.Register(nameof(PointerOverBackground), typeof(Brush), typeof(CardPanel), new PropertyMetadata(default));

        public static readonly DependencyProperty PressedBorderBrushProperty =
            DependencyProperty.Register(nameof(PressedBorderBrush), typeof(Brush), typeof(CardPanel), new PropertyMetadata(default));

        public static readonly DependencyProperty PressedBackgroundProperty =
            DependencyProperty.Register(nameof(PressedBackground), typeof(Brush), typeof(CardPanel), new PropertyMetadata(default));

        public static readonly DependencyProperty DisabledBorderBrushProperty =
            DependencyProperty.Register(nameof(DisabledBorderBrush), typeof(Brush), typeof(CardPanel), new PropertyMetadata(default));

        public static readonly DependencyProperty DisabledBackgroundProperty =
            DependencyProperty.Register(nameof(DisabledBackground), typeof(Brush), typeof(CardPanel), new PropertyMetadata(default));

        public static readonly DependencyProperty CheckedBorderBrushProperty =
            DependencyProperty.Register(nameof(CheckedBorderBrush), typeof(Brush), typeof(CardPanel), new PropertyMetadata(default));

        public static readonly DependencyProperty CheckedBackgroundProperty =
            DependencyProperty.Register(nameof(CheckedBackground), typeof(Brush), typeof(CardPanel), new PropertyMetadata(default));

        public static readonly DependencyProperty CheckedPointerOverBorderBrushProperty =
            DependencyProperty.Register(nameof(CheckedPointerOverBorderBrush), typeof(Brush), typeof(CardPanel), new PropertyMetadata(default));

        public static readonly DependencyProperty CheckedPointerOverBackgroundProperty =
            DependencyProperty.Register(nameof(CheckedPointerOverBackground), typeof(Brush), typeof(CardPanel), new PropertyMetadata(default));

        public static readonly DependencyProperty CheckedPressedBorderBrushProperty =
            DependencyProperty.Register(nameof(CheckedPressedBorderBrush), typeof(Brush), typeof(CardPanel), new PropertyMetadata(default));

        public static readonly DependencyProperty CheckedPressedBackgroundProperty =
            DependencyProperty.Register(nameof(CheckedPressedBackground), typeof(Brush), typeof(CardPanel), new PropertyMetadata(default));

        public static readonly DependencyProperty CheckedDisabledBorderBrushProperty =
            DependencyProperty.Register(nameof(CheckedDisabledBorderBrush), typeof(Brush), typeof(CardPanel), new PropertyMetadata(default));

        public static readonly DependencyProperty CheckedDisabledBackgroundProperty =
            DependencyProperty.Register(nameof(CheckedDisabledBackground), typeof(Brush), typeof(CardPanel), new PropertyMetadata(default));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register(nameof(StrokeThickness), typeof(double), typeof(CardPanel), new PropertyMetadata(1d));

        /// <summary>
        /// 是否支持选中.
        /// </summary>
        public bool IsEnableCheck
        {
            get { return (bool)GetValue(IsEnableCheckProperty); }
            set { SetValue(IsEnableCheckProperty, value); }
        }

        public Brush PointerOverBorderBrush
        {
            get { return (Brush)GetValue(PointerOverBorderBrushProperty); }
            set { SetValue(PointerOverBorderBrushProperty, value); }
        }

        public Brush PointerOverBackground
        {
            get { return (Brush)GetValue(PointerOverBackgroundProperty); }
            set { SetValue(PointerOverBackgroundProperty, value); }
        }

        public Brush PressedBorderBrush
        {
            get { return (Brush)GetValue(PressedBorderBrushProperty); }
            set { SetValue(PressedBorderBrushProperty, value); }
        }

        public Brush PressedBackground
        {
            get { return (Brush)GetValue(PressedBackgroundProperty); }
            set { SetValue(PressedBackgroundProperty, value); }
        }

        public Brush DisabledBorderBrush
        {
            get { return (Brush)GetValue(DisabledBorderBrushProperty); }
            set { SetValue(DisabledBorderBrushProperty, value); }
        }

        public Brush DisabledBackground
        {
            get { return (Brush)GetValue(DisabledBackgroundProperty); }
            set { SetValue(DisabledBackgroundProperty, value); }
        }

        public Brush CheckedBorderBrush
        {
            get { return (Brush)GetValue(CheckedBorderBrushProperty); }
            set { SetValue(CheckedBorderBrushProperty, value); }
        }

        public Brush CheckedBackground
        {
            get { return (Brush)GetValue(CheckedBackgroundProperty); }
            set { SetValue(CheckedBackgroundProperty, value); }
        }

        public Brush CheckedPointerOverBorderBrush
        {
            get { return (Brush)GetValue(CheckedPointerOverBorderBrushProperty); }
            set { SetValue(CheckedPointerOverBorderBrushProperty, value); }
        }

        public Brush CheckedPointerOverBackground
        {
            get { return (Brush)GetValue(CheckedPointerOverBackgroundProperty); }
            set { SetValue(CheckedPointerOverBackgroundProperty, value); }
        }

        public Brush CheckedPressedBorderBrush
        {
            get { return (Brush)GetValue(CheckedPressedBorderBrushProperty); }
            set { SetValue(CheckedPressedBorderBrushProperty, value); }
        }

        public Brush CheckedPressedBackground
        {
            get { return (Brush)GetValue(CheckedPressedBackgroundProperty); }
            set { SetValue(CheckedPressedBackgroundProperty, value); }
        }

        public Brush CheckedDisabledBorderBrush
        {
            get { return (Brush)GetValue(CheckedDisabledBorderBrushProperty); }
            set { SetValue(CheckedDisabledBorderBrushProperty, value); }
        }

        public Brush CheckedDisabledBackground
        {
            get { return (Brush)GetValue(CheckedDisabledBackgroundProperty); }
            set { SetValue(CheckedDisabledBackgroundProperty, value); }
        }

        /// <summary>
        /// 边框厚度.
        /// </summary>
        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value); }
        }
#pragma warning restore SA1600 // Elements should be documented
    }
}

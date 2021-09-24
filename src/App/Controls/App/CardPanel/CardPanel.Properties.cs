// Copyright (c) GodLeaveMe. All rights reserved.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 卡片面板.
    /// </summary>
    public partial class CardPanel
    {
#pragma warning disable SA1600 // Elements should be documented
        public static readonly DependencyProperty IsEnableHoverAnimationProperty =
            DependencyProperty.Register(nameof(IsEnableHoverAnimation), typeof(bool), typeof(CardPanel), new PropertyMetadata(true));

        public static readonly DependencyProperty IsEnableShadowProperty =
            DependencyProperty.Register(nameof(IsEnableShadow), typeof(bool), typeof(CardPanel), new PropertyMetadata(true, new PropertyChangedCallback(OnIsEnableShadowChanged)));

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

        /// <summary>
        /// 是否支持鼠标移入/移出动画.
        /// </summary>
        public bool IsEnableHoverAnimation
        {
            get { return (bool)this.GetValue(IsEnableHoverAnimationProperty); }
            set { this.SetValue(IsEnableHoverAnimationProperty, value); }
        }

        /// <summary>
        /// 是否显示阴影.
        /// </summary>
        public bool IsEnableShadow
        {
            get { return (bool)this.GetValue(IsEnableShadowProperty); }
            set { this.SetValue(IsEnableShadowProperty, value); }
        }

        /// <summary>
        /// 是否支持选中.
        /// </summary>
        public bool IsEnableCheck
        {
            get { return (bool)this.GetValue(IsEnableCheckProperty); }
            set { this.SetValue(IsEnableCheckProperty, value); }
        }

        public Brush PointerOverBorderBrush
        {
            get { return (Brush)this.GetValue(PointerOverBorderBrushProperty); }
            set { this.SetValue(PointerOverBorderBrushProperty, value); }
        }

        public Brush PointerOverBackground
        {
            get { return (Brush)this.GetValue(PointerOverBackgroundProperty); }
            set { this.SetValue(PointerOverBackgroundProperty, value); }
        }

        public Brush PressedBorderBrush
        {
            get { return (Brush)this.GetValue(PressedBorderBrushProperty); }
            set { this.SetValue(PressedBorderBrushProperty, value); }
        }

        public Brush PressedBackground
        {
            get { return (Brush)this.GetValue(PressedBackgroundProperty); }
            set { this.SetValue(PressedBackgroundProperty, value); }
        }

        public Brush DisabledBorderBrush
        {
            get { return (Brush)this.GetValue(DisabledBorderBrushProperty); }
            set { this.SetValue(DisabledBorderBrushProperty, value); }
        }

        public Brush DisabledBackground
        {
            get { return (Brush)this.GetValue(DisabledBackgroundProperty); }
            set { this.SetValue(DisabledBackgroundProperty, value); }
        }

        public Brush CheckedBorderBrush
        {
            get { return (Brush)this.GetValue(CheckedBorderBrushProperty); }
            set { this.SetValue(CheckedBorderBrushProperty, value); }
        }

        public Brush CheckedBackground
        {
            get { return (Brush)this.GetValue(CheckedBackgroundProperty); }
            set { this.SetValue(CheckedBackgroundProperty, value); }
        }

        public Brush CheckedPointerOverBorderBrush
        {
            get { return (Brush)this.GetValue(CheckedPointerOverBorderBrushProperty); }
            set { this.SetValue(CheckedPointerOverBorderBrushProperty, value); }
        }

        public Brush CheckedPointerOverBackground
        {
            get { return (Brush)this.GetValue(CheckedPointerOverBackgroundProperty); }
            set { this.SetValue(CheckedPointerOverBackgroundProperty, value); }
        }

        public Brush CheckedPressedBorderBrush
        {
            get { return (Brush)this.GetValue(CheckedPressedBorderBrushProperty); }
            set { this.SetValue(CheckedPressedBorderBrushProperty, value); }
        }

        public Brush CheckedPressedBackground
        {
            get { return (Brush)this.GetValue(CheckedPressedBackgroundProperty); }
            set { this.SetValue(CheckedPressedBackgroundProperty, value); }
        }

        public Brush CheckedDisabledBorderBrush
        {
            get { return (Brush)this.GetValue(CheckedDisabledBorderBrushProperty); }
            set { this.SetValue(CheckedDisabledBorderBrushProperty, value); }
        }

        public Brush CheckedDisabledBackground
        {
            get { return (Brush)this.GetValue(CheckedDisabledBackgroundProperty); }
            set { this.SetValue(CheckedDisabledBackgroundProperty, value); }
        }

#pragma warning restore SA1600 // Elements should be documented
    }
}

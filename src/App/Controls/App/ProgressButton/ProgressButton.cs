// Copyright (c) Richasy. All rights reserved.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 带进度环和完成动画的按钮.
    /// </summary>
    public sealed class ProgressButton : ToggleButton
    {
        /// <summary>
        /// Dependency property of <see cref="Diameter"/>.
        /// </summary>
        public static readonly DependencyProperty DiameterProperty =
            DependencyProperty.Register(
                nameof(Diameter),
                typeof(double),
                typeof(ProgressButton),
                new PropertyMetadata(default, new PropertyChangedCallback(OnDiameterChanged)));

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressButton"/> class.
        /// </summary>
        public ProgressButton()
        {
            this.DefaultStyleKey = typeof(ProgressButton);
        }

        /// <summary>
        /// Gets or sets button diameter.
        /// </summary>
        public double Diameter
        {
            get { return (double)this.GetValue(DiameterProperty); }
            set { this.SetValue(DiameterProperty, value); }
        }

        private static void OnDiameterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var instance = d as ProgressButton;
            if (e.NewValue is double v && v > 0)
            {
                instance.Width = instance.Height = v;
                instance.CornerRadius = new CornerRadius(Math.Ceiling(v / 2.0) + 1);
            }
        }
    }
}

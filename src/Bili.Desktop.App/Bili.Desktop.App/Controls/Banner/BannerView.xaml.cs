// Copyright (c) Richasy. All rights reserved.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace Bili.Desktop.App.Controls
{
    /// <summary>
    /// 横幅视图.
    /// </summary>
    public sealed partial class BannerView : UserControl
    {
        /// <summary>
        /// 数据源的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(BannerView), new PropertyMetadata(null));

        /// <summary>
        /// <see cref="ViewStyle"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewStyleProperty =
           DependencyProperty.Register(nameof(ViewStyle), typeof(Style), typeof(BannerView), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerView"/> class.
        /// </summary>
        public BannerView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 横幅数据源.
        /// </summary>
        public object ItemsSource
        {
            get { return (object)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// 视图样式，指<see cref="HorizontalRepeaterView"/>.
        /// </summary>
        public Style ViewStyle
        {
            get { return (Style)GetValue(ViewStyleProperty); }
            set { SetValue(ViewStyleProperty, value); }
        }
    }
}

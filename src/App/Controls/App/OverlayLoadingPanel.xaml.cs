// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 覆盖式加载面板.
    /// </summary>
    public sealed partial class OverlayLoadingPanel : UserControl
    {
        /// <summary>
        /// <see cref="IsRingActive"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsRingActiveProperty =
            DependencyProperty.Register(nameof(IsRingActive), typeof(bool), typeof(OverlayLoadingPanel), new PropertyMetadata(default));

        /// <summary>
        /// <see cref="IsBarActive"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty IsBarActiveProperty =
            DependencyProperty.Register(nameof(IsBarActive), typeof(bool), typeof(OverlayLoadingPanel), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="OverlayLoadingPanel"/> class.
        /// </summary>
        public OverlayLoadingPanel()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 是否激活进度环.
        /// </summary>
        /// <remarks>
        /// 进度环居中显示.
        /// </remarks>
        public bool IsRingActive
        {
            get { return (bool)GetValue(IsRingActiveProperty); }
            set { SetValue(IsRingActiveProperty, value); }
        }

        /// <summary>
        /// 是否激活进度条.
        /// </summary>
        /// <remarks>
        /// 进度条位于底部.
        /// </remarks>
        public bool IsBarActive
        {
            get { return (bool)GetValue(IsBarActiveProperty); }
            set { SetValue(IsBarActiveProperty, value); }
        }
    }
}

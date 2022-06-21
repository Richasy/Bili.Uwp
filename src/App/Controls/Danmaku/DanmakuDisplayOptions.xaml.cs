// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls
{
    /// <summary>
    /// 弹幕显示设置.
    /// </summary>
    public sealed partial class DanmakuDisplayOptions : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的视图模型.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(DanmakuModuleViewModel), typeof(DanmakuDisplayOptions), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="DanmakuDisplayOptions"/> class.
        /// </summary>
        public DanmakuDisplayOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public DanmakuModuleViewModel ViewModel
        {
            get { return (DanmakuModuleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
    }
}

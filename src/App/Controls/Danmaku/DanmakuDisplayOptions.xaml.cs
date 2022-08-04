// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Danmaku
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
            DependencyProperty.Register(nameof(ViewModel), typeof(IDanmakuModuleViewModel), typeof(DanmakuDisplayOptions), new PropertyMetadata(default));

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
        public IDanmakuModuleViewModel ViewModel
        {
            get { return (IDanmakuModuleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
    }
}

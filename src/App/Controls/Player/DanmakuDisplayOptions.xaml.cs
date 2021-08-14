// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
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
            DependencyProperty.Register(nameof(ViewModel), typeof(DanmakuViewModel), typeof(DanmakuDisplayOptions), new PropertyMetadata(DanmakuViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="DanmakuDisplayOptions"/> class.
        /// </summary>
        public DanmakuDisplayOptions()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public DanmakuViewModel ViewModel
        {
            get { return (DanmakuViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }
    }
}

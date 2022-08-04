// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Local;
using Bili.ViewModels.Interfaces.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Danmaku
{
    /// <summary>
    /// 弹幕发送设置.
    /// </summary>
    public sealed partial class DanmakuSendOptions : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的视图模型.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(IDanmakuModuleViewModel), typeof(DanmakuSendOptions), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="DanmakuSendOptions"/> class.
        /// </summary>
        public DanmakuSendOptions()
            => InitializeComponent();

        /// <summary>
        /// 视图模型.
        /// </summary>
        public IDanmakuModuleViewModel ViewModel
        {
            get { return (IDanmakuModuleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private void OnColorItemClick(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext as KeyValue<string>;
            ViewModel.Color = item.Value;
        }

        private void OnSizeItemClick(object sender, RoutedEventArgs e)
            => ViewModel.IsStandardSize = !ViewModel.IsStandardSize;
    }
}

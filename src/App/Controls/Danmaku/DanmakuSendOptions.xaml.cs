// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.Models.App.Other;
using Richasy.Bili.ViewModels.Uwp.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
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
            DependencyProperty.Register(nameof(ViewModel), typeof(DanmakuViewModel), typeof(DanmakuSendOptions), new PropertyMetadata(DanmakuViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="DanmakuSendOptions"/> class.
        /// </summary>
        public DanmakuSendOptions()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public DanmakuViewModel ViewModel
        {
            get { return (DanmakuViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <summary>
        /// 初始化.
        /// </summary>
        public void Initialize()
        {
            if (ViewModel.IsStandardSize)
            {
                StandardItem.IsChecked = true;
                SmallItem.IsChecked = false;
            }
            else
            {
                StandardItem.IsChecked = false;
                SmallItem.IsChecked = true;
            }
        }

        private void OnColorItemClick(object sender, RoutedEventArgs e)
        {
            var item = (sender as FrameworkElement).DataContext as KeyValue<string>;
            ViewModel.Color = item.Value;
        }
    }
}

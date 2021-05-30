// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页.
    /// </summary>
    public sealed partial class PartitionPage : Page
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register("ViewModel", typeof(PartitionViewModel), typeof(PartitionPage), new PropertyMetadata(PartitionViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionPage"/> class.
        /// </summary>
        public PartitionPage()
        {
            this.InitializeComponent();
            this.FindName("PartitionView");
        }

        /// <summary>
        /// 分区视图模型.
        /// </summary>
        public PartitionViewModel ViewModel
        {
            get { return (PartitionViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private async void OnItemsRepeaterLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (ViewModel.PartitionCollection.Count == 0)
            {
                await ViewModel.InitializePartitionAsync();
            }
        }
    }
}

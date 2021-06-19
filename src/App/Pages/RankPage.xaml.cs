// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页.
    /// </summary>
    public sealed partial class RankPage : Page
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(RankViewModel), typeof(RankPage), new PropertyMetadata(RankViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="RankPage"/> class.
        /// </summary>
        public RankPage()
        {
            this.InitializeComponent();
            this.Loaded += OnLoadedAsync;
            this.Unloaded += OnUnloaded;
        }

        /// <summary>
        /// 排行榜视图模型.
        /// </summary>
        public RankViewModel ViewModel
        {
            get { return (RankViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            this.ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeAsync();
            this.ViewModel.PropertyChanged += OnViewModelPropertyChanged;
            CheckSelectedItem();
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.CurrentPartition))
            {
                CheckSelectedItem();
            }
        }

        private void CheckSelectedItem()
        {
            if (RankNavigationView.SelectedItem != ViewModel.CurrentPartition)
            {
                RankNavigationView.SelectedItem = ViewModel.CurrentPartition;
            }
        }

        private async void OnDetailNavigationViewItemInvokedAsync(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            var item = args.InvokedItem as RankPartition;
            ContentScrollViewer.ChangeView(0, 0, 1);
            await ViewModel.SetSelectedRankPartitionAsync(item);
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.Models.Enums;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页.
    /// </summary>
    public sealed partial class SpecialColumnPage : Page
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(SpecialColumnModuleViewModel), typeof(SpecialColumnPage), new PropertyMetadata(SpecialColumnModuleViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialColumnPage"/> class.
        /// </summary>
        public SpecialColumnPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            this.Loaded += OnLoadedAsync;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public SpecialColumnModuleViewModel ViewModel
        {
            get { return (SpecialColumnModuleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (this.ViewModel.CategoryCollection.Count == 0)
            {
                await this.ViewModel.RequestCategoriesAsync();
            }

            this.FindName(nameof(SpecialColumnNavigationView));
            this.FindName(nameof(ContentGrid));
        }

        private async void OnArticleViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            if (!this.ViewModel.CurrentCategory.IsInitializeLoading && !this.ViewModel.CurrentCategory.IsDeltaLoading)
            {
                await this.ViewModel.CurrentCategory.RequestDataAsync();
            }
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            if (!ViewModel.CurrentCategory.IsInitializeLoading && !ViewModel.CurrentCategory.IsDeltaLoading)
            {
                await ViewModel.CurrentCategory.InitializeRequestAsync();
            }
        }

        private void OnArticleSortComboBoxSlectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ArticleSortComboBox.SelectedItem != null)
            {
                var item = (ArticleSortType)ArticleSortComboBox.SelectedItem;
                ViewModel.CurrentCategory.CurrentSortType = item;
            }
        }
    }
}

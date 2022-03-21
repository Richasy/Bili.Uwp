// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.App.Controls;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页.
    /// </summary>
    public sealed partial class SpecialColumnPage : AppPage, IRefreshPage
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
            InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            Loaded += OnLoadedAsync;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public SpecialColumnModuleViewModel ViewModel
        {
            get { return (SpecialColumnModuleViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        /// <inheritdoc/>
        public Task RefreshAsync()
            => ViewModel.CurrentCategory.InitializeRequestAsync();

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            if (ViewModel.CategoryCollection.Count == 0)
            {
                await ViewModel.RequestCategoriesAsync();
            }

            CheckCurrentTabAsync(true);
        }

        private async void OnArticleViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            if (!ViewModel.CurrentCategory.IsInitializeLoading && !ViewModel.CurrentCategory.IsDeltaLoading)
            {
                await ViewModel.CurrentCategory.RequestDataAsync();
            }
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await RefreshAsync();
        }

        private void OnArticleSortComboBoxSlectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ArticleSortComboBox.SelectedItem != null)
            {
                var item = (ArticleSortType)ArticleSortComboBox.SelectedItem;
                ViewModel.CurrentCategory.CurrentSortType = item;
            }
        }

        private void OnSpecialColumnNavigationViewItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            if (args.InvokedItem is SpecialColumnCategoryViewModel vm)
            {
                ViewModel.CurrentCategory = vm;
            }
            else if (args.InvokedItemContainer is Microsoft.UI.Xaml.Controls.NavigationViewItem item)
            {
                ViewModel.CurrentCategory = item.Tag as SpecialColumnCategoryViewModel;
            }

            CheckCurrentTabAsync();
        }

        private async void CheckCurrentTabAsync(bool needDelay = false)
        {
            if (!(SpecialColumnNavigationView.SelectedItem is SpecialColumnCategoryViewModel selectedItem) || selectedItem != ViewModel.CurrentCategory)
            {
                if (needDelay)
                {
                    await Task.Delay(100);
                }

                SpecialColumnNavigationView.SelectedItem = ViewModel.CurrentCategory;
            }
        }
    }
}

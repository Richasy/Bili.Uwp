﻿// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.App.Pages.Base;
using Bili.Models.Data.Appearance;
using Bili.ViewModels.Uwp.Search;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Bili.App.Pages.Desktop.Overlay
{
    /// <summary>
    /// 搜索页面.
    /// </summary>
    public sealed partial class SearchPage : SearchPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchPage"/> class.
        /// </summary>
        public SearchPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.NavigationMode != NavigationMode.Back && e.Parameter is string keyword)
            {
                ViewModel.SetKeyword(keyword);
            }
        }

        private void OnNavItemInvokedAsync(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            var item = args.InvokedItem as SearchModuleItemViewModel;
            if (item != ViewModel.CurrentModule)
            {
                ViewModel.SelectModuleCommand.Execute(item).Subscribe();
            }
        }

        private void OnFilterItemSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var comboBox = sender as ComboBox;
            if (comboBox.DataContext is not SearchFilterViewModel context)
            {
                return;
            }

            var selectItem = comboBox.SelectedItem as Condition;
            if (selectItem != context.CurrentCondition && selectItem != null)
            {
                // 条件变更，重载模块.
                context.CurrentCondition = selectItem;
                ViewModel.ReloadModuleCommand.Execute().Subscribe();
            }
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Data.Community;
using Bili.Models.Enums;
using Bili.ViewModels.Uwp.Article;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Pages.Desktop
{
    /// <summary>
    /// 文章分区页面.
    /// </summary>
    public sealed partial class ArticlePartitionPage : ArticlePartitionPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticlePartitionPage"/> class.
        /// </summary>
        public ArticlePartitionPage() => InitializeComponent();

        private void OnArticleSortComboBoxSlectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ArticleSortComboBox.SelectedItem is ArticleSortType type
                 && ViewModel.SortType != type)
            {
                ViewModel.SortType = type;
                ViewModel.ReloadCommand.Execute().Subscribe();
            }
        }

        private void OnSpecialColumnNavigationViewItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            var data = args.InvokedItem as Partition;
            ContentScrollViewer.ChangeView(0, 0, 1);
            ViewModel.SelectPartitionCommand.Execute(data).Subscribe();
        }
    }

    /// <summary>
    /// <see cref="ArticlePartitionPage"/> 的基类.
    /// </summary>
    public class ArticlePartitionPageBase : AppPage<ArticlePartitionPageViewModel>
    {
    }
}

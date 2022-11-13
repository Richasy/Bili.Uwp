// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Data.Community;
using Bili.Models.Enums;
using Bili.ViewModels.Interfaces.Article;
using Windows.UI.Xaml.Controls;

namespace Bili.Uwp.App.Pages.Desktop
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

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();

        private void OnArticleSortComboBoxSlectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ArticleSortComboBox.SelectedItem is ArticleSortType type
                 && ViewModel.SortType != type)
            {
                ViewModel.SortType = type;
                ViewModel.ReloadCommand.ExecuteAsync(null);
            }
        }

        private void OnSpecialColumnNavigationViewItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            var data = args.InvokedItem as Partition;
            ContentScrollViewer.ChangeView(0, 0, 1);
            ViewModel.SelectPartitionCommand.Execute(data);
        }
    }

    /// <summary>
    /// <see cref="ArticlePartitionPage"/> 的基类.
    /// </summary>
    public class ArticlePartitionPageBase : AppPage<IArticlePartitionPageViewModel>
    {
    }
}

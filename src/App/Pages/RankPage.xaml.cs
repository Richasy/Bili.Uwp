// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Data.Community;
using Bili.ViewModels.Uwp.Home;

namespace Bili.App.Pages
{
    /// <summary>
    /// 排行榜页面.
    /// </summary>
    public sealed partial class RankPage : RankPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RankPage"/> class.
        /// </summary>
        public RankPage() => InitializeComponent();

        private void OnDetailNavigationViewItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            ContentScrollViewer.ChangeView(0, 0, 1, true);
            var data = args.InvokedItem as Partition;

            if (data != ViewModel.CurrentPartition)
            {
                ViewModel.SelectPartitionCommand.Execute(data).Subscribe();
            }
        }
    }

    /// <summary>
    /// <see cref="RankPage"/> 的基类.
    /// </summary>
    public class RankPageBase : AppPage<RankPageViewModel>
    {
    }
}

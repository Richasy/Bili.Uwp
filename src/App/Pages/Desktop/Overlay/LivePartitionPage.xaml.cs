// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Data.Community;
using Bili.ViewModels.Uwp.Live;

namespace Bili.App.Pages.Desktop.Overlay
{
    /// <summary>
    /// 直播分区页.
    /// </summary>
    public sealed partial class LivePartitionPage : LivePartitionPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LivePartitionPage"/> class.
        /// </summary>
        public LivePartitionPage()
            => InitializeComponent();

        private void OnParentItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            ContentScrollViewer.ChangeView(0, 0, 1, true);
            var data = args.InvokedItem as Partition;

            if (data != ViewModel.CurrentParentPartition)
            {
                ViewModel.SelectPartitionCommand.Execute(data).Subscribe();
            }
        }
    }

    /// <summary>
    /// <see cref="LivePartitionPage"/> 的基类.
    /// </summary>
    public class LivePartitionPageBase: AppPage<LivePartitionPageViewModel>
    {
    }
}

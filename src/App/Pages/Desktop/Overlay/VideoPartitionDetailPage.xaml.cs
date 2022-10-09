// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.App.Pages.Base;
using Bili.Models.Data.Community;
using Bili.Models.Enums;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Bili.App.Pages.Desktop.Overlay
{
    /// <summary>
    /// 分区详情页面.
    /// </summary>
    public sealed partial class VideoPartitionDetailPage : VideoPartitionDetailPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPartitionDetailPage"/> class.
        /// </summary>
        public VideoPartitionDetailPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter != null && e.Parameter is Partition partition)
            {
                ViewModel.SetPartition(partition);
            }
        }

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();

        private void OnDetailNavigationViewItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            var data = args.InvokedItem as Partition;
            ContentScrollViewer.ChangeView(0, 0, 1);
            ViewModel.SelectPartitionCommand.Execute(data);
        }

        private void OnVideoSortComboBoxSlectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VideoSortComboBox.SelectedItem is VideoSortType type
                && ViewModel.SortType != type)
            {
                ViewModel.SortType = type;
                ViewModel.ReloadCommand.ExecuteAsync(null);
            }
        }
    }
}

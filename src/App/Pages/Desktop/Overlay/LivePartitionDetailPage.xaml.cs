// Copyright (c) Richasy. All rights reserved.

using Bili.App.Pages.Base;
using Bili.Models.Data.Community;
using Bili.Models.Data.Live;
using Windows.UI.Xaml.Navigation;

namespace Bili.App.Pages.Desktop.Overlay
{
    /// <summary>
    /// 直播分区详情页.
    /// </summary>
    public sealed partial class LivePartitionDetailPage : LivePartitionDetailPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LivePartitionDetailPage"/> class.
        /// </summary>
        public LivePartitionDetailPage() => InitializeComponent();

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Partition partition)
            {
                ViewModel.SetPartition(partition);
            }
        }

        private void OnTagItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            var data = args.InvokedItem as LiveTag;
            ContentScrollViewer.ChangeView(0, 0, 1);
            ViewModel.SelectTagCommand.Execute(data);
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Uwp.App.Pages.Base;
using Bili.Models.Data.Community;

namespace Bili.Uwp.App.Pages.Desktop.Overlay
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

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();

        private void OnParentItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            ContentScrollViewer.ChangeView(0, 0, 1, true);
            var data = args.InvokedItem as Partition;

            if (data != ViewModel.CurrentParentPartition)
            {
                ViewModel.SelectPartitionCommand.Execute(data);
            }
        }
    }
}

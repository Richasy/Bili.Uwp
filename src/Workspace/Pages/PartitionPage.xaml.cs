// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Bili.Models.Data.Community;
using Bili.ViewModels.Interfaces.Video;
using Microsoft.UI.Xaml.Navigation;

namespace Bili.Workspace.Pages
{
    /// <summary>
    /// 分区页面.
    /// </summary>
    public sealed partial class PartitionPage : PartitionPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionPage"/> class.
        /// </summary>
        public PartitionPage()
        {
            InitializeComponent();
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Partition partition)
            {
                Header.Title = partition.Name;
                ViewModel.SetPartition(partition);
            }
        }

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => ViewModel.InitializeCommand.Execute(default);

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.IsReloading))
            {
                ContentScrollViewer.ChangeView(0, 0, 1);
            }
        }

        private void OnErrorPanelButtonClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
            => ViewModel.ReloadCommand.Execute(default);

        private void OnVideoViewRequestLoadMore(object sender, System.EventArgs e)
            => ViewModel.IncrementalCommand.Execute(default);
    }

    /// <summary>
    /// <see cref="PartitionPage"/>的基类.
    /// </summary>
    public class PartitionPageBase : PageBase<IVideoPartitionDetailPageViewModel>
    {
    }
}

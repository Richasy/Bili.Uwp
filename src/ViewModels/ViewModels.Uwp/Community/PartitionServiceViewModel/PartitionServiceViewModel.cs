// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.App.Args;
using Bili.Models.Data.Community;
using Bili.ViewModels.Interfaces;
using ReactiveUI;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 分区内容连接处理视图模型.
    /// </summary>
    public sealed partial class PartitionServiceViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionServiceViewModel"/> class.
        /// </summary>
        /// <param name="navigation">导航服务.</param>
        public PartitionServiceViewModel(INavigationViewModel navigation)
        {
            _navigation = navigation;

            _navigation.Backing += OnNavigationBacking;
            GotoDetailViewCommand = ReactiveCommand.Create<Partition>(GotoDetailView, outputScheduler: RxApp.MainThreadScheduler);
        }

        private void GotoDetailView(Partition partition)
            => _navigation.NavigateToSecondaryView(Models.Enums.PageIds.PartitionDetail, partition);

        private void OnNavigationBacking(object sender, AppBackEventArgs e)
        {
            if (e.FromPageId == Models.Enums.PageIds.PartitionDetail
                && e.BackParameter is Partition partition)
            {
                BackRequested?.Invoke(this, partition);
            }
        }
    }
}

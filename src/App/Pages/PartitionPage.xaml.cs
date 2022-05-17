// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.App.Pages.Overlay;
using Bili.App.Resources.Extension;
using Bili.Models.Data.Community;
using Bili.ViewModels.Uwp.Community;
using Splat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Bili.App.Pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页.
    /// </summary>
    public sealed partial class PartitionPage : PartitionPageBase
    {
        /// <summary>
        /// 服务视图模型.
        /// </summary>
        private readonly PartitionServiceViewModel _serviceViewModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionPage"/> class.
        /// </summary>
        public PartitionPage()
            : base()
        {
            _serviceViewModel = Splat.Locator.Current.GetService<PartitionServiceViewModel>();
            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
            => _serviceViewModel.BackRequested -= OnBackRequested;

        private void OnLoaded(object sender, RoutedEventArgs e)
            => _serviceViewModel.BackRequested += OnBackRequested;

        private void OnBackRequested(object sender, Partition e)
        {
            if (PartitionView != null && e != null)
            {
                var element = PartitionView.GetOrCreateElement(ViewModel.Partitions.IndexOf(e));
                if (element != null)
                {
                    var animateService = ConnectedAnimationService.GetForCurrentView();
                    animateService.TryStartAnimation(nameof(PartitionDetailPage), element);
                }
            }
        }
    }

    /// <summary>
    /// <see cref="PartitionPage"/> 的基类.
    /// </summary>
    public class PartitionPageBase : AppPage<PartitionPageViewModel>
    {
    }
}

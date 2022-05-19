// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Community;
using Bili.ViewModels.Uwp.Core;
using Splat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Bili.App.Controls.Community
{
    /// <summary>
    /// 分区条目视图.
    /// </summary>
    public sealed class PartitionItem : ReactiveControl<Partition>
    {
        private const string RootCardName = "RootCard";

        private ButtonBase _rootCard;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionItem"/> class.
        /// </summary>
        public PartitionItem() => DefaultStyleKey = typeof(PartitionItem);

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _rootCard = GetTemplateChild(RootCardName) as CardPanel;
            _rootCard.Click += OnRootCardClick;
        }

        private void OnRootCardClick(object sender, RoutedEventArgs e)
        {
            var vm = Splat.Locator.Current.GetService<NavigationViewModel>();
            vm.NavigateToSecondaryView(Models.Enums.PageIds.PartitionDetail, ViewModel);
        }
    }
}

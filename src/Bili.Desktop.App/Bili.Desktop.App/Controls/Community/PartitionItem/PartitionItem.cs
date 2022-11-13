// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.Models.Data.Community;
using Bili.Models.Enums.App;
using Bili.ViewModels.Interfaces.Core;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls.Primitives;

namespace Bili.Desktop.App.Controls.Community
{
    /// <summary>
    /// 分区条目视图.
    /// </summary>
    public sealed class PartitionItem : ReactiveControl<Partition>
    {
        /// <summary>
        /// <see cref="Type"/> 的依赖属性.
        /// </summary>
        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register(nameof(Type), typeof(PartitionType), typeof(PartitionItem), new PropertyMetadata(PartitionType.Video));

        private const string RootCardName = "RootCard";

        private ButtonBase _rootCard;

        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionItem"/> class.
        /// </summary>
        public PartitionItem() => DefaultStyleKey = typeof(PartitionItem);

        /// <summary>
        /// 类型.
        /// </summary>
        public PartitionType Type
        {
            get { return (PartitionType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            _rootCard = GetTemplateChild(RootCardName) as CardPanel;
            _rootCard.Click += OnRootCardClick;
        }

        private void OnRootCardClick(object sender, RoutedEventArgs e)
        {
            var vm = Locator.Instance.GetService<INavigationViewModel>();
            var pageId = Type == PartitionType.Video
                ? Models.Enums.PageIds.VideoPartitionDetail
                : Models.Enums.PageIds.LivePartitionDetail;
            vm.NavigateToSecondaryView(pageId, ViewModel);
        }
    }
}

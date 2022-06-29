// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using Bili.ViewModels.Uwp.Base;
using Bili.ViewModels.Uwp.Core;
using Bili.ViewModels.Uwp.Pgc;
using Splat;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bili.App.Controls.Favorite
{
    /// <summary>
    /// PGC收藏夹视图.
    /// </summary>
    public sealed partial class PgcFavoritePanel : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(PgcFavoriteModuleViewModelBase), typeof(PgcFavoritePanel), new PropertyMetadata(default));

        /// <summary>
        /// Initializes a new instance of the <see cref="PgcFavoritePanel"/> class.
        /// </summary>
        public PgcFavoritePanel() => InitializeComponent();

        /// <summary>
        /// 核心数据模型.
        /// </summary>
        public AppViewModel CoreViewModel { get; } = Splat.Locator.Current.GetService<AppViewModel>();

        /// <summary>
        /// 视图模型.
        /// </summary>
        public PgcFavoriteModuleViewModelBase ViewModel
        {
            get { return (PgcFavoriteModuleViewModelBase)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private void OnItemFlyoutOpened(object sender, object e)
        {
            var items = (sender as MenuFlyout).Items.OfType<MenuFlyoutItem>().Take(3);
            foreach (var item in items)
            {
                if (item is MenuFlyoutItem btn)
                {
                    var status = int.Parse(btn.Tag.ToString());
                    item.IsEnabled = status != ViewModel.Status;
                }
            }
        }

        private void OnMarkStatusButtonClick(object sender, RoutedEventArgs e)
        {
            var item = sender as MenuFlyoutItem;
            var context = item.DataContext as SeasonItemViewModel;
            var status = int.Parse(item.Tag.ToString());
            context.ChangeFavoriteStatusCommand.Execute(status).Subscribe();
        }

        private void OnStatusSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StatusComboBox.SelectedItem is int status && status != ViewModel.Status)
            {
                ViewModel.SetStatusCommand.Execute(status).Subscribe();
            }
        }
    }
}

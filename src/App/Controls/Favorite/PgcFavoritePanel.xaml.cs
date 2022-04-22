// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
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
            DependencyProperty.Register(nameof(ViewModel), typeof(PgcFavoriteViewModelBase), typeof(PgcFavoritePanel), new PropertyMetadata(null));

        /// <summary>
        /// Initializes a new instance of the <see cref="PgcFavoritePanel"/> class.
        /// </summary>
        public PgcFavoritePanel()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public PgcFavoriteViewModelBase ViewModel
        {
            get { return (PgcFavoriteViewModelBase)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private async void OnViewRequestLoadMoreAsync(object sender, System.EventArgs e)
        {
            await ViewModel.RequestDataAsync();
        }

        private async void OnPgcRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeRequestAsync();
        }

        private async void OnUnFavoritePgcButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var vm = (sender as FrameworkElement).DataContext as SeasonViewModel;
            await ViewModel.RemoveFavoritePgcAsync(vm);
        }

        private void OnItemFlyoutOpened(object sender, object e)
        {
            var items = (sender as Microsoft.UI.Xaml.Controls.CommandBarFlyout).SecondaryCommands.OfType<AppBarButton>().Take(3);
            foreach (var item in items)
            {
                if (item is AppBarButton btn)
                {
                    var status = int.Parse(btn.Tag.ToString());
                    item.IsEnabled = status != ViewModel.Status;
                }
            }
        }

        private async void OnMarkStatusButtonClickAsync(object sender, RoutedEventArgs e)
        {
            var btn = sender as AppBarButton;
            var context = btn.DataContext as SeasonViewModel;
            var status = int.Parse(btn.Tag.ToString());
            await ViewModel.UpdateItemStatusAsync(context, status);
        }
    }
}

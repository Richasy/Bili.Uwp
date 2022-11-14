// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using Bili.App.Pages.Base;
using Bili.Models.App.Other;
using Bili.Models.Enums.App;
using Windows.UI.Xaml.Navigation;

namespace Bili.App.Pages.Xbox.Overlay
{
    /// <summary>
    /// 收藏页面.
    /// </summary>
    public sealed partial class FavoritePage : FavoritePageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FavoritePage"/> class.
        /// </summary>
        public FavoritePage()
        {
            InitializeComponent();
            AnimePanel.ViewModel = AnimeFavoriteModule;
            AnimePanel.DataContext = AnimeFavoriteModule;
            CinemaPanel.ViewModel = CinemaFavoriteModule;
            CinemaPanel.DataContext = CinemaFavoriteModule;
        }

        /// <inheritdoc/>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is FavoriteType type)
            {
                var header = ViewModel.TypeCollection.First(p => p.Type == type);
                ViewModel.SelectTypeCommand.Execute(header);
            }
        }

        /// <inheritdoc/>
        protected override void OnPageLoaded()
            => Bindings.Update();

        /// <inheritdoc/>
        protected override void OnPageUnloaded()
            => Bindings.StopTracking();

        private void OnRefreshButtonClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            switch (ViewModel.CurrentType.Type)
            {
                case FavoriteType.Video:
                    VideoPanel.ViewModel.ReloadCommand.ExecuteAsync(null);
                    break;
                case FavoriteType.Anime:
                    AnimePanel.ViewModel.ReloadCommand.ExecuteAsync(null);
                    break;
                case FavoriteType.Cinema:
                    CinemaPanel.ViewModel.ReloadCommand.ExecuteAsync(null);
                    break;
                default:
                    break;
            }
        }

        private void OnNavItemInvoked(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs args)
        {
            var type = args.InvokedItem as FavoriteHeader;
            if (type != ViewModel.CurrentType)
            {
                ViewModel.SelectTypeCommand.Execute(type);
            }
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.App.Other;
using Bili.Models.Enums.App;
using Bili.ViewModels.Uwp;
using Bili.ViewModels.Uwp.Account;
using Splat;

namespace Bili.App.Pages.Desktop.Overlay
{
    /// <summary>
    /// 收藏夹页面.
    /// </summary>
    public sealed partial class FavoritePage : FavoritePageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FavoritePage"/> class.
        /// </summary>
        public FavoritePage()
        {
            InitializeComponent();
            AnimePanel.ViewModel = Splat.Locator.Current.GetService<AnimeFavoriteModuleViewModel>();
            AnimePanel.DataContext = AnimePanel.ViewModel;
            CinemaPanel.ViewModel = Splat.Locator.Current.GetService<CinemaFavoriteModuleViewModel>();
            CinemaPanel.DataContext = CinemaPanel.ViewModel;
        }

        private void OnRefreshButtonClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            switch (ViewModel.CurrentType.Type)
            {
                case FavoriteType.Video:
                    VideoPanel.ViewModel.ReloadCommand.Execute().Subscribe();
                    break;
                case FavoriteType.Anime:
                    AnimePanel.ViewModel.ReloadCommand.Execute().Subscribe();
                    break;
                case FavoriteType.Cinema:
                    CinemaPanel.ViewModel.ReloadCommand.Execute().Subscribe();
                    break;
                case FavoriteType.Article:
                    ArticlePanel.ViewModel.ReloadCommand.Execute().Subscribe();
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
                ViewModel.SelectTypeCommand.Execute(type).Subscribe();
            }
        }
    }

    /// <summary>
    /// <see cref="FavoritePage"/> 的基类.
    /// </summary>
    public class FavoritePageBase : AppPage<FavoritePageViewModel>
    {
    }
}

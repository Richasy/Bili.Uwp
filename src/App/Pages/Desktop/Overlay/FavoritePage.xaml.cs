﻿// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using System.Threading.Tasks;
using Bili.Models.Enums.App;
using Bili.ViewModels.Uwp;
using Windows.UI.Xaml;

namespace Bili.App.Pages.Desktop.Overlay
{
    /// <summary>
    /// 收藏夹页面.
    /// </summary>
    public sealed partial class FavoritePage : AppPage
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(FavoriteViewModel), typeof(FavoritePage), new PropertyMetadata(FavoriteViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="FavoritePage"/> class.
        /// </summary>
        public FavoritePage()
        {
            InitializeComponent();
            Loaded += OnLoadedAsync;
            Unloaded += OnUnloaded;
            AnimePanel.ViewModel = FavoriteAnimeViewModel.Instance;
            CinemaPanel.ViewModel = FavoriteCinemaViewModel.Instance;
        }

        /// <summary>
        /// 视图模型.
        /// </summary>
        public FavoriteViewModel ViewModel
        {
            get { return (FavoriteViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
            => ViewModel.PropertyChanged -= OnViewModelPropertyChangedAsync;

        private async void OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged -= OnViewModelPropertyChangedAsync;
            ViewModel.PropertyChanged += OnViewModelPropertyChangedAsync;

            await CheckFavoriteTypeAsync();
        }

        private async void OnViewModelPropertyChangedAsync(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.CurrentType))
            {
                await CheckFavoriteTypeAsync();
            }
        }

        private async Task CheckFavoriteTypeAsync()
        {
            VideoPanel.Visibility = ViewModel.CurrentType == FavoriteType.Video ? Visibility.Visible : Visibility.Collapsed;
            AnimePanel.Visibility = ViewModel.CurrentType == FavoriteType.Anime ? Visibility.Visible : Visibility.Collapsed;
            CinemaPanel.Visibility = ViewModel.CurrentType == FavoriteType.Cinema ? Visibility.Visible : Visibility.Collapsed;
            ArticlePanel.Visibility = ViewModel.CurrentType == FavoriteType.Article ? Visibility.Visible : Visibility.Collapsed;
            switch (ViewModel.CurrentType)
            {
                case FavoriteType.Video:
                    VideoCard.IsSelected = true;
                    if (!ViewModel.IsVideoRequested)
                    {
                        await ViewModel.InitializeRequestAsync(FavoriteType.Video);
                    }

                    break;
                case FavoriteType.Anime:
                    AnimeItem.IsSelected = true;
                    if (!FavoriteAnimeViewModel.Instance.IsRequested)
                    {
                        await FavoriteAnimeViewModel.Instance.InitializeRequestAsync();
                    }

                    break;
                case FavoriteType.Cinema:
                    CinemaItem.IsSelected = true;
                    if (!FavoriteCinemaViewModel.Instance.IsRequested)
                    {
                        await FavoriteCinemaViewModel.Instance.InitializeRequestAsync();
                    }

                    break;
                case FavoriteType.Article:
                    ArticleItem.IsSelected = true;
                    if (!FavoriteArticleViewModel.Instance.IsRequested)
                    {
                        await FavoriteArticleViewModel.Instance.InitializeRequestAsync();
                    }

                    break;
                default:
                    break;
            }
        }

        private void OnNavSelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is Microsoft.UI.Xaml.Controls.NavigationViewItem item)
            {
                switch (item.Name)
                {
                    case nameof(VideoCard):
                        ViewModel.CurrentType = FavoriteType.Video;
                        break;
                    case nameof(AnimeItem):
                        ViewModel.CurrentType = FavoriteType.Anime;
                        break;
                    case nameof(CinemaItem):
                        ViewModel.CurrentType = FavoriteType.Cinema;
                        break;
                    case nameof(ArticleItem):
                        ViewModel.CurrentType = FavoriteType.Article;
                        break;
                    default:
                        break;
                }
            }
        }

        private async void OnRefreshButtonClickAsync(object sender, RoutedEventArgs e)
        {
            RefreshButton.IsEnabled = false;
            switch (ViewModel.CurrentType)
            {
                case FavoriteType.Video:
                    await ViewModel.InitializeRequestAsync(FavoriteType.Video);
                    break;
                case FavoriteType.Anime:
                    await FavoriteAnimeViewModel.Instance.InitializeRequestAsync();
                    break;
                case FavoriteType.Cinema:
                    await FavoriteCinemaViewModel.Instance.InitializeRequestAsync();
                    break;
                case FavoriteType.Article:
                    await FavoriteArticleViewModel.Instance.InitializeRequestAsync();
                    break;
                default:
                    break;
            }

            RefreshButton.IsEnabled = true;
        }
    }
}
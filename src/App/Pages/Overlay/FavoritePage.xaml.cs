// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using System.Threading.Tasks;
using Richasy.Bili.Models.Enums.App;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Pages.Overlay
{
    /// <summary>
    /// 收藏夹页面.
    /// </summary>
    public sealed partial class FavoritePage : Page
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
            this.InitializeComponent();
            this.Loaded += OnLoadedAsync;
            this.Unloaded += OnUnloaded;
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
        {
            ViewModel.PropertyChanged -= OnViewModelPropertyChangedAsync;
        }

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
            var canInit = false;
            VideoPanel.Visibility = ViewModel.CurrentType == FavoriteType.Video ? Visibility.Visible : Visibility.Collapsed;
            AnimePanel.Visibility = ViewModel.CurrentType == FavoriteType.Anime ? Visibility.Visible : Visibility.Collapsed;
            CinemaPanel.Visibility = ViewModel.CurrentType == FavoriteType.Cinema ? Visibility.Visible : Visibility.Collapsed;
            switch (ViewModel.CurrentType)
            {
                case FavoriteType.Video:
                    VideoItem.IsSelected = true;
                    canInit = !ViewModel.IsVideoRequested;
                    if (canInit)
                    {
                        await ViewModel.InitializeRequestAsync(FavoriteType.Video);
                    }

                    break;
                case FavoriteType.Anime:
                    AnimeItem.IsSelected = true;
                    canInit = !FavoriteAnimeViewModel.Instance.IsRequested;
                    if (canInit)
                    {
                        await FavoriteAnimeViewModel.Instance.InitializeRequestAsync();
                    }

                    break;
                case FavoriteType.Cinema:
                    CinemaItem.IsSelected = true;
                    canInit = !FavoriteCinemaViewModel.Instance.IsRequested;
                    if (canInit)
                    {
                        await FavoriteCinemaViewModel.Instance.InitializeRequestAsync();
                    }

                    break;
                case FavoriteType.Article:
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
                    case nameof(VideoItem):
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
    }
}

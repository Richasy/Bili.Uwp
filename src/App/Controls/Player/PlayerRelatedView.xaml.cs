// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 播放器的关联视图.
    /// </summary>
    public sealed partial class PlayerRelatedView : PlayerComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerRelatedView"/> class.
        /// </summary>
        public PlayerRelatedView()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.IsDetailLoading))
            {
                if (ViewModel.IsShowParts)
                {
                    Nav.SelectedItem = PartsItem;
                }
                else if (ViewModel.IsShowEpisode)
                {
                    Nav.SelectedItem = EpisodeItem;
                }
                else if (ViewModel.IsShowSeason)
                {
                    Nav.SelectedItem = SeasonItem;
                }
                else if (ViewModel.IsShowRelatedVideos)
                {
                    Nav.SelectedItem = RelatedViedeosItem;
                }
            }
        }

        private void OnNavSelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            RelatedVideoView.Visibility = Nav.SelectedItem == RelatedViedeosItem ?
                Visibility.Visible : Visibility.Collapsed;
            PartView.Visibility = Nav.SelectedItem == PartsItem ?
                Visibility.Visible : Visibility.Collapsed;
            EpisodeView.Visibility = Nav.SelectedItem == EpisodeItem ?
                Visibility.Visible : Visibility.Collapsed;
        }

        private async void OnPartItemClickAsync(object sender, RoutedEventArgs e)
        {
            var card = sender as CardPanel;
            var data = card.DataContext as VideoPartViewModel;
            if (!data.Data.Equals(ViewModel.CurrentVideoPart))
            {
                await ViewModel.ChangeVideoPartAsync(data.Data.Page.Cid);
            }
            else
            {
                data.IsSelected = true;
            }
        }

        private async void OnEpisodeItemClickAsync(object sender, RoutedEventArgs e)
        {
            var card = sender as CardPanel;
            var data = card.DataContext as PgcEpisodeViewModel;
            if (!data.Data.Equals(ViewModel.CurrentPgcEpisode))
            {
                await ViewModel.ChangePgcEpisodeAsync(data.Data.Id);
            }
            else
            {
                data.IsSelected = true;
            }
        }
    }
}

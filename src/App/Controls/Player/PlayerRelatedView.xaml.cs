// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
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
            InitializeLayout();
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
            ViewModel.RequestRelatedViewScrollToBottom += OnRequestScrollToBottom;
        }

        private void OnRequestScrollToBottom(object sender, EventArgs e)
        {
            ContentScrollViewer.ChangeView(0, ContentScrollViewer.ExtentHeight + ContentScrollViewer.ScrollableHeight + ContentScrollViewer.VerticalOffset, 1);
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.IsDetailLoading))
            {
                if (ViewModel.IsDetailLoading)
                {
                    return;
                }

                if (ViewModel.IsPgc && ViewModel.IsCurrentEpisodeInPgcSection)
                {
                    Nav.SelectedItem = SectionItem;
                }
                else if (ViewModel.IsShowParts)
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
                else if (ViewModel.IsShowSection)
                {
                    Nav.SelectedItem = SectionItem;
                }
                else if (ViewModel.IsShowChat)
                {
                    Nav.SelectedItem = ChatItem;
                }
                else if (ViewModel.IsShowReply)
                {
                    Nav.SelectedItem = ReplyItem;
                }
            }
            else if (e.PropertyName == nameof(ViewModel.IsCurrentEpisodeInPgcSection))
            {
                if (ViewModel.IsCurrentEpisodeInPgcSection)
                {
                    Nav.SelectedItem = SectionItem;
                }
            }
        }

        private void OnNavSelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            if (ViewModel.IsShowRelatedVideos)
            {
                RelatedVideoView.Visibility = Nav.SelectedItem == RelatedViedeosItem ?
                Visibility.Visible : Visibility.Collapsed;
            }

            if (ViewModel.IsShowParts)
            {
                VideoPartView.Visibility = Nav.SelectedItem == PartsItem ?
                Visibility.Visible : Visibility.Collapsed;
            }

            if (ViewModel.IsShowEpisode)
            {
                EpisodeView.Visibility = Nav.SelectedItem == EpisodeItem ?
                Visibility.Visible : Visibility.Collapsed;
            }

            if (ViewModel.IsShowSeason)
            {
                SeasonView.Visibility = Nav.SelectedItem == SeasonItem ?
                Visibility.Visible : Visibility.Collapsed;
            }

            if (ViewModel.IsShowSection)
            {
                SectionView.Visibility = Nav.SelectedItem == SectionItem ?
                Visibility.Visible : Visibility.Collapsed;
            }

            if (ViewModel.IsShowChat)
            {
                ChatView.Visibility = Nav.SelectedItem == ChatItem ?
                Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}

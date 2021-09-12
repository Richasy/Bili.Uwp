// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using Richasy.Bili.Models.App.Constants;
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
            this.ViewModel.Loaded += OnViewModelLoaded;
        }

        private void OnViewModelLoaded(object sender, EventArgs e)
        {
            InitializeLayoutAsync();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            InitializeLayoutAsync();
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.IsDetailLoading))
            {
                if (ViewModel.IsDetailLoading)
                {
                    return;
                }

                if (!string.IsNullOrEmpty(ViewModel.InitializeSection))
                {
                    InitializeSelectedSection();
                }
                else if (ViewModel.IsPgc && ViewModel.IsCurrentEpisodeInPgcSection)
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
                if (!string.IsNullOrEmpty(ViewModel.InitializeSection))
                {
                    InitializeSelectedSection();
                }
                else if (ViewModel.IsCurrentEpisodeInPgcSection)
                {
                    Nav.SelectedItem = SectionItem;
                }
            }
            else if (e.PropertyName == nameof(ViewModel.IsDetailCanLoaded))
            {
                if (ViewModel.IsDetailCanLoaded)
                {
                    if (!string.IsNullOrEmpty(ViewModel.InitializeSection))
                    {
                        InitializeSelectedSection();
                    }

                    InitializeLayoutAsync();
                }
            }
        }

        private void OnNavSelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            InitializeLayoutAsync();
        }

        private async void InitializeLayoutAsync()
        {
            if (ViewModel.IsShowRelatedVideos && RelatedVideoView != null)
            {
                RelatedVideoView.Visibility = Nav.SelectedItem == RelatedViedeosItem ?
                Visibility.Visible : Visibility.Collapsed;
            }

            if (ViewModel.IsShowParts && VideoPartView != null)
            {
                VideoPartView.Visibility = Nav.SelectedItem == PartsItem ?
                Visibility.Visible : Visibility.Collapsed;
            }

            if (ViewModel.IsShowEpisode && EpisodeView != null)
            {
                EpisodeView.Visibility = Nav.SelectedItem == EpisodeItem ?
                Visibility.Visible : Visibility.Collapsed;
            }

            if (ViewModel.IsShowSeason && SeasonView != null)
            {
                SeasonView.Visibility = Nav.SelectedItem == SeasonItem ?
                Visibility.Visible : Visibility.Collapsed;
            }

            if (ViewModel.IsShowSection && SectionView != null)
            {
                SectionView.Visibility = Nav.SelectedItem == SectionItem ?
                Visibility.Visible : Visibility.Collapsed;
            }

            if (ViewModel.IsShowChat && ChatView != null)
            {
                ChatView.Visibility = Nav.SelectedItem == ChatItem ?
                Visibility.Visible : Visibility.Collapsed;
            }

            if (ViewModel.IsShowReply && ReplyView != null)
            {
                ReplyView.Visibility = Nav.SelectedItem == ReplyItem ?
                Visibility.Visible : Visibility.Collapsed;

                if (ReplyView.Visibility == Visibility.Visible)
                {
                    await ReplyView.CheckInitializeAsync();
                }
            }
        }

        private void InitializeSelectedSection()
        {
            switch (ViewModel.InitializeSection)
            {
                case AppConstants.ReplySection:
                    Nav.SelectedItem = ReplyItem;
                    break;
                default:
                    break;
            }

            ViewModel.InitializeSection = string.Empty;
        }
    }
}

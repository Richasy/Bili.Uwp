// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using Bili.Models.App.Constants;
using Windows.UI.Xaml;

namespace Bili.App.Controls
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
            InitializeComponent();
            Loaded += OnLoaded;
            ViewModel.Loaded += OnViewModelLoaded;
        }

        private void OnViewModelLoaded(object sender, EventArgs e)
            => InitializeLayout();

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            InitializeLayout();
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
                else if (ViewModel.IsShowViewLater)
                {
                    Nav.SelectedItem = ViewLaterItem;
                }
                else if (ViewModel.IsPgc && ViewModel.IsCurrentEpisodeInPgcSection)
                {
                    Nav.SelectedItem = SectionItem;
                }
                else if (ViewModel.IsShowParts)
                {
                    Nav.SelectedItem = PartsItem;
                }
                else if (ViewModel.IsShowUgcSection)
                {
                    Nav.SelectedItem = UgcEpisodeItem;
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

                    InitializeLayout();
                }
            }
        }

        private void OnNavSelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            InitializeLayout();
        }

        private void InitializeLayout()
        {
            if (ViewModel.IsShowViewLater && ViewLaterVideoView != null)
            {
                ViewLaterVideoView.Visibility = Nav.SelectedItem == ViewLaterItem ?
                Visibility.Visible : Visibility.Collapsed;
            }

            if (ViewModel.IsShowRelatedVideos && RelatedVideoView != null)
            {
                RelatedVideoView.Visibility = Nav.SelectedItem == RelatedViedeosItem ?
                Visibility.Visible : Visibility.Collapsed;
            }

            if (ViewModel.IsShowUgcSection && UgcEpisodeView != null)
            {
                UgcEpisodeView.Visibility = Nav.SelectedItem == UgcEpisodeItem ?
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
            }
        }

        private void InitializeSelectedSection()
        {
            switch (ViewModel.InitializeSection)
            {
                case AppConstants.ReplySection:
                    Nav.SelectedItem = ReplyItem;
                    break;
                case AppConstants.ViewLaterSection:
                    Nav.SelectedItem = ViewLaterItem;
                    break;
                default:
                    break;
            }

            ViewModel.InitializeSection = string.Empty;
        }
    }
}

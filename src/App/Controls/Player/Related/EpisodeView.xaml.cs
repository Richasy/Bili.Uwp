﻿// Copyright (c) Richasy. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;

namespace Richasy.Bili.App.Controls.Player.Related
{
    /// <summary>
    /// 分集视图.
    /// </summary>
    public sealed partial class EpisodeView : PlayerComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodeView"/> class.
        /// </summary>
        public EpisodeView()
        {
            this.InitializeComponent();
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

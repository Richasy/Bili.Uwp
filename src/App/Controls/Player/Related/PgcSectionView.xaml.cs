// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp;

namespace Bili.App.Controls.Player.Related
{
    /// <summary>
    /// PGC区块视图.
    /// </summary>
    public sealed partial class PgcSectionView : PlayerComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcSectionView"/> class.
        /// </summary>
        public PgcSectionView()
        {
            InitializeComponent();
        }

        private async void OnEpisodeItemClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var card = sender as CardPanel;
            var data = card.DataContext as PgcEpisodeViewModel;
            data.Data.IsPV = 1;
            if (!data.Data.Id.ToString().Equals(ViewModel.EpisodeId))
            {
                await ViewModel.LoadAsync(data.Data);
            }
        }
    }
}

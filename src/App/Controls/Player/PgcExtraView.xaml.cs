// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.ViewModels.Uwp.Pgc;
using ReactiveUI;
using Splat;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// PGC区块视图.
    /// </summary>
    public sealed partial class PgcExtraView : PgcExtraViewBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcExtraView"/> class.
        /// </summary>
        public PgcExtraView()
        {
            InitializeComponent();
            ViewModel = Splat.Locator.Current.GetService<PgcPlayerPageViewModel>();
            DataContext = ViewModel;
        }

        private void OnEpisodeItemClick(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var card = sender as CardPanel;
            var data = card.DataContext as EpisodeItemViewModel;
            if (!ViewModel.CurrentEpisode.Equals(data.Data))
            {
                ViewModel.ChangeEpisodeCommand.Execute(data.Data).Subscribe();
            }
        }
    }

    /// <summary>
    /// <see cref="PgcExtraView"/> 的基类.
    /// </summary>
    public class PgcExtraViewBase : ReactiveUserControl<PgcPlayerPageViewModel>
    {
    }
}

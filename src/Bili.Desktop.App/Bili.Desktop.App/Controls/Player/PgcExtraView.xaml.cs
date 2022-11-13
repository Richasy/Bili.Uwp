// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Pgc;

namespace Bili.Desktop.App.Controls.Player
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
            DataContext = ViewModel;
        }

        private void OnEpisodeItemClick(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            var card = sender as CardPanel;
            var data = card.DataContext as IEpisodeItemViewModel;
            if (!ViewModel.CurrentEpisode.Equals(data.Data))
            {
                ViewModel.ChangeEpisodeCommand.Execute(data.Data);
            }
        }
    }

    /// <summary>
    /// <see cref="PgcExtraView"/> 的基类.
    /// </summary>
    public class PgcExtraViewBase : ReactiveUserControl<IPgcPlayerPageViewModel>
    {
    }
}

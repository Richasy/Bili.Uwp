// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
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
            InitializeComponent();
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
                data.IsSelected = false;
                await Task.Delay(100);
                data.IsSelected = true;
            }
        }

        private void OnEpisodeRepeaterLoaded(object sender, RoutedEventArgs e)
            => RelocateSelectedItem();

        private void RelocateSelectedItem()
        {
            var vm = ViewModel.EpisodeCollection.FirstOrDefault(p => p.IsSelected);
            if (vm != null)
            {
                var index = ViewModel.EpisodeCollection.IndexOf(vm);
                if (index >= 0)
                {
                    EpisodeRepeater.ScrollToItem(index);
                    if (ViewModel.IsOnlyShowIndex)
                    {
                        var ele = IndexRepeater.GetOrCreateElement(index);
                        if (ele != null)
                        {
                            ele.StartBringIntoView(new BringIntoViewOptions { VerticalAlignmentRatio = 0f });
                        }
                    }
                }
            }
        }
    }
}

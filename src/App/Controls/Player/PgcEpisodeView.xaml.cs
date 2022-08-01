// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Bili.ViewModels.Interfaces.Pgc;
using Bili.ViewModels.Uwp.Pgc;
using ReactiveUI;
using Splat;
using Windows.UI.Xaml;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// PGC 分集视图.
    /// </summary>
    public sealed partial class PgcEpisodeView : PgcEpisodeViewBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcEpisodeView"/> class.
        /// </summary>
        public PgcEpisodeView()
        {
            InitializeComponent();
            ViewModel = Splat.Locator.Current.GetService<PgcPlayerPageViewModel>();
            DataContext = ViewModel;
        }

        private async void OnEpisodeItemClickAsync(object sender, RoutedEventArgs e)
        {
            var card = sender as CardPanel;
            var data = card.DataContext as IEpisodeItemViewModel;
            if (!data.Data.Equals(ViewModel.CurrentEpisode))
            {
                ViewModel.ChangeEpisodeCommand.Execute(data.Data).Subscribe();
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
            var vm = ViewModel.Episodes.FirstOrDefault(p => p.IsSelected);
            if (vm != null)
            {
                var index = ViewModel.Episodes.IndexOf(vm);
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

    /// <summary>
    /// <see cref="PgcEpisodeView"/> 的基类.
    /// </summary>
    public class PgcEpisodeViewBase : ReactiveUserControl<PgcPlayerPageViewModel>
    {
    }
}

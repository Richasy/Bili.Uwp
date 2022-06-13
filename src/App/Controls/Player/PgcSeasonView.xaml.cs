// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Bili.ViewModels.Uwp.Pgc;
using Bili.ViewModels.Uwp.Video;
using ReactiveUI;
using Splat;
using Windows.UI.Xaml;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// 剧集/系列视图.
    /// </summary>
    public sealed partial class PgcSeasonView : PgcSeasonViewBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcSeasonView"/> class.
        /// </summary>
        public PgcSeasonView()
        {
            InitializeComponent();
            ViewModel = Splat.Locator.Current.GetService<PgcPlayerPageViewModel>();
            DataContext = ViewModel;
        }

        private async void OnSeasonItemClickAsync(object sender, RoutedEventArgs e)
        {
            var card = sender as CardPanel;
            var data = card.DataContext as VideoIdentifierSelectableViewModel;
            if (!data.Data.Id.Equals(ViewModel.View.Information.Identifier.Id) || ViewModel.CurrentEpisode.IsPreviewVideo)
            {
                ViewModel.SetSnapshot(new Models.Data.Local.PlaySnapshot(default, data.Data.Id, Models.Enums.VideoType.Pgc));
            }
            else
            {
                data.IsSelected = false;
                await Task.Delay(100);
                data.IsSelected = true;
            }
        }
    }

    /// <summary>
    /// <see cref="PgcSeasonView"/> 的基类.
    /// </summary>
    public class PgcSeasonViewBase : ReactiveUserControl<PgcPlayerPageViewModel>
    {
    }
}

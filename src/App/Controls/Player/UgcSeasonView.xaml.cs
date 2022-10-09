// Copyright (c) Richasy. All rights reserved.

using System.Linq;
using Bili.Models.Data.Video;
using Bili.ViewModels.Interfaces.Video;

namespace Bili.App.Controls.Player
{
    /// <summary>
    /// 合集视图.
    /// </summary>
    public sealed partial class UgcSeasonView : UgcSeasonViewBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UgcSeasonView"/> class.
        /// </summary>
        public UgcSeasonView() => InitializeComponent();

        private void OnSeasonComboBoxSelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            var season = SeasonComboBox.SelectedItem as VideoSeason;
            if (ViewModel.CurrentSeason != season)
            {
                ViewModel.SelectSeasonCommand.Execute(season);
            }
        }

        private async void OnRepeaterLoadedAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await System.Threading.Tasks.Task.Delay(200);
            if (ViewModel.IsShowUgcSeason)
            {
                var selectedVideo = ViewModel.CurrentSeasonVideos.FirstOrDefault(p => p.IsSelected);
                if (selectedVideo != null)
                {
                    var index = ViewModel.CurrentSeasonVideos.IndexOf(selectedVideo);
                    if (index > 0)
                    {
                        (sender as VerticalRepeaterView).ScrollToItem(index);
                    }
                }
            }
        }
    }

    /// <summary>
    /// <see cref="UgcSeason"/> 的基类.
    /// </summary>
    public class UgcSeasonViewBase : ReactiveUserControl<IVideoPlayerPageViewModel>
    {
    }
}

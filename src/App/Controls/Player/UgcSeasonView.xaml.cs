// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Video;
using Bili.ViewModels.Uwp.Video;
using Bilibili.App.View.V1;
using ReactiveUI;
using Splat;

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
        public UgcSeasonView()
        {
            InitializeComponent();
            ViewModel = Splat.Locator.Current.GetService<VideoPlayerPageViewModel>();
            DataContext = ViewModel;
        }

        private void OnSeasonComboBoxSelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            var season = SeasonComboBox.SelectedItem as VideoSeason;
            if (ViewModel.CurrentSeason != season)
            {
                ViewModel.CurrentSeason = season;
            }
        }
    }

    /// <summary>
    /// <see cref="UgcSeason"/> 的基类.
    /// </summary>
    public class UgcSeasonViewBase : ReactiveUserControl<VideoPlayerPageViewModel>
    {
    }
}

// Copyright (c) Richasy. All rights reserved.

using System.Threading.Tasks;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;

namespace Richasy.Bili.App.Controls.Player.Related
{
    /// <summary>
    /// 剧集/系列视图.
    /// </summary>
    public sealed partial class SeasonView : PlayerComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonView"/> class.
        /// </summary>
        public SeasonView() => InitializeComponent();

        private async void OnSeasonItemClickAsync(object sender, RoutedEventArgs e)
        {
            var card = sender as CardPanel;
            var data = card.DataContext as PgcSeasonViewModel;
            if (!data.Data.SeasonId.ToString().Equals(ViewModel.SeasonId) || ViewModel.IsPv)
            {
                await ViewModel.LoadAsync(data.Data);
            }
            else
            {
                data.IsSelected = false;
                await Task.Delay(100);
                data.IsSelected = true;
            }
        }
    }
}

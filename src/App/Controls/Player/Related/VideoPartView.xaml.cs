// Copyright (c) GodLeaveMe. All rights reserved.

using Richasy.Bili.ViewModels.Uwp;

namespace Richasy.Bili.App.Controls.Player.Related
{
    /// <summary>
    /// 视频分集.
    /// </summary>
    public sealed partial class VideoPartView : PlayerComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPartView"/> class.
        /// </summary>
        public VideoPartView()
        {
            this.InitializeComponent();
        }

        private async void OnPartItemClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var card = sender as CardPanel;
            var data = card.DataContext as VideoPartViewModel;
            if (!data.Data.Equals(ViewModel.CurrentVideoPart))
            {
                await ViewModel.ChangeVideoPartAsync(data.Data.Page.Cid);
            }
            else
            {
                data.IsSelected = true;
            }
        }
    }
}

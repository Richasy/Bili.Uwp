// Copyright (c) Richasy. All rights reserved.

using Windows.UI.Xaml;

namespace Bili.App.Controls.Player.PlayerControls
{
    /// <summary>
    /// 播放速率按钮.
    /// </summary>
    public sealed partial class PlayRateButton : PlayerControlBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayRateButton"/> class.
        /// </summary>
        public PlayRateButton()
        {
            this.InitializeComponent();
        }

        private void OnPlaybackRatePresetButtonClick(object sender, RoutedEventArgs e)
        {
            var rate = (double)(sender as FrameworkElement).DataContext;
            ViewModel.ChangePlayRateCommand.Execute(rate);
        }
    }
}

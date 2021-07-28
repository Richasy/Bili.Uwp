// Copyright (c) Richasy. All rights reserved.

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 播放数据面板.
    /// </summary>
    public sealed partial class PlayerDashboard : PlayerComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerDashboard"/> class.
        /// </summary>
        public PlayerDashboard()
        {
            this.InitializeComponent();
        }

        private void OnLikeButtonHoldingStart(object sender, System.EventArgs e)
        {
            CoinButton.BeginProgressAnimation(true);
            FavoriteButton.BeginProgressAnimation(true);
        }

        private void OnButtonHoldingSuspend(object sender, System.EventArgs e)
        {
            CoinButton.StopProgressAnimation();
            FavoriteButton.StopProgressAnimation();
        }
    }
}

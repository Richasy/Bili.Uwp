// Copyright (c) Richasy. All rights reserved.

using System;

namespace Richasy.Bili.App.Controls.Player.Related
{
    /// <summary>
    /// 直播消息视图.
    /// </summary>
    public sealed partial class LiveMessageView : PlayerComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveMessageView"/> class.
        /// </summary>
        public LiveMessageView()
        {
            this.InitializeComponent();
            ViewModel.RequestLiveMessageScrollToBottom += OnRequestLiveMessageScrollToBottom;
        }

        private void OnRequestLiveMessageScrollToBottom(object sender, EventArgs e)
        {
            ScrollViewer.ChangeView(0, (ScrollViewer.ExtentHeight + ScrollViewer.ScrollableHeight) * 2, 1);
        }
    }
}

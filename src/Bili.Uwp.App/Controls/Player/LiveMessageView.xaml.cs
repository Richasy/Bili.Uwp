// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.ViewModels.Interfaces.Live;

namespace Bili.Uwp.App.Controls.Player
{
    /// <summary>
    /// 直播消息视图.
    /// </summary>
    public sealed partial class LiveMessageView : LiveMessageViewBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveMessageView"/> class.
        /// </summary>
        public LiveMessageView()
        {
            InitializeComponent();
            ViewModel = Locator.Instance.GetService<ILivePlayerPageViewModel>();
            DataContext = ViewModel;
            ViewModel.RequestDanmakusScrollToBottom += OnRequestDanmakusScrollToBottomAsync;
        }

        private async void OnRequestDanmakusScrollToBottomAsync(object sender, EventArgs e)
        {
            await Task.Delay(50);
            ScrollViewer.ChangeView(0, double.MaxValue, 1);
        }

        private void OnViewChanged(object sender, Windows.UI.Xaml.Controls.ScrollViewerViewChangedEventArgs e)
        {
            if (!e.IsIntermediate)
            {
                // 这里的逻辑是，如果滚动到了底部，则表示允许视图自动滚动，
                // 如果不在底部，表示用户自己滚动了视图，此时则不再自动滚动.
                ViewModel.IsDanmakusAutoScroll = ScrollViewer.VerticalOffset + ScrollViewer.ViewportHeight >= ScrollViewer.ExtentHeight - 50;
            }
        }
    }

    /// <summary>
    /// <see cref="LiveMessageView"/> 的基类.
    /// </summary>
    public class LiveMessageViewBase : ReactiveUserControl<ILivePlayerPageViewModel>
    {
    }
}

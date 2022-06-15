// Copyright (c) Richasy. All rights reserved.

namespace Bili.ViewModels.Uwp.Live
{
    /// <summary>
    /// 直播播放页面视图模型.
    /// </summary>
    public sealed partial class LivePlayerPageViewModel
    {
        private void ResetTimers()
        {
            if (_heartBeatTimer != null)
            {
                _heartBeatTimer.Stop();
            }
        }

        private void ResetPublisher()
            => User = null;

        private void ResetOverview()
            => WatchingCountText = default;

        private void ResetInterop()
        {
            // TODO：重置下载内容.
            IsLiveFixed = false;
            IsDanmakusAutoScroll = true;
            Danmakus.Clear();
            _liveProvider.ResetLiveConnection();
        }
    }
}

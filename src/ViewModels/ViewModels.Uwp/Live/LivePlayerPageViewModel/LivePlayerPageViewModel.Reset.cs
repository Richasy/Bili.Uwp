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
        {
            IsError = false;
            WatchingCountText = default;
        }

        private void ResetInterop()
        {
            IsLiveFixed = false;
            IsDanmakusAutoScroll = true;
            TryClear(Danmakus);
            _liveProvider.ResetLiveConnection();
        }
    }
}

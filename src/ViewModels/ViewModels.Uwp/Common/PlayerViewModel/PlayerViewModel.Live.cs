// Copyright (c) Richasy. All rights reserved.

using System;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 播放器视图模型.
    /// </summary>
    public partial class PlayerViewModel
    {
        private async void OnLiveMessageReceivedAsync(object sender, LiveMessageEventArgs e)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (e.Type == LiveMessageType.ConnectSuccess)
                {
                    _heartBeatTimer.Start();
                }
                else if (e.Type == LiveMessageType.Danmaku)
                {
                    var data = e.Data as LiveDanmakuMessage;
                    LiveDanmakuCollection.Add(data);
                    NewLiveDanmakuAdded?.Invoke(this, data);
                }
            });
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using Bili.Models.App.Args;
using Bili.Models.BiliBili;
using Bili.Models.Enums;

namespace Bili.ViewModels.Uwp
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
                    if (LiveDanmakuCollection.Count > 1000)
                    {
                        var saveMessages = LiveDanmakuCollection.Skip(600).ToList();
                        LiveDanmakuCollection.Clear();
                        saveMessages.ForEach(x => LiveDanmakuCollection.Add(x));
                    }
                }
            });
        }
    }
}

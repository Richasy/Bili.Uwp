// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.Enums;

namespace Bili.Models.App.Args
{
    /// <summary>
    /// 媒体状态更改事件参数.
    /// </summary>
    public sealed class MediaStateChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaStateChangedEventArgs"/> class.
        /// </summary>
        public MediaStateChangedEventArgs(PlayerStatus status, string message)
        {
            Status = status;
            Message = message;
        }

        /// <summary>
        /// 播放器状态.
        /// </summary>
        public PlayerStatus Status { get; set; }

        /// <summary>
        /// 消息.
        /// </summary>
        public string Message { get; set; }
    }
}

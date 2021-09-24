// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 直播间消息信息.
    /// </summary>
    public class LiveMessageEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LiveMessageEventArgs"/> class.
        /// </summary>
        /// <param name="type">消息类型.</param>
        /// <param name="data">传递参数.</param>
        public LiveMessageEventArgs(LiveMessageType type, object data)
        {
            Type = type;
            Data = data;
        }

        /// <summary>
        /// 消息类型.
        /// </summary>
        public LiveMessageType Type { get; set; }

        /// <summary>
        /// 传递参数.
        /// </summary>
        public object Data { get; set; }
    }
}

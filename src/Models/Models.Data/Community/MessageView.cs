// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Community
{
    /// <summary>
    /// 消息视图.
    /// </summary>
    public sealed class MessageView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageView"/> class.
        /// </summary>
        /// <param name="messages">消息列表.</param>
        /// <param name="isFinished">是否已经结束.</param>
        public MessageView(
            IEnumerable<MessageInformation> messages,
            bool isFinished)
        {
            Messages = messages;
            IsFinished = isFinished;
        }

        /// <summary>
        /// 消息列表.
        /// </summary>
        public IEnumerable<MessageInformation> Messages { get; }

        /// <summary>
        /// 是否已经结束.
        /// </summary>
        public bool IsFinished { get; }
    }
}

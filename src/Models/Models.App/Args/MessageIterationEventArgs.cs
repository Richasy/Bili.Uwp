// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums.App;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 消息更新事件参数.
    /// </summary>
    public class MessageIterationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageIterationEventArgs"/> class.
        /// </summary>
        /// <param name="response">响应结果.</param>
        public MessageIterationEventArgs(LikeMessageResponse response)
            : this()
        {
            Type = MessageType.Like;
            if (response.Latest != null)
            {
                foreach (var item in response.Latest.Items)
                {
                    (item as LikeMessageItem).IsLatest = true;
                }

                response.Latest.Items.ForEach(p => Items.Add(p));
            }

            if (response.Total != null)
            {
                Cursor = response.Total.Cursor;
                response.Total.Items.ForEach(p => Items.Add(p));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageIterationEventArgs"/> class.
        /// </summary>
        /// <param name="response">@我的消息响应.</param>
        public MessageIterationEventArgs(AtMessageResponse response)
            : this()
        {
            Type = MessageType.At;
            Cursor = response.Cursor;
            response.Items.ForEach(p => Items.Add(p));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageIterationEventArgs"/> class.
        /// </summary>
        /// <param name="response">回复我的响应.</param>
        public MessageIterationEventArgs(ReplyMessageResponse response)
            : this()
        {
            Type = MessageType.Reply;
            Cursor = response.Cursor;
            response.Items.ForEach(p => Items.Add(p));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageIterationEventArgs"/> class.
        /// </summary>
        public MessageIterationEventArgs()
        {
            Items = new List<MessageItem>();
        }

        /// <summary>
        /// 偏移指针.
        /// </summary>
        public MessageCursor Cursor { get; set; }

        /// <summary>
        /// 条目列表.
        /// </summary>
        public List<MessageItem> Items { get; set; }

        /// <summary>
        /// 消息类型.
        /// </summary>
        public MessageType Type { get; set; }
    }
}

// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Community;

namespace Bili.App.Controls.Community
{
    /// <summary>
    /// 消息条目.
    /// </summary>
    public sealed class MessageItem : ReactiveControl<MessageItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageItem"/> class.
        /// </summary>
        public MessageItem() => DefaultStyleKey = typeof(MessageItem);
    }
}

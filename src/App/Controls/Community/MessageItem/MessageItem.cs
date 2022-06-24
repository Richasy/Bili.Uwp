// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Uwp.Community;
using Windows.Foundation;

namespace Bili.App.Controls.Community
{
    /// <summary>
    /// 消息条目.
    /// </summary>
    public sealed class MessageItem : ReactiveControl<MessageItemViewModel>, IRepeaterItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageItem"/> class.
        /// </summary>
        public MessageItem() => DefaultStyleKey = typeof(MessageItem);

        /// <inheritdoc/>
        public Size GetHolderSize() => new Size(200, 120);
    }
}

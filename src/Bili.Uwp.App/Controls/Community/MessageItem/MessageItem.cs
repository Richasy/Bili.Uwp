// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Community;
using Windows.Foundation;

namespace Bili.Uwp.App.Controls.Community
{
    /// <summary>
    /// 消息条目.
    /// </summary>
    public sealed class MessageItem : ReactiveControl<IMessageItemViewModel>, IRepeaterItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageItem"/> class.
        /// </summary>
        public MessageItem() => DefaultStyleKey = typeof(MessageItem);

        /// <inheritdoc/>
        public Size GetHolderSize() => new Size(200, 120);
    }
}

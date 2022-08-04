// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Reactive;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 消息条目视图模型.
    /// </summary>
    public sealed partial class MessageItemViewModel
    {
        private readonly ICallerViewModel _callerViewModel;
        private readonly IResourceToolkit _resourceToolkit;

        /// <inheritdoc/>
        [Reactive]
        public MessageInformation Data { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string PublishTime { get; set; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ActiveCommand { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is MessageItemViewModel model && EqualityComparer<MessageInformation>.Default.Equals(Data, model.Data);

        /// <inheritdoc/>
        public override int GetHashCode() => Data.GetHashCode();
    }
}

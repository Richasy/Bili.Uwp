// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Reactive;
using Bili.Models.Data.Community;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 消息条目视图模型.
    /// </summary>
    public sealed partial class MessageItemViewModel
    {
        private readonly AppViewModel _appViewModel;

        /// <summary>
        /// 消息信息.
        /// </summary>
        [Reactive]
        public MessageInformation Information { get; internal set; }

        /// <summary>
        /// 可读的发布时间.
        /// </summary>
        [Reactive]
        public string PublishTime { get; set; }

        /// <summary>
        /// 激活命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ActiveCommand { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is MessageItemViewModel model && EqualityComparer<MessageInformation>.Default.Equals(Information, model.Information);

        /// <inheritdoc/>
        public override int GetHashCode() => Information.GetHashCode();
    }
}

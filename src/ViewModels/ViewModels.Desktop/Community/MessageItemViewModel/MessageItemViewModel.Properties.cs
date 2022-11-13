// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Community
{
    /// <summary>
    /// 消息条目视图模型.
    /// </summary>
    public sealed partial class MessageItemViewModel
    {
        private readonly ICallerViewModel _callerViewModel;
        private readonly IResourceToolkit _resourceToolkit;

        [ObservableProperty]
        private MessageInformation _data;

        [ObservableProperty]
        private string _publishTime;

        /// <inheritdoc/>
        public IAsyncRelayCommand ActiveCommand { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is MessageItemViewModel model && EqualityComparer<MessageInformation>.Default.Equals(Data, model.Data);

        /// <inheritdoc/>
        public override int GetHashCode() => Data.GetHashCode();
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Player;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Common
{
    /// <summary>
    /// 互动视频模块视图模型.
    /// </summary>
    public sealed partial class InteractionModuleViewModel
    {
        private readonly IPlayerProvider _playerProvider;

        private string _partId;
        private string _choiceId;
        private string _graphVersion;

        /// <inheritdoc/>
        public event EventHandler NoMoreChoices;

        /// <inheritdoc/>
        public IRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<InteractionInformation> Choices { get; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsReloading { get; set; }
    }
}

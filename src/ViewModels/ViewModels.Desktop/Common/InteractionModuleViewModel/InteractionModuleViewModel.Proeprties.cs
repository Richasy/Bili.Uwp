// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Player;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Common
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

        [ObservableProperty]
        private bool _isReloading;

        /// <inheritdoc/>
        public event EventHandler NoMoreChoices;

        /// <inheritdoc/>
        public IAsyncRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<InteractionInformation> Choices { get; }
    }
}

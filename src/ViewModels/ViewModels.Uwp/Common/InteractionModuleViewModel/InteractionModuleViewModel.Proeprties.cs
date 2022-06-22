// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Player;
using ReactiveUI;

namespace Bili.ViewModels.Uwp.Common
{
    /// <summary>
    /// 互动视频模块视图模型.
    /// </summary>
    public sealed partial class InteractionModuleViewModel
    {
        private readonly IPlayerProvider _playerProvider;
        private readonly ObservableAsPropertyHelper<bool> _isReloading;

        private string _partId;
        private string _choiceId;
        private string _graphVersion;

        /// <summary>
        /// 无法获取到选项时发生.
        /// </summary>
        public event EventHandler NoMoreChoices;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <summary>
        /// 选择集合.
        /// </summary>
        public ObservableCollection<InteractionInformation> Choices { get; }

        /// <inheritdoc/>
        public bool IsReloading => _isReloading.Value;
    }
}

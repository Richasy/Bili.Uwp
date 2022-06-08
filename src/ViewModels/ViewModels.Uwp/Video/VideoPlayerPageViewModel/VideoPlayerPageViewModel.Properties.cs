// Copyright (c) Richasy. All rights reserved.

using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频播放页面视图模型.
    /// </summary>
    public sealed partial class VideoPlayerPageViewModel
    {
        private readonly IPlayerProvider _playerProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly AppViewModel _appViewModel;
        private readonly NavigationViewModel _navigationViewModel;
        private readonly ObservableAsPropertyHelper<bool> _isReloading;

        private string _presetVideoId;
        private VideoIdentifier _identifier;

        /// <inheritdoc/>
        public bool IsReloading => _isReloading.Value;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsError { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string ErrorText { get; set; }
    }
}

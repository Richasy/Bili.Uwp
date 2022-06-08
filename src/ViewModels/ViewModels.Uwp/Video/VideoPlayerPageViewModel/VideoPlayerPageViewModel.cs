// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp.Core;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频播放页面视图模型.
    /// </summary>
    public sealed partial class VideoPlayerPageViewModel : ViewModelBase, IReloadViewModel, IErrorViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoPlayerPageViewModel"/> class.
        /// </summary>
        public VideoPlayerPageViewModel(
            IPlayerProvider playerProvider,
            IResourceToolkit resourceToolkit,
            AppViewModel appViewModel,
            NavigationViewModel navigationViewModel)
        {
            _playerProvider = playerProvider;
            _resourceToolkit = resourceToolkit;
            _appViewModel = appViewModel;
            _navigationViewModel = navigationViewModel;
        }

        /// <summary>
        /// 设置视频 Id.
        /// </summary>
        /// <param name="videoId">视频 Id.</param>
        public void SetViedoId(string videoId)
        {
            _presetVideoId = videoId;
            ReloadCommand.Execute().Subscribe();
        }

        /// <inheritdoc/>
        public void DisplayException(Exception exception)
        {
            IsError = true;
            var msg = exception is ServiceException se
                ? se.Error?.Message ?? se.Message
                : exception.Message;
            ErrorText = $"{_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestVideoFailed)}\n{exception.Message}";
            LogException(exception);
        }
    }
}

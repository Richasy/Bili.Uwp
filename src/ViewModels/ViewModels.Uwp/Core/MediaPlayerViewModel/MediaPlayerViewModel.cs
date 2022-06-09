// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Player;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp.Account;
using FFmpegInterop;
using ReactiveUI;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 媒体播放器视图模型.
    /// </summary>
    public sealed partial class MediaPlayerViewModel : ViewModelBase, IReloadViewModel, IErrorViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPlayerViewModel"/> class.
        /// </summary>
        public MediaPlayerViewModel(
            IPlayerProvider playerProvider,
            IResourceToolkit resourceToolkit,
            IFileToolkit fileToolkit,
            AccountViewModel accountViewModel,
            CoreDispatcher dispatcher)
        {
            _playerProvider = playerProvider;
            _resourceToolkit = resourceToolkit;
            _fileToolkit = fileToolkit;
            _accountViewModel = accountViewModel;
            _dispatcher = dispatcher;

            _liveConfig = new FFmpegInteropConfig();
            _liveConfig.FFmpegOptions.Add("referer", "https://live.bilibili.com/");
            _liveConfig.FFmpegOptions.Add("user-agent", "Mozilla/5.0 BiliDroid/1.12.0 (bbcallen@gmail.com)");

            Volume = _settingsToolkit.ReadLocalSetting(SettingNames.Volume, 100d);
            PlaybackRate = _settingsToolkit.ReadLocalSetting(SettingNames.PlaybackRate, 1d);

            Formats = new ObservableCollection<FormatInformation>();
            ReloadCommand = ReactiveCommand.CreateFromTask(LoadAsync, outputScheduler: RxApp.MainThreadScheduler);
        }

        /// <summary>
        /// 设置数据.
        /// </summary>
        /// <param name="data">视图数据.</param>
        /// <param name="type">视频类型.</param>
        public void SetData(object data, VideoType type)
        {
            _viewData = data;
            _videoType = type;
        }

        /// <inheritdoc/>
        public void DisplayException(Exception exception)
        {
            IsError = true;
            var msg = GetErrorMessage(exception);
            ErrorText = $"{_resourceToolkit.GetLocaleString(LanguageNames.RequestVideoFailed)}\n{msg}";
            LogException(exception);
        }

        private void Reset()
        {
            ResetVideoData();
        }

        private async Task LoadAsync()
        {
            Reset();
            if (_videoType == VideoType.Video)
            {
                await LoadVideoAsync();
            }
        }
    }
}

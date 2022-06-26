// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Live;
using Bili.Models.Data.Local;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;

namespace Bili.ViewModels.Uwp.Common
{
    /// <summary>
    /// 弹幕视图模型.
    /// </summary>
    public sealed partial class DanmakuModuleViewModel : ViewModelBase, IReloadViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DanmakuModuleViewModel"/> class.
        /// </summary>
        public DanmakuModuleViewModel(
            ISettingsToolkit settingsToolkit,
            IFontToolkit fontToolkit,
            IResourceToolkit resourceToolkit,
            IPlayerProvider playerProvider,
            ILiveProvider liveProvider,
            AppViewModel appViewModel)
        {
            _settingsToolkit = settingsToolkit;
            _fontToolkit = fontToolkit;
            _resourceToolkit = resourceToolkit;
            _playerProvider = playerProvider;
            _liveProvider = liveProvider;
            _appViewModel = appViewModel;

            FontCollection = new ObservableCollection<string>();
            LocationCollection = new ObservableCollection<Models.Enums.App.DanmakuLocation>();
            ColorCollection = new ObservableCollection<KeyValue<string>>();

            ResetCommand = ReactiveCommand.Create(Reset, outputScheduler: RxApp.MainThreadScheduler);
            SendDanmakuCommand = ReactiveCommand.CreateFromTask<string, bool>(SendDanmakuAsync, outputScheduler: RxApp.MainThreadScheduler);
            ReloadCommand = ReactiveCommand.CreateFromTask(ReloadAsync, outputScheduler: RxApp.MainThreadScheduler);
            LoadSegmentDanmakuCommand = ReactiveCommand.CreateFromTask<int>(LoadSegmentDanmakuAsync, outputScheduler: RxApp.MainThreadScheduler);
            SeekCommand = ReactiveCommand.Create<double>(Seek, outputScheduler: RxApp.MainThreadScheduler);
            AddLiveDanmakuCommand = ReactiveCommand.Create<LiveDanmakuInformation>(AddLiveDanmaku, outputScheduler: RxApp.MainThreadScheduler);

            _isReloading = ReloadCommand.IsExecuting.ToProperty(this, x => x.IsReloading, scheduler: RxApp.MainThreadScheduler);
            _isDanmakuLoading = LoadSegmentDanmakuCommand.IsExecuting.ToProperty(this, x => x.IsDanmakuLoading, scheduler: RxApp.MainThreadScheduler);

            SendDanmakuCommand.ThrownExceptions
                .Merge(ReloadCommand.ThrownExceptions)
                .Merge(LoadSegmentDanmakuCommand.ThrownExceptions)
                .Subscribe(LogException);

            Initialize();
        }

        /// <summary>
        /// 加载弹幕.
        /// </summary>
        /// <param name="mainId">视频 Id.</param>
        /// <param name="partId">分P Id.</param>
        public void SetData(string mainId, string partId, VideoType type)
        {
            _mainId = mainId;
            _partId = partId;
            _videoType = type;

            ReloadCommand.Execute().Subscribe();
        }

        /// <summary>
        /// 重置.
        /// </summary>
        private void Reset()
        {
            _segmentIndex = 0;
            _currentSeconds = 0;
            RequestClearDanmaku?.Invoke(this, EventArgs.Empty);
        }

        private async Task ReloadAsync()
        {
            if (IsReloading)
            {
                return;
            }

            Reset();
            await LoadSegmentDanmakuAsync(1);
        }

        private async Task LoadSegmentDanmakuAsync(int index)
        {
            if (IsDanmakuLoading || _segmentIndex == index || !IsShowDanmaku || _videoType == VideoType.Live)
            {
                return;
            }

            var danmakus = await _playerProvider.GetSegmentDanmakuAsync(_mainId, _partId, index);
            DanmakuListAdded?.Invoke(this, danmakus);
            _segmentIndex = index;
        }

        private async Task<bool> SendDanmakuAsync(string danmakuText)
        {
            var result = _videoType == VideoType.Live
                ? await SendLiveDanmakuAsync(danmakuText)
                : await SendVideoDanmakuAsync(danmakuText);

            if (!result)
            {
                _appViewModel.ShowTip(_resourceToolkit.GetLocaleString(LanguageNames.FailedToSendDanmaku), Models.Enums.App.InfoType.Error);
            }

            return result;
        }

        /// <summary>
        /// 发送弹幕.
        /// </summary>
        /// <param name="danmakuText">弹幕文本.</param>
        /// <returns>发送结果.</returns>
        private async Task<bool> SendVideoDanmakuAsync(string danmakuText)
        {
            var result = await _playerProvider.SendDanmakuAsync(
                danmakuText,
                _mainId,
                _partId,
                Convert.ToInt32(_currentSeconds),
                ToDanmakuColor(Color),
                IsStandardSize,
                Location);

            if (result)
            {
                SendDanmakuSucceeded?.Invoke(this, danmakuText);
            }

            return result;
        }

        /// <summary>
        /// 发送弹幕.
        /// </summary>
        /// <param name="danmakuText">弹幕文本.</param>
        /// <returns>发送结果.</returns>
        private async Task<bool> SendLiveDanmakuAsync(string danmakuText)
        {
            var result = await _liveProvider.SendDanmakuAsync(
                _mainId,
                danmakuText,
                ToDanmakuColor(Color),
                IsStandardSize,
                Location);

            return result;
        }

        private void AddLiveDanmaku(LiveDanmakuInformation info)
            => LiveDanmakuAdded?.Invoke(this, info);
    }
}

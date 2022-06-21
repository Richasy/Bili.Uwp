// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Local;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
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
            IPlayerProvider playerProvider)
        {
            _settingsToolkit = settingsToolkit;
            _fontToolkit = fontToolkit;
            _resourceToolkit = resourceToolkit;
            _playerProvider = playerProvider;

            FontCollection = new ObservableCollection<string>();
            LocationCollection = new ObservableCollection<Models.Enums.App.DanmakuLocation>();
            ColorCollection = new ObservableCollection<KeyValue<string>>();

            ResetCommand = ReactiveCommand.Create(Reset, outputScheduler: RxApp.MainThreadScheduler);
            SendDanmakuCommand = ReactiveCommand.CreateFromTask<string, bool>(SendDanmakuAsync, outputScheduler: RxApp.MainThreadScheduler);
            ReloadCommand = ReactiveCommand.CreateFromTask(ReloadAsync, outputScheduler: RxApp.MainThreadScheduler);
            LoadSegmentDanmakuCommand = ReactiveCommand.CreateFromTask<int>(LoadSegmentDanmakuAsync, outputScheduler: RxApp.MainThreadScheduler);
            SeekCommand = ReactiveCommand.Create<double>(Seek, outputScheduler: RxApp.MainThreadScheduler);

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
        public void SetData(string mainId, string partId)
        {
            _mainId = mainId;
            _partId = partId;

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
            Reset();
            await LoadSegmentDanmakuAsync(1);
        }

        private async Task LoadSegmentDanmakuAsync(int index)
        {
            if (IsDanmakuLoading || _segmentIndex == index)
            {
                return;
            }

            var danmakus = await _playerProvider.GetSegmentDanmakuAsync(_mainId, _partId, index);
            DanmakuListAdded?.Invoke(this, danmakus);
            _segmentIndex = index;
        }

        /// <summary>
        /// 发送弹幕.
        /// </summary>
        /// <param name="danmakuText">弹幕文本.</param>
        /// <returns>发送结果.</returns>
        private async Task<bool> SendDanmakuAsync(string danmakuText)
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

        private void Seek(double seconds)
            => _currentSeconds = seconds;

        private void Initialize()
        {
            IsShowDanmaku = _settingsToolkit.ReadLocalSetting(SettingNames.IsShowDanmaku, true);
            DanmakuOpacity = _settingsToolkit.ReadLocalSetting(SettingNames.DanmakuOpacity, 0.8);
            DanmakuFontSize = _settingsToolkit.ReadLocalSetting(SettingNames.DanmakuFontSize, 1.5d);
            DanmakuArea = _settingsToolkit.ReadLocalSetting(SettingNames.DanmakuArea, 1d);
            DanmakuSpeed = _settingsToolkit.ReadLocalSetting(SettingNames.DanmakuSpeed, 1d);
            DanmakuFont = _settingsToolkit.ReadLocalSetting(SettingNames.DanmakuFont, "Segoe UI");
            IsDanmakuMerge = _settingsToolkit.ReadLocalSetting(SettingNames.IsDanmakuMerge, false);
            IsDanmakuBold = _settingsToolkit.ReadLocalSetting(SettingNames.IsDanmakuBold, true);
            IsDanmakuLimit = _settingsToolkit.ReadLocalSetting(SettingNames.IsDanmakuLimit, false);
            UseCloudShieldSettings = _settingsToolkit.ReadLocalSetting(SettingNames.UseCloudShieldSettings, true);
            Location = _settingsToolkit.ReadLocalSetting(SettingNames.DanmakuLocation, Models.Enums.App.DanmakuLocation.Scroll);

            IsStandardSize = _settingsToolkit.ReadLocalSetting(SettingNames.IsDanmakuStandardSize, true);
            PropertyChanged += OnPropertyChanged;

            FontCollection.Clear();
            var fontList = _fontToolkit.GetSystemFonts();
            fontList.ForEach(p => FontCollection.Add(p));

            LocationCollection.Add(Models.Enums.App.DanmakuLocation.Scroll);
            LocationCollection.Add(Models.Enums.App.DanmakuLocation.Top);
            LocationCollection.Add(Models.Enums.App.DanmakuLocation.Bottom);

            ColorCollection.Add(new KeyValue<string>(_resourceToolkit.GetLocaleString(LanguageNames.White), "#FFFFFF"));
            ColorCollection.Add(new KeyValue<string>(_resourceToolkit.GetLocaleString(LanguageNames.Red), "#FE0302"));
            ColorCollection.Add(new KeyValue<string>(_resourceToolkit.GetLocaleString(LanguageNames.Orange), "#FFAA02"));
            ColorCollection.Add(new KeyValue<string>(_resourceToolkit.GetLocaleString(LanguageNames.Khaki), "#FFD302"));
            ColorCollection.Add(new KeyValue<string>(_resourceToolkit.GetLocaleString(LanguageNames.Yellow), "#FFFF00"));
            ColorCollection.Add(new KeyValue<string>(_resourceToolkit.GetLocaleString(LanguageNames.Grass), "#A0EE00"));
            ColorCollection.Add(new KeyValue<string>(_resourceToolkit.GetLocaleString(LanguageNames.Green), "#00CD00"));
            ColorCollection.Add(new KeyValue<string>(_resourceToolkit.GetLocaleString(LanguageNames.Blue), "#019899"));
            ColorCollection.Add(new KeyValue<string>(_resourceToolkit.GetLocaleString(LanguageNames.Purple), "#4266BE"));
            ColorCollection.Add(new KeyValue<string>(_resourceToolkit.GetLocaleString(LanguageNames.LightBlue), "#89D5FF"));

            Color = _settingsToolkit.ReadLocalSetting(SettingNames.DanmakuColor, ColorCollection.First().Value);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IsShowDanmaku):
                    _settingsToolkit.WriteLocalSetting(SettingNames.IsShowDanmaku, IsShowDanmaku);
                    break;
                case nameof(DanmakuOpacity):
                    _settingsToolkit.WriteLocalSetting(SettingNames.DanmakuOpacity, DanmakuOpacity);
                    break;
                case nameof(DanmakuFontSize):
                    _settingsToolkit.WriteLocalSetting(SettingNames.DanmakuFontSize, DanmakuFontSize);
                    break;
                case nameof(DanmakuArea):
                    _settingsToolkit.WriteLocalSetting(SettingNames.DanmakuArea, DanmakuArea);
                    break;
                case nameof(DanmakuSpeed):
                    _settingsToolkit.WriteLocalSetting(SettingNames.DanmakuSpeed, DanmakuSpeed);
                    break;
                case nameof(DanmakuFont):
                    _settingsToolkit.WriteLocalSetting(SettingNames.DanmakuFont, DanmakuFont);
                    break;
                case nameof(IsDanmakuMerge):
                    _settingsToolkit.WriteLocalSetting(SettingNames.IsDanmakuMerge, IsDanmakuMerge);
                    break;
                case nameof(IsDanmakuLimit):
                    _settingsToolkit.WriteLocalSetting(SettingNames.IsDanmakuLimit, IsDanmakuLimit);
                    break;
                case nameof(IsDanmakuBold):
                    _settingsToolkit.WriteLocalSetting(SettingNames.IsDanmakuBold, IsDanmakuBold);
                    break;
                case nameof(UseCloudShieldSettings):
                    _settingsToolkit.WriteLocalSetting(SettingNames.UseCloudShieldSettings, UseCloudShieldSettings);
                    break;
                case nameof(IsStandardSize):
                    _settingsToolkit.WriteLocalSetting(SettingNames.IsDanmakuStandardSize, IsStandardSize);
                    break;
                case nameof(Location):
                    _settingsToolkit.WriteLocalSetting(SettingNames.DanmakuLocation, Location);
                    break;
                case nameof(Color):
                    _settingsToolkit.WriteLocalSetting(SettingNames.DanmakuColor, Color);
                    break;
                default:
                    break;
            }
        }
    }
}

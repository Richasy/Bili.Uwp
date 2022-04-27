// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Bilibili.Community.Service.Dm.V1;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp.Common
{
    /// <summary>
    /// 弹幕视图模型.
    /// </summary>
    public partial class DanmakuViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DanmakuViewModel"/> class.
        /// </summary>
        internal DanmakuViewModel()
        {
            ServiceLocator.Instance.LoadService(out _settingsToolkit)
                                   .LoadService(out _fontToolkit)
                                   .LoadService(out _resourceToolkit);
            Initialize();
        }

        /// <summary>
        /// 弹幕列表已添加.
        /// </summary>
        public event EventHandler<List<DanmakuElem>> DanmakuListAdded;

        /// <summary>
        /// 已成功发送弹幕.
        /// </summary>
        public event EventHandler<string> SendDanmakuSucceeded;

        /// <summary>
        /// 请求清除弹幕列表.
        /// </summary>
        public event EventHandler RequestClearDanmaku;

        /// <summary>
        /// 加载弹幕.
        /// </summary>
        /// <param name="videoId">视频Id.</param>
        /// <param name="partId">分P Id.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task LoadAsync(long videoId, long partId)
        {
            _videoId = videoId;
            _partId = partId;
            _isRequestingDanmaku = false;
            Reset();

            try
            {
                var danmakuMeta = await Controller.GetDanmakuMetaDataAsync(_videoId, _partId);
                DanmakuConfig = danmakuMeta;
                await Controller.RequestNewSegmentDanmakuAsync(_videoId, _partId, 1);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 重置.
        /// </summary>
        public void Reset()
            => RequestClearDanmaku?.Invoke(this, EventArgs.Empty);

        /// <summary>
        /// 发送弹幕.
        /// </summary>
        /// <param name="danmakuText">弹幕文本.</param>
        /// <returns>发送结果.</returns>
        public async Task<bool> SendDanmakuAsync(string danmakuText)
        {
            var result = false;
            var playerVM = PlayerViewModel.Instance;
            if (playerVM.IsLive)
            {
                result = await Controller.SendLiveDanmakuAsync(Convert.ToInt32(playerVM.RoomId), danmakuText, ToDanmakuColor(Color), IsStandardSize, Location);
            }
            else
            {
                var videoId = 0;
                var partId = 0;
                if (playerVM.IsPgc)
                {
                    videoId = playerVM.CurrentPgcEpisode.Aid;
                    partId = playerVM.CurrentPgcEpisode.PartId;
                }
                else
                {
                    videoId = Convert.ToInt32(playerVM.AvId);
                    partId = Convert.ToInt32(playerVM.CurrentVideoPart.Page.Cid);
                }

                var player = playerVM.BiliPlayer.MediaPlayer;
                if (player != null && player.PlaybackSession != null)
                {
                    result = await Controller.SendDanmakuAsync(danmakuText, videoId, partId, player.PlaybackSession.Position, ToDanmakuColor(Color), IsStandardSize, Location);
                }

                if (result)
                {
                    SendDanmakuSucceeded?.Invoke(this, danmakuText);
                }
            }

            return result;
        }

        private void Initialize()
        {
            FontCollection = new ObservableCollection<string>();
            LocationCollection = new ObservableCollection<Models.Enums.App.DanmakuLocation>();
            ColorCollection = new ObservableCollection<KeyValue<string>>();

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

            IsStandardSize = _settingsToolkit.ReadLocalSetting(SettingNames.IsDanmakuStandardSize, true);

            Controller.SegmentDanmakuIteration += OnSegmentDanmakuIteration;
            PropertyChanged += OnPropertyChanged;

            FontCollection.Clear();
            var fontList = _fontToolkit.GetSystemFontList();
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

            Location = _settingsToolkit.ReadLocalSetting(SettingNames.DanmakuLocation, Models.Enums.App.DanmakuLocation.Scroll);
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

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Bilibili.Community.Service.Dm.V1;
using Richasy.Bili.Locator.Uwp;
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
                                   .LoadService(out _fontToolkit);
            Initialize();
        }

        /// <summary>
        /// 弹幕列表已添加.
        /// </summary>
        public event EventHandler<List<DanmakuElem>> DanmakuListAdded;

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
            _danmakuList.Clear();
            RequestClearDanmaku?.Invoke(this, EventArgs.Empty);

            if (UseCloudShieldSettings)
            {
                try
                {
                    var danmakuMeta = await Controller.GetDanmakuMetaDataAsync(_videoId, _partId);
                    DanmakuConfig = danmakuMeta;
                }
                catch (Exception)
                {
                }
            }

            try
            {
                await Controller.RequestNewSegmentDanmakuAsync(_videoId, _partId, 1);
            }
            catch (Exception)
            {
            }
        }

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
                result = await Controller.SendLiveDanmakuAsync(Convert.ToInt32(playerVM.RoomId), danmakuText);
            }

            return result;
        }

        private void Initialize()
        {
            _danmakuList = new List<DanmakuElem>();
            FontCollection = new ObservableCollection<string>();
            StyleCollection = new ObservableCollection<Models.Enums.App.DanmakuStyle>();

            IsShowDanmaku = _settingsToolkit.ReadLocalSetting(SettingNames.IsShowDanmaku, true);
            DanmakuOpacity = _settingsToolkit.ReadLocalSetting(SettingNames.DanmakuOpacity, 0.8);
            DanmakuZoom = _settingsToolkit.ReadLocalSetting(SettingNames.DanmakuZoom, 1d);
            DanmakuDensity = _settingsToolkit.ReadLocalSetting(SettingNames.DanmakuDensity, 400d);
            DanmakuFont = _settingsToolkit.ReadLocalSetting(SettingNames.DanmakuFont, "Segoe UI");
            IsDanmakuMerge = _settingsToolkit.ReadLocalSetting(SettingNames.IsDanmakuMerge, false);
            IsDanmakuBold = _settingsToolkit.ReadLocalSetting(SettingNames.IsDanmakuBold, true);
            UseCloudShieldSettings = _settingsToolkit.ReadLocalSetting(SettingNames.UseCloudShieldSettings, true);

            Controller.SegmentDanmakuIteration += OnSegmentDanmakuIteration;
            PropertyChanged += OnPropertyChanged;

            FontCollection.Clear();
            var fontList = _fontToolkit.GetSystemFontList();
            fontList.ForEach(p => FontCollection.Add(p));

            StyleCollection.Add(Models.Enums.App.DanmakuStyle.Stroke);
            StyleCollection.Add(Models.Enums.App.DanmakuStyle.Shadow);
            StyleCollection.Add(Models.Enums.App.DanmakuStyle.NoStroke);
            DanmakuStyle = _settingsToolkit.ReadLocalSetting(SettingNames.DanmakuStyle, Models.Enums.App.DanmakuStyle.Stroke);
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
                case nameof(DanmakuZoom):
                    _settingsToolkit.WriteLocalSetting(SettingNames.DanmakuZoom, DanmakuZoom);
                    break;
                case nameof(DanmakuDensity):
                    _settingsToolkit.WriteLocalSetting(SettingNames.DanmakuDensity, DanmakuDensity);
                    break;
                case nameof(DanmakuFont):
                    _settingsToolkit.WriteLocalSetting(SettingNames.DanmakuFont, DanmakuFont);
                    break;
                case nameof(IsDanmakuMerge):
                    _settingsToolkit.WriteLocalSetting(SettingNames.IsDanmakuMerge, IsDanmakuMerge);
                    break;
                case nameof(IsDanmakuBold):
                    _settingsToolkit.WriteLocalSetting(SettingNames.IsDanmakuMerge, IsDanmakuBold);
                    break;
                case nameof(UseCloudShieldSettings):
                    _settingsToolkit.WriteLocalSetting(SettingNames.UseCloudShieldSettings, UseCloudShieldSettings);
                    break;
                case nameof(DanmakuStyle):
                    _settingsToolkit.WriteLocalSetting(SettingNames.DanmakuStyle, DanmakuStyle);
                    break;
                default:
                    break;
            }
        }
    }
}

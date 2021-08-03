// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
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
        public DanmakuViewModel()
        {
            ServiceLocator.Instance.LoadService(out _settingsToolkit);
            _danmakuList = new List<DanmakuElem>();
            IsShowDanmaku = _settingsToolkit.ReadLocalSetting(SettingNames.IsShowDanmaku, true);
            Controller.SegmentDanmakuIteration += OnSegmentDanmakuIteration;
            PropertyChanged += OnPropertyChanged;
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

            var danmakuMeta = await Controller.GetDanmakuMetaDataAsync(_videoId, _partId);
            await Controller.RequestNewSegmentDanmakuAsync(_videoId, _partId, 1);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(IsShowDanmaku):
                    _settingsToolkit.WriteLocalSetting(SettingNames.IsShowDanmaku, IsShowDanmaku);
                    break;
                default:
                    break;
            }
        }
    }
}

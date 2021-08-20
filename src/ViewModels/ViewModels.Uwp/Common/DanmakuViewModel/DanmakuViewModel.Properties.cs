// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bilibili.Community.Service.Dm.V1;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Models.Enums.App;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.ViewModels.Uwp.Common
{
    /// <summary>
    /// 弹幕视图模型.
    /// </summary>
    public partial class DanmakuViewModel
    {
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly IFontToolkit _fontToolkit;

        private List<DanmakuElem> _danmakuList;
        private long _videoId;
        private long _partId;

        /// <summary>
        /// 弹幕视图模型单例.
        /// </summary>
        public static DanmakuViewModel Instance { get; } = new Lazy<DanmakuViewModel>(() => new DanmakuViewModel()).Value;

        /// <summary>
        /// 弹幕配置文件.
        /// </summary>
        public DmViewReply DanmakuConfig { get; set; }

        /// <summary>
        /// 是否显示弹幕.
        /// </summary>
        [Reactive]
        public bool IsShowDanmaku { get; set; }

        /// <summary>
        /// 弹幕透明度.
        /// </summary>
        [Reactive]
        public double DanmakuOpacity { get; set; }

        /// <summary>
        /// 弹幕缩放大小.
        /// </summary>
        [Reactive]
        public double DanmakuZoom { get; set; }

        /// <summary>
        /// 弹幕密度（同屏弹幕数量）.
        /// </summary>
        [Reactive]
        public double DanmakuDensity { get; set; }

        /// <summary>
        /// 弹幕字体.
        /// </summary>
        [Reactive]
        public string DanmakuFont { get; set; }

        /// <summary>
        /// 弹幕样式.
        /// </summary>
        [Reactive]
        public DanmakuStyle DanmakuStyle { get; set; }

        /// <summary>
        /// 系统字体集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<string> FontCollection { get; set; }

        /// <summary>
        /// 样式集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<DanmakuStyle> StyleCollection { get; set; }

        /// <summary>
        /// 是否启用弹幕合并.
        /// </summary>
        [Reactive]
        public bool IsDanmakuMerge { get; set; }

        /// <summary>
        /// 是否启用云屏蔽设置.
        /// </summary>
        [Reactive]
        public bool UseCloudShieldSettings { get; set; }

        private BiliController Controller { get; } = BiliController.Instance;
    }
}

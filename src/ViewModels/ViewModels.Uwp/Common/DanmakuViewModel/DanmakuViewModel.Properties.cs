// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bilibili.Community.Service.Dm.V1;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Models.App.Other;
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
        private readonly IResourceToolkit _resourceToolkit;

        private long _videoId;
        private long _partId;
        private bool _isRequestingDanmaku;

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
        /// 是否可以显示弹幕.
        /// </summary>
        [Reactive]
        public bool CanShowDanmaku { get; set; }

        /// <summary>
        /// 弹幕透明度.
        /// </summary>
        [Reactive]
        public double DanmakuOpacity { get; set; }

        /// <summary>
        /// 弹幕文本大小.
        /// </summary>
        [Reactive]
        public double DanmakuFontSize { get; set; }

        /// <summary>
        /// 弹幕显示区域.
        /// </summary>
        [Reactive]
        public double DanmakuArea { get; set; }

        /// <summary>
        /// 弹幕速度.
        /// </summary>
        [Reactive]
        public double DanmakuSpeed { get; set; }

        /// <summary>
        /// 弹幕字体.
        /// </summary>
        [Reactive]
        public string DanmakuFont { get; set; }

        /// <summary>
        /// 系统字体集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<string> FontCollection { get; set; }

        /// <summary>
        /// 是否启用同屏弹幕限制.
        /// </summary>
        [Reactive]
        public bool IsDanmakuLimit { get; set; }

        /// <summary>
        /// 是否启用弹幕合并.
        /// </summary>
        [Reactive]
        public bool IsDanmakuMerge { get; set; }

        /// <summary>
        /// 是否加粗弹幕.
        /// </summary>
        [Reactive]
        public bool IsDanmakuBold { get; set; }

        /// <summary>
        /// 是否启用云屏蔽设置.
        /// </summary>
        [Reactive]
        public bool UseCloudShieldSettings { get; set; }

        /// <summary>
        /// 是否为标准字号.
        /// </summary>
        [Reactive]
        public bool IsStandardSize { get; set; }

        /// <summary>
        /// 弹幕位置.
        /// </summary>
        [Reactive]
        public DanmakuLocation Location { get; set; }

        /// <summary>
        /// 弹幕位置可选集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<DanmakuLocation> LocationCollection { get; set; }

        /// <summary>
        /// 弹幕颜色.
        /// </summary>
        [Reactive]
        public string Color { get; set; }

        /// <summary>
        /// 弹幕颜色集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<KeyValue<string>> ColorCollection { get; set; }

        private BiliController Controller { get; } = BiliController.Instance;
    }
}

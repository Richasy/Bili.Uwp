// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Bilibili.Community.Service.Dm.V1;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.ViewModels.Uwp.Common
{
    /// <summary>
    /// 弹幕视图模型.
    /// </summary>
    public partial class DanmakuViewModel
    {
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly List<DanmakuElem> _danmakuList;

        private long _videoId;
        private long _partId;

        /// <summary>
        /// 弹幕视图模型单例.
        /// </summary>
        public static DanmakuViewModel Instance { get; } = new Lazy<DanmakuViewModel>(() => new DanmakuViewModel()).Value;

        /// <summary>
        /// 是否显示弹幕.
        /// </summary>
        [Reactive]
        public bool IsShowDanmaku { get; set; }

        private BiliController Controller { get; } = BiliController.Instance;
    }
}

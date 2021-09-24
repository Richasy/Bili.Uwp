// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.System.Display;
using Windows.UI.Xaml;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// <see cref="AppViewModel"/>的属性集.
    /// </summary>
    public partial class AppViewModel
    {
        private readonly IResourceToolkit _resourceToolkit;
        private readonly ISettingsToolkit _settingToolkit;
        private readonly DisplayRequest _displayRequest;

        /// <summary>
        /// 请求导航至二级页面时发生
        /// </summary>
        public event EventHandler<object> RequestOverlayNavigation;

        /// <summary>
        /// 请求播放视频.
        /// </summary>
        public event EventHandler<object> RequestPlay;

        /// <summary>
        /// 请求显示提醒.
        /// </summary>
        public event EventHandler<AppTipNotificationEventArgs> RequestShowTip;

        /// <summary>
        /// <see cref="AppViewModel"/>的单例.
        /// </summary>
        public static AppViewModel Instance { get; } = new Lazy<AppViewModel>(() => new AppViewModel()).Value;

        /// <summary>
        /// 当前主视图中的页面标识.
        /// </summary>
        [Reactive]
        public PageIds CurrentMainContentId { get; internal set; }

        /// <summary>
        /// 当前覆盖视图中的页面标识.
        /// </summary>
        [Reactive]
        public PageIds CurrentOverlayContentId { get; internal set; }

        /// <summary>
        /// 是否显示覆盖视图.
        /// </summary>
        [Reactive]
        public bool IsShowOverlay { get; internal set; }

        /// <summary>
        /// 导航面板是否已展开.
        /// </summary>
        [Reactive]
        public bool IsNavigatePaneOpen { get; set; }

        /// <summary>
        /// 页面标题文本.
        /// </summary>
        [Reactive]
        public string HeaderText { get; set; }

        /// <summary>
        /// 是否开启播放器.
        /// </summary>
        [Reactive]
        public bool IsOpenPlayer { get; set; }

        /// <summary>
        /// 是否启用回退按钮.
        /// </summary>
        [Reactive]
        public bool IsBackButtonEnabled { get; set; }

        /// <summary>
        /// 覆盖层是否扩展至TitleBar.
        /// </summary>
        [Reactive]
        public bool IsOverLayerExtendToTitleBar { get; set; }

        /// <summary>
        /// 主题.
        /// </summary>
        [Reactive]
        public ElementTheme Theme { get; set; }
    }
}

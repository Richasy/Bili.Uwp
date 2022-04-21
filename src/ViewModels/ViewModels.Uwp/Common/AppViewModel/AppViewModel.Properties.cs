// Copyright (c) Richasy. All rights reserved.

using System;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Controller.Uwp.Interfaces;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.System.Display;
using Windows.UI.Core;
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
        private readonly ILoggerModule _loggerModule;
        private readonly DisplayRequest _displayRequest;
        private readonly BiliController _controller;

        private bool? _isWide;

        /// <summary>
        /// 请求导航至二级页面时发生
        /// </summary>
        public event EventHandler<object> RequestOverlayNavigation;

        /// <summary>
        /// 请求播放视频.
        /// </summary>
        public event EventHandler<object> RequestPlay;

        /// <summary>
        /// 请求返回.
        /// </summary>
        public event EventHandler RequestBack;

        /// <summary>
        /// 请求显示提醒.
        /// </summary>
        public event EventHandler<AppTipNotificationEventArgs> RequestShowTip;

        /// <summary>
        /// 请求显示升级提示.
        /// </summary>
        public event EventHandler<UpdateEventArgs> RequestShowUpdateDialog;

        /// <summary>
        /// 请求进行之前的播放.
        /// </summary>
        public event EventHandler RequestContinuePlay;

        /// <summary>
        /// 请求显示图片列表.
        /// </summary>
        public event EventHandler<ShowImageEventArgs> RequestShowImages;

        /// <summary>
        /// <see cref="AppViewModel"/>的单例.
        /// </summary>
        public static AppViewModel Instance { get; } = new Lazy<AppViewModel>(() => new AppViewModel()).Value;

        /// <summary>
        /// UI调度器.
        /// </summary>
        public CoreDispatcher Dispatcher { get; set; }

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
        /// 是否可以显示返回首页按钮.
        /// </summary>
        [Reactive]
        public bool CanShowHomeButton { get; set; }

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

        /// <summary>
        /// 是否在Xbox上运行.
        /// </summary>
        [Reactive]
        public bool IsXbox { get; set; }

        /// <summary>
        /// 页面左侧或上部的边距.
        /// </summary>
        [Reactive]
        public Thickness PageLeftPadding { get; set; }

        /// <summary>
        /// 页面右侧或下部的边距.
        /// </summary>
        [Reactive]
        public Thickness PageRightPadding { get; set; }
    }
}

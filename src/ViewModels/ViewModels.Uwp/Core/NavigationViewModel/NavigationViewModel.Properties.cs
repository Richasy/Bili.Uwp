// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reactive;
using Bili.Models.App.Args;
using Bili.Models.Enums;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 导航视图模型.
    /// </summary>
    public sealed partial class NavigationViewModel
    {
        private readonly IRecordViewModel _recordViewModel;
        private readonly List<AppBackEventArgs> _backStack;

        /// <summary>
        /// 当传入新的导航请求时触发.
        /// </summary>
        public event EventHandler<AppNavigationEventArgs> Navigating;

        /// <summary>
        /// 当退出播放界面时触发.
        /// </summary>
        public event EventHandler ExitPlayer;

        /// <summary>
        /// 返回上一级页面的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> BackCommand { get; }

        /// <summary>
        /// 是否显示主视图.
        /// </summary>
        [Reactive]
        public bool IsMainViewShown { get; set; }

        /// <summary>
        /// 是否显示二级页面.
        /// </summary>
        [Reactive]
        public bool IsSecondaryViewShown { get; set; }

        /// <summary>
        /// 是否显示播放页面.
        /// </summary>
        [Reactive]
        public bool IsPlayViewShown { get; set; }

        /// <summary>
        /// 是否可以返回.
        /// </summary>
        [Reactive]
        public bool CanBack { get; set; }

        /// <summary>
        /// 是否显示后退按钮.
        /// </summary>
        [Reactive]
        public bool IsBackButtonEnabled { get; set; }

        /// <summary>
        /// 当前主视图展示的页面 Id.
        /// </summary>
        public PageIds MainViewId { get; private set; }

        /// <summary>
        /// 当前二级页面展示的页面 Id.
        /// </summary>
        public PageIds SecondaryViewId { get; private set; }

        /// <summary>
        /// 播放页面的页面 Id.
        /// </summary>
        public PageIds PlayViewId { get; private set; }
    }
}

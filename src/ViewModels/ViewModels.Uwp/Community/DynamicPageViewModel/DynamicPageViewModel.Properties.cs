// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Toolkit.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 动态页面视图模型.
    /// </summary>
    public sealed partial class DynamicPageViewModel
    {
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IAuthorizeProvider _authorizeProvider;

        /// <summary>
        /// 分区集合.
        /// </summary>
        public ObservableCollection<DynamicHeader> Headers { get; }

        /// <summary>
        /// 视频模块.
        /// </summary>
        public DynamicVideoModuleViewModel VideoModule { get; }

        /// <summary>
        /// 综合模块.
        /// </summary>
        public DynamicAllModuleViewModel AllModule { get; }

        /// <summary>
        /// 刷新当前模块命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> RefreshModuleCommand { get; }

        /// <summary>
        /// 选中分区命令.
        /// </summary>
        public ReactiveCommand<DynamicHeader, Unit> SelectHeaderCommand { get; }

        /// <summary>
        /// 当前分区.
        /// </summary>
        [Reactive]
        public DynamicHeader CurrentHeader { get; set; }

        /// <summary>
        /// 是否显示视频动态.
        /// </summary>
        [Reactive]
        public bool IsShowVideo { get; set; }

        /// <summary>
        /// 是否显示全部动态.
        /// </summary>
        [Reactive]
        public bool IsShowAll { get; set; }

        /// <summary>
        /// 是否需要用户登录.
        /// </summary>
        [Reactive]
        public bool NeedSignIn { get; set; }
    }
}

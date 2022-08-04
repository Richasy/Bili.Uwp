// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Models.App.Other;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces.Community
{
    /// <summary>
    /// 动态页面视图模型的接口定义.
    /// </summary>
    public interface IDynamicPageViewModel : IReactiveObject
    {
        /// <summary>
        /// 刷新当前模块命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> RefreshModuleCommand { get; }

        /// <summary>
        /// 选中分区命令.
        /// </summary>
        ReactiveCommand<DynamicHeader, Unit> SelectHeaderCommand { get; }

        /// <summary>
        /// 分区集合.
        /// </summary>
        ObservableCollection<DynamicHeader> Headers { get; }

        /// <summary>
        /// 视频模块.
        /// </summary>
        IDynamicVideoModuleViewModel VideoModule { get; }

        /// <summary>
        /// 综合模块.
        /// </summary>
        IDynamicAllModuleViewModel AllModule { get; }

        /// <summary>
        /// 当前分区.
        /// </summary>
        DynamicHeader CurrentHeader { get; }

        /// <summary>
        /// 是否显示视频动态.
        /// </summary>
        bool IsShowVideo { get; }

        /// <summary>
        /// 是否显示全部动态.
        /// </summary>
        bool IsShowAll { get; }

        /// <summary>
        /// 是否需要用户登录.
        /// </summary>
        bool NeedSignIn { get; }
    }
}

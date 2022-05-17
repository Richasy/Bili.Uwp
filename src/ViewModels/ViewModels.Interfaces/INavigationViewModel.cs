// Copyright (c) Richasy. All rights reserved.

using System;
using System.Reactive;
using Bili.Models.App.Args;
using Bili.Models.Enums;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces
{
    /// <summary>
    /// 导航视图模型的接口定义.
    /// </summary>
    public interface INavigationViewModel
    {
        /// <summary>
        /// 当传入新的导航请求时触发.
        /// </summary>
        event EventHandler<AppNavigationEventArgs> Navigating;

        /// <summary>
        /// 当应用进行回退时触发.
        /// </summary>
        event EventHandler<AppBackEventArgs> Backing;

        /// <summary>
        /// 返回上一级页面的命令.
        /// </summary>
        ReactiveCommand<object, Unit> BackCommand { get; }

        /// <summary>
        /// 是否可以返回.
        /// </summary>
        bool CanBack { get; }

        /// <summary>
        /// 在主视图中进行导航，传入的 PageIds 应该是主视图的页面 Id.
        /// </summary>
        /// <param name="pageId">页面 Id.</param>
        /// <param name="parameter">导航参数.</param>
        void NavigateToMainView(PageIds pageId, object parameter);

        /// <summary>
        /// 导航到指定的二级页面，传入的 PageIds 应该是二级页面的页面 Id.
        /// </summary>
        /// <param name="pageId">页面 Id.</param>
        /// <param name="parameter">导航参数.</param>
        void NavigateToSecondaryView(PageIds pageId, object parameter);

        /// <summary>
        /// 导航到播放页，传入播放参数.
        /// </summary>
        /// <param name="parameter">播放参数.</param>
        void NavigateToPlayView(object parameter);

        /// <summary>
        /// 传入页面 Id, 并由应用自行判断层级，导航到指定类型的页面.
        /// </summary>
        /// <param name="pageId">页面 Id.</param>
        /// <param name="parameter">导航参数.</param>
        void Navigate(PageIds pageId, object parameter);
    }
}

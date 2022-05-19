// Copyright (c) Richasy. All rights reserved.

using System;
using System.Reactive;
using Bili.Models.App.Args;
using Bili.Models.Enums;
using Bili.Models.Enums.App;
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
        /// 返回上一级页面的命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> BackCommand { get; }

        /// <summary>
        /// 是否可以返回.
        /// </summary>
        bool CanBack { get; }

        /// <summary>
        /// 是否显示主视图.
        /// </summary>
        bool IsMainViewShown { get; }

        /// <summary>
        /// 是否显示二级页面.
        /// </summary>
        bool IsSecondaryViewShown { get; }

        /// <summary>
        /// 是否显示播放页面.
        /// </summary>
        bool IsPlayViewShown { get; }

        /// <summary>
        /// 当前主视图展示的页面 Id.
        /// </summary>
        PageIds MainViewId { get; }

        /// <summary>
        /// 当前二级页面展示的页面 Id.
        /// </summary>
        PageIds SecondaryViewId { get; }

        /// <summary>
        /// 在主视图中进行导航，传入的 PageIds 应该是主视图的页面 Id.
        /// </summary>
        /// <param name="pageId">页面 Id.</param>
        /// <param name="parameter">导航参数.</param>
        void NavigateToMainView(PageIds pageId, object parameter = null);

        /// <summary>
        /// 导航到指定的二级页面，传入的 PageIds 应该是二级页面的页面 Id.
        /// </summary>
        /// <param name="pageId">页面 Id.</param>
        /// <param name="parameter">导航参数.</param>
        void NavigateToSecondaryView(PageIds pageId, object parameter = null);

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
        void Navigate(PageIds pageId, object parameter = null);

        /// <summary>
        /// 添加后退栈，应用会按照先进后出的顺序依次执行回退行为.
        /// </summary>
        /// <param name="id">行为标识.</param>
        /// <param name="backBehavior">自定义的后退行为.</param>
        /// <param name="parameter">后退时携带的参数.</param>
        /// <remarks>
        /// 这里的导航服务的主要职能是暂存和转发。应用各个组件定义自己的后退行为，按照顺序依次进入这个栈中，
        /// 导航服务不必知道 <see cref="Action"/> 是做什么的，只负责在需要后退时按顺序执行这些组件自定义的行为即可.
        /// 举例来说，当应用进入二级页面，由组件发送一个回退行为给导航服务，当应用点击标题栏的后退按钮时，
        /// 服务检测到最后一个回退行为是二级页面定义的，那么就执行二级页面预设的 <see cref="Action"/>.
        /// </remarks>
        void AddBackStack(BackBehavior id, Action<object> backBehavior, object parameter = null);

        /// <summary>
        /// 移除已有的后退栈.
        /// </summary>
        /// <param name="id">行为标识.</param>
        void RemoveBackStack(BackBehavior id);
    }
}

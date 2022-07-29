// Copyright (c) Richasy. All rights reserved.

using System;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces.Core
{
    /// <summary>
    /// 应用视图模型的接口定义.
    /// </summary>
    public interface IAppViewModel : IReactiveObject
    {
        /// <summary>
        /// 检查应用更新命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> CheckUpdateCommand { get; }

        /// <summary>
        /// 检查新动态通知的命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> CheckNewDynamicRegistrationCommand { get; }

        /// <summary>
        /// 导航面板是否已展开.
        /// </summary>
        bool IsNavigatePaneOpen { get; set; }

        /// <summary>
        /// 页面标题文本.
        /// </summary>
        string HeaderText { get; set; }

        /// <summary>
        /// 是否在Xbox上运行.
        /// </summary>
        bool IsXbox { get; }

        /// <summary>
        /// 页面横向边距.
        /// </summary>
        double PageHorizontalPadding { get; }

        /// <summary>
        /// 页面顶部边距.
        /// </summary>
        double PageTopPadding { get; }

        /// <summary>
        /// 是否显示标题栏.
        /// </summary>
        bool IsShowTitleBar { get; set; }

        /// <summary>
        /// 是否显示菜单按钮.
        /// </summary>
        bool IsShowMenuButton { get; set; }

        /// <summary>
        /// 网络是否可用.
        /// </summary>
        bool IsNetworkAvaliable { get; }

        /// <summary>
        /// 是否为繁体中文环境.
        /// </summary>
        bool IsTraditionalChinese { get; }

        /// <summary>
        /// 初始化边距设置.
        /// </summary>
        void InitializePadding();

        /// <summary>
        /// 从命令行参数初始化命令.
        /// </summary>
        /// <param name="arguments">命令行参数.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task InitializeCommandFromArgumentsAsync(string arguments);

        /// <summary>
        /// 从参数初始化协议调用.
        /// </summary>
        /// <param name="link">协议调用链接.</param>
        /// <returns><see cref="Task"/>.</returns>
        Task InitializeProtocolFromQueryAsync(Uri link);
    }
}

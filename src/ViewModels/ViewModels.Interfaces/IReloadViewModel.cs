﻿// Copyright (c) Richasy. All rights reserved.

using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces
{
    /// <summary>
    /// 支持刷新重载的视图模型.
    /// </summary>
    public interface IReloadViewModel
    {
        /// <summary>
        /// 是否正在重新载入.
        /// </summary>
        bool IsReloading { get; }

        /// <summary>
        /// 重新加载命令.
        /// </summary>
        IAsyncRelayCommand ReloadCommand { get; }
    }
}

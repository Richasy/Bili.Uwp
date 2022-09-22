// Copyright (c) Richasy. All rights reserved.

using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces
{
    /// <summary>
    /// 支持初始化的视图模型.
    /// </summary>
    public interface IInitializeViewModel
    {
        /// <summary>
        /// 初始化命令.
        /// </summary>
        IRelayCommand InitializeCommand { get; }
    }
}

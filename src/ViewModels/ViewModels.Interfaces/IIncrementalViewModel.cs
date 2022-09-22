// Copyright (c) Richasy. All rights reserved.

using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces
{
    /// <summary>
    /// 支持增量加载的视图模型.
    /// </summary>
    public interface IIncrementalViewModel
    {
        /// <summary>
        /// 是否正在增量加载.
        /// </summary>
        bool IsIncrementalLoading { get; }

        /// <summary>
        /// 增量加载命令.
        /// </summary>
        IRelayCommand IncrementalCommand { get; }
    }
}

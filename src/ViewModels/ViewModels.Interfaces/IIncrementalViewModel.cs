// Copyright (c) Richasy. All rights reserved.

using System.Reactive;
using ReactiveUI;

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
        ReactiveCommand<Unit, Unit> IncrementalCommand { get; }
    }
}

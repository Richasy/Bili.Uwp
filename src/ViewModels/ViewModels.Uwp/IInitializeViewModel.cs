// Copyright (c) Richasy. All rights reserved.

using System.Reactive;
using ReactiveUI;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 支持初始化的视图模型.
    /// </summary>
    public interface IInitializeViewModel
    {
        /// <summary>
        /// 初始化命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> InitializeCommand { get; }
    }
}

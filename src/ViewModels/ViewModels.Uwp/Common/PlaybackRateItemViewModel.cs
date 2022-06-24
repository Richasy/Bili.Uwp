// Copyright (c) Richasy. All rights reserved.

using System;
using System.Reactive;
using ReactiveUI;

namespace Bili.ViewModels.Uwp.Common
{
    /// <summary>
    /// 播放速率条目视图模型.
    /// </summary>
    public sealed class PlaybackRateItemViewModel : SelectableViewModelBase<double>
    {
        private readonly Action<double> _action;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaybackRateItemViewModel"/> class.
        /// </summary>
        /// <param name="rate">播放速率.</param>
        /// <param name="isSelected">是否选中.</param>
        /// <param name="action">点击执行动作.</param>
        public PlaybackRateItemViewModel(double rate, bool isSelected, Action<double> action)
            : base(rate, isSelected)
        {
            _action = action;
            ActiveCommand = ReactiveCommand.Create(() => { _action.Invoke(Data); }, outputScheduler: RxApp.MainThreadScheduler);
        }

        /// <summary>
        /// 执行命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ActiveCommand { get; }
    }
}

// Copyright (c) Richasy. All rights reserved.

using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Common
{
    /// <summary>
    /// 播放速率条目视图模型的接口定义.
    /// </summary>
    public interface IPlaybackRateItemViewModel : ISelectableViewModel<double>, IInjectActionViewModel<double>
    {
        /// <summary>
        /// 执行命令.
        /// </summary>
        IRelayCommand ActiveCommand { get; }
    }
}

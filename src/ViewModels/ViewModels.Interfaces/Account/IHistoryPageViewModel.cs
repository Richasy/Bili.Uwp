// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Video;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Account
{
    /// <summary>
    /// 历史记录页面的视图模型接口定义.
    /// </summary>
    public interface IHistoryPageViewModel : IInformationFlowViewModel<IVideoItemViewModel>
    {
        /// <summary>
        /// 清空全部命令.
        /// </summary>
        IAsyncRelayCommand ClearCommand { get; }

        /// <summary>
        /// 稍后再看列表是否为空.
        /// </summary>
        bool IsEmpty { get; }
    }
}

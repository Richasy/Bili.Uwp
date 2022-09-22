// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Video;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Account
{
    /// <summary>
    /// 稍后再看页面视图模型的接口定义.
    /// </summary>
    public interface IViewLaterPageViewModel : IInformationFlowViewModel<IVideoItemViewModel>
    {
        /// <summary>
        /// 播放全部命令.
        /// </summary>
        IRelayCommand PlayAllCommand { get; }

        /// <summary>
        /// 清空全部命令.
        /// </summary>
        IRelayCommand ClearCommand { get; }

        /// <summary>
        /// 稍后再看列表是否为空.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// 是否正在清空内容.
        /// </summary>
        bool IsClearing { get; }
    }
}

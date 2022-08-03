// Copyright (c) Richasy. All rights reserved.

using System.Reactive;
using Bili.ViewModels.Interfaces.Video;
using ReactiveUI;

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
        ReactiveCommand<Unit, Unit> ClearCommand { get; }

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

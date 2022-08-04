// Copyright (c) Richasy. All rights reserved.

using System.Reactive;
using Bili.Models.Data.Community;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces.Community
{
    /// <summary>
    /// 消息条目视图模型的接口定义.
    /// </summary>
    public interface IMessageItemViewModel : IInjectDataViewModel<MessageInformation>
    {
        /// <summary>
        /// 可读的发布时间.
        /// </summary>
        string PublishTime { get; }

        /// <summary>
        /// 激活命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> ActiveCommand { get; }
    }
}

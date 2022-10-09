// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Community
{
    /// <summary>
    /// 消息页面视图模型的接口定义.
    /// </summary>
    public interface IMessagePageViewModel : IInformationFlowViewModel<IMessageItemViewModel>
    {
        /// <summary>
        /// 选择消息类型命令.
        /// </summary>
        IAsyncRelayCommand<IMessageHeaderViewModel> SelectTypeCommand { get; }

        /// <summary>
        /// 消息类型集合.
        /// </summary>
        ObservableCollection<IMessageHeaderViewModel> MessageTypes { get; }

        /// <summary>
        /// 当前选中的消息类型.
        /// </summary>
        IMessageHeaderViewModel CurrentType { get; set; }

        /// <summary>
        /// 是否为空.
        /// </summary>
        bool IsEmpty { get; }
    }
}

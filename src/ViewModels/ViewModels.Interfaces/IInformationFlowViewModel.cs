// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Bili.ViewModels.Interfaces
{
    /// <summary>
    /// 信息流视图模型接口定义.
    /// </summary>
    /// <typeparam name="T">信息类型.</typeparam>
    public interface IInformationFlowViewModel<T> : INotifyPropertyChanged, IInitializeViewModel, IReloadViewModel, IIncrementalViewModel, IErrorViewModel
    {
        /// <summary>
        /// 视频集合.
        /// </summary>
        ObservableCollection<T> Items { get; }
    }
}

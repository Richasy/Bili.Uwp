// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Bili.Models.Data.Video;

namespace Bili.ViewModels.Interfaces
{
    /// <summary>
    /// 视频基类视图模型接口.
    /// </summary>
    public interface IVideoBaseViewModel : INotifyPropertyChanged
    {
    }

    /// <summary>
    /// 标识数据类型的视频基类视图模型接口.
    /// </summary>
    /// <typeparam name="T">视频类型.</typeparam>
    public interface IVideoBaseViewModel<T> : IVideoBaseViewModel, IInjectDataViewModel<T>
        where T : class, IVideoBase
    {
    }
}

// Copyright (c) Richasy. All rights reserved.

using System.Reactive;
using Bili.Models.Data.Live;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces.Live
{
    /// <summary>
    /// 直播条目视图模型的接口定义.
    /// </summary>
    public interface ILiveItemViewModel : IVideoBaseViewModel<LiveInformation>
    {
        /// <summary>
        /// 在网页中打开的命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> OpenInBroswerCommand { get; }

        /// <summary>
        /// 播放命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> PlayCommand { get; }

        /// <summary>
        /// 观看人数的可读文本.
        /// </summary>
        string ViewerCountText { get; }
    }
}

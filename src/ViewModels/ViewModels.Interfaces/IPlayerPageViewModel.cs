// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces
{
    /// <summary>
    /// 播放器页面视图模型的接口定义.
    /// </summary>
    public interface IPlayerPageViewModel : IReactiveObject
    {
        /// <summary>
        /// 媒体播放视图模型.
        /// </summary>
        IMediaPlayerViewModel MediaPlayerViewModel { get; }
    }
}

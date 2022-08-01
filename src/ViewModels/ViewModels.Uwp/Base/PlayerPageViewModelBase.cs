// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Core;

namespace Bili.ViewModels.Uwp.Base
{
    /// <summary>
    /// 播放器页面视图模型基类.
    /// </summary>
    public class PlayerPageViewModelBase : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerPageViewModelBase"/> class.
        /// </summary>
        public PlayerPageViewModelBase(
            IMediaPlayerViewModel playerViewModel)
            => MediaPlayerViewModel = playerViewModel;

        /// <summary>
        /// 媒体播放视图模型.
        /// </summary>
        public IMediaPlayerViewModel MediaPlayerViewModel { get; }
    }
}

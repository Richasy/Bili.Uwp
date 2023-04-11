// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Interfaces.Core;

namespace Bili.ViewModels.Workspace.Core
{
    /// <summary>
    /// 播放器页面视图模型基类.
    /// </summary>
    public class PlayerPageViewModelBase : ViewModelBase, IPlayerPageViewModel
    {
        /// <summary>
        /// 媒体播放视图模型.
        /// </summary>
        public IMediaPlayerViewModel MediaPlayerViewModel { get; set; }
    }
}

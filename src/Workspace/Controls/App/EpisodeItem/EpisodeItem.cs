// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Pgc;

namespace Bili.Workspace.Controls.App
{
    /// <summary>
    /// 剧集单集条目视图.
    /// </summary>
    public sealed class EpisodeItem : ReactiveControl<IEpisodeItemViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EpisodeItem"/> class.
        /// </summary>
        public EpisodeItem()
            => DefaultStyleKey = typeof(EpisodeItem);
    }
}

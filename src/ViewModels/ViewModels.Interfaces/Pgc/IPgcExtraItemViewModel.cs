// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bili.Models.Data.Pgc;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces.Pgc
{
    /// <summary>
    /// PGC 附加内容条目视图模型的接口定义.
    /// </summary>
    public interface IPgcExtraItemViewModel : IReactiveObject
    {
        /// <summary>
        /// 标题.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 分集.
        /// </summary>
        ObservableCollection<IEpisodeItemViewModel> Episodes { get; }

        /// <summary>
        /// 设置数据.
        /// </summary>
        /// <param name="title">标题.</param>
        /// <param name="episodes">分集列表.</param>
        /// <param name="currentId">当前正在播放的分集的 Id.</param>
        void SetData(string title, IEnumerable<EpisodeInformation> episodes, string currentId);
    }
}

// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bili.Models.Data.Pgc;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces.Pgc
{
    /// <summary>
    /// PGC 排行榜视图模型的接口定义.
    /// </summary>
    public interface IPgcRankViewModel : IReactiveObject
    {
        /// <summary>
        /// 标题.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 剧集列表.
        /// </summary>
        ObservableCollection<IEpisodeItemViewModel> Episodes { get; }

        /// <summary>
        /// 设置数据.
        /// </summary>
        /// <param name="title">排行榜标题.</param>
        /// <param name="episodes">下属剧集.</param>
        void SetData(string title, IEnumerable<EpisodeInformation> episodes);
    }
}

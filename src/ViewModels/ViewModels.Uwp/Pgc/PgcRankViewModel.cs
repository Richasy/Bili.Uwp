// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Bili.Models.Data.Pgc;
using Bili.ViewModels.Interfaces.Pgc;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC 排行榜视图模型.
    /// </summary>
    public sealed class PgcRankViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcRankViewModel"/> class.
        /// </summary>
        /// <param name="title">排行榜标题.</param>
        /// <param name="episodes">下属剧集.</param>
        internal PgcRankViewModel(string title, IEnumerable<EpisodeInformation> episodes)
        {
            Title = title;
            Episodes = new ObservableCollection<IEpisodeItemViewModel>();
            episodes
                .Select(p =>
                {
                    var episodeVM = Splat.Locator.Current.GetService<IEpisodeItemViewModel>();
                    episodeVM.InjectData(p);
                    return episodeVM;
                })
                .ToList()
                .ForEach(p => Episodes.Add(p));
        }

        /// <summary>
        /// 标题.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <summary>
        /// 剧集列表.
        /// </summary>
        public ObservableCollection<IEpisodeItemViewModel> Episodes { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is PgcRankViewModel model && Title == model.Title;

        /// <inheritdoc/>
        public override int GetHashCode() => Title.GetHashCode();
    }
}

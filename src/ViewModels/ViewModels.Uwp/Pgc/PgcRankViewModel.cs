// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Bili.DI.Container;
using Bili.Models.Data.Pgc;
using Bili.ViewModels.Interfaces.Pgc;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC 排行榜视图模型.
    /// </summary>
    public sealed partial class PgcRankViewModel : ViewModelBase, IPgcRankViewModel
    {
        [ObservableProperty]
        private string _title;

        /// <summary>
        /// Initializes a new instance of the <see cref="PgcRankViewModel"/> class.
        /// </summary>
        public PgcRankViewModel()
            => Episodes = new ObservableCollection<IEpisodeItemViewModel>();

        /// <inheritdoc/>
        public ObservableCollection<IEpisodeItemViewModel> Episodes { get; }

        /// <inheritdoc/>
        public void SetData(string title, IEnumerable<EpisodeInformation> episodes)
        {
            Title = title;
            episodes
                .Select(p =>
                {
                    var episodeVM = Locator.Instance.GetService<IEpisodeItemViewModel>();
                    episodeVM.InjectData(p);
                    return episodeVM;
                })
                .ToList()
                .ForEach(p => Episodes.Add(p));
        }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is PgcRankViewModel model && Title == model.Title;

        /// <inheritdoc/>
        public override int GetHashCode() => Title.GetHashCode();
    }
}

// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bili.Models.Data.Pgc;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC 附加内容条目视图模型.
    /// </summary>
    public sealed class PgcExtraItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcExtraItemViewModel"/> class.
        /// </summary>
        /// <param name="title">标题.</param>
        /// <param name="episodes">分集列表.</param>
        public PgcExtraItemViewModel(
            string title,
            IEnumerable<EpisodeInformation> episodes,
            string currentId)
        {
            Title = title;
            Episodes = new ObservableCollection<EpisodeItemViewModel>();
            foreach (var item in episodes)
            {
                var vm = Splat.Locator.Current.GetService<EpisodeItemViewModel>();
                vm.SetInformation(item);
                vm.IsSelected = item.Identifier.Id == currentId;
                Episodes.Add(vm);
            }
        }

        /// <summary>
        /// 标题.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <summary>
        /// 分集.
        /// </summary>
        public ObservableCollection<EpisodeItemViewModel> Episodes { get; }
    }
}

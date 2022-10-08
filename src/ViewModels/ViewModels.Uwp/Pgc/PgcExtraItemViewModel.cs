// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bili.Models.Data.Pgc;
using Bili.ViewModels.Interfaces.Pgc;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC 附加内容条目视图模型.
    /// </summary>
    public sealed class PgcExtraItemViewModel : ViewModelBase, IPgcExtraItemViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcExtraItemViewModel"/> class.
        /// </summary>
        public PgcExtraItemViewModel()
            => Episodes = new ObservableCollection<IEpisodeItemViewModel>();

        /// <summary>
        /// 标题.
        /// </summary>
        [ObservableProperty]
        public string Title { get; set; }

        /// <summary>
        /// 分集.
        /// </summary>
        public ObservableCollection<IEpisodeItemViewModel> Episodes { get; }

        /// <inheritdoc/>
        public void SetData(string title, IEnumerable<EpisodeInformation> episodes, string currentId)
        {
            Title = title;
            foreach (var item in episodes)
            {
                var vm = Locator.Current.GetService<IEpisodeItemViewModel>();
                vm.InjectData(item);
                vm.IsSelected = item.Identifier.Id == currentId;
                Episodes.Add(vm);
            }
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// PGC模块视图模型.
    /// </summary>
    public partial class PgcModuleViewModel : ViewModelBase
    {
        /// <summary>
        /// 从动漫模块创建视图模型.
        /// </summary>
        /// <param name="module">动漫模块.</param>
        /// <returns><see cref="PgcModuleViewModel"/>.</returns>
        public static PgcModuleViewModel CreateFromAnime(PgcModule module)
        {
            var vm = new PgcModuleViewModel();
            vm.Title = module.Title;
            vm.SeasonCollection = new ObservableCollection<SeasonViewModel>();
            vm.Type = PgcModuleType.Anime;
            module.Items.ForEach(p => vm.SeasonCollection.Add(SeasonViewModel.CreateFromModuleItem(p)));
            if (module.Headers != null && module.Headers.Count > 0)
            {
                var header = module.Headers.First();
                if (header.Url.Contains("/sl"))
                {
                    var uri = new Uri(header.Url);
                    var playListId = uri.Segments.Where(p => p.Contains("sl")).First();
                    vm.Id = Convert.ToInt32(playListId.Replace("sl", string.Empty));
                    vm.IsDisplayMoreButton = true;
                }
            }

            return vm;
        }

        /// <summary>
        /// 从排行榜条目中创建.
        /// </summary>
        /// <param name="module">排行榜.</param>
        /// <returns><see cref="PgcModuleViewModel"/>.</returns>
        public static PgcModuleViewModel CreateFromRank(PgcModuleItem module)
        {
            var vm = new PgcModuleViewModel();
            vm.Title = module.Title;
            vm.SeasonCollection = new ObservableCollection<SeasonViewModel>();
            vm.Type = PgcModuleType.Rank;
            module.Cards.Take(3).ToList().ForEach(p => vm.SeasonCollection.Add(SeasonViewModel.CreateFromModuleItem(p, false)));
            vm.IsDisplayMoreButton = false;
            return vm;
        }
    }
}

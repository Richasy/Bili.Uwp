// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// PGC相关内容视图模型.
    /// </summary>
    public class PgcSectionViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcSectionViewModel"/> class.
        /// </summary>
        /// <param name="module">模块信息.</param>
        public PgcSectionViewModel(PgcDetailModule module)
        {
            Source = module;
            Name = module.Title;
            Episodes = new ObservableCollection<PgcEpisodeViewModel>();
            module.Data.Episodes.ForEach(p => Episodes.Add(new PgcEpisodeViewModel(p, false)));
        }

        /// <summary>
        /// 显示标题.
        /// </summary>
        [Reactive]
        public string Name { get; set; }

        /// <summary>
        /// 分集列表.
        /// </summary>
        [Reactive]
        public ObservableCollection<PgcEpisodeViewModel> Episodes { get; set; }

        /// <summary>
        /// 源数据.
        /// </summary>
        public PgcDetailModule Source { get; }
    }
}

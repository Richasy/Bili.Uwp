// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 动漫视图模型基类.
    /// </summary>
    public partial class AnimeViewModelBase
    {
        /// <summary>
        /// PGC类型.
        /// </summary>
        public PgcType Type { get; set; }

        /// <summary>
        /// 标签集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<PgcTabViewModel> TabCollection { get; set; }

        /// <summary>
        /// 当前选中标签.
        /// </summary>
        [Reactive]
        public PgcTabViewModel CurrentTab { get; set; }
    }
}

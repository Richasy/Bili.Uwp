// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 动漫视图模型基类.
    /// </summary>
    public partial class AnimeViewModelBase
    {
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

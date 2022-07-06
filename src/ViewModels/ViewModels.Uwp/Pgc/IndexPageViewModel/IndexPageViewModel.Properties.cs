// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC 内容索引页面视图模型.
    /// </summary>
    public sealed partial class IndexPageViewModel
    {
        private readonly IPgcProvider _pgcProvider;
        private readonly IResourceToolkit _resourceToolkit;

        private PgcType _type;
        private bool _isFinished;

        /// <summary>
        /// 筛选条件集合.
        /// </summary>
        public ObservableCollection<IndexFilterViewModel> Filters { get; }

        /// <summary>
        /// 页面类型.
        /// </summary>
        [Reactive]
        public string PageType { get; set; }

        /// <summary>
        /// 是否为空.
        /// </summary>
        [Reactive]
        public bool IsEmpty { get; set; }
    }
}

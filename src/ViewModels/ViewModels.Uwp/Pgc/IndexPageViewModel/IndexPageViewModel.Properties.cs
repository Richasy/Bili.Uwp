// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Pgc;
using CommunityToolkit.Mvvm.ComponentModel;

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
        /// 页面类型.
        /// </summary>
        [ObservableProperty]
        private string _pageType;

        /// <summary>
        /// 是否为空.
        /// </summary>
        [ObservableProperty]
        private bool _isEmpty;

        /// <summary>
        /// 筛选条件集合.
        /// </summary>
        public ObservableCollection<IIndexFilterViewModel> Filters { get; }
    }
}

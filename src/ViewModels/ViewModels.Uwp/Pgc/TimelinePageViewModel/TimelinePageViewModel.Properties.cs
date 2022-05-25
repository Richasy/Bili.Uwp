// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Pgc;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// 时间线页面视图模型.
    /// </summary>
    public sealed partial class TimelinePageViewModel
    {
        private readonly IPgcProvider _pgcProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly ObservableAsPropertyHelper<bool> _isReloading;
        private PgcType _type;

        /// <summary>
        /// 时间线集合.
        /// </summary>
        public ObservableCollection<TimelineInformation> Timelines { get; }

        /// <summary>
        /// 是否正在初始化.
        /// </summary>
        public bool IsReloading => _isReloading.Value;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> InitializeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <summary>
        /// 标题.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <summary>
        /// 描述.
        /// </summary>
        [Reactive]
        public string Description { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsError { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string ErrorText { get; set; }
    }
}

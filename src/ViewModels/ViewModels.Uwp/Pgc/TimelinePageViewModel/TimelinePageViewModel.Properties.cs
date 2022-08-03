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
        private PgcType _type;

        /// <inheritdoc/>
        public ObservableCollection<TimelineInformation> Timelines { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> InitializeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <inheritdoc/>
        [Reactive]
        public string Title { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string Description { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsError { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string ErrorText { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsReloading { get; set; }
    }
}

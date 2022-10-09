// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Pgc;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private bool _isError;

        [ObservableProperty]
        private string _errorText;

        [ObservableProperty]
        private bool _isReloading;

        /// <inheritdoc/>
        public ObservableCollection<TimelineInformation> Timelines { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand InitializeCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand ReloadCommand { get; }
    }
}

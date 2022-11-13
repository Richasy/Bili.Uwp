// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Base
{
    /// <summary>
    /// PGC 信息流页面（不包括动漫）的通用视图模型.
    /// </summary>
    public partial class PgcPageViewModelBase
    {
        private readonly IPgcProvider _pgcProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly INavigationViewModel _navigationViewModel;
        private readonly PgcType _type;

        [ObservableProperty]
        private bool _isShowBanner;

        [ObservableProperty]
        private string _title;

        /// <inheritdoc/>
        public ObservableCollection<IBannerViewModel> Banners { get; }

        /// <inheritdoc/>
        public IRelayCommand GotoIndexPageCommand { get; }
    }
}

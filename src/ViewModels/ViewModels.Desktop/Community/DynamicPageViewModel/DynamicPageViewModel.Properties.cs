// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Community;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Community
{
    /// <summary>
    /// 动态页面视图模型.
    /// </summary>
    public sealed partial class DynamicPageViewModel
    {
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IAuthorizeProvider _authorizeProvider;

        [ObservableProperty]
        private DynamicHeader _currentHeader;

        [ObservableProperty]
        private bool _isShowVideo;

        [ObservableProperty]
        private bool _isShowAll;

        [ObservableProperty]
        private bool _needSignIn;

        /// <inheritdoc/>
        public IRelayCommand RefreshModuleCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<DynamicHeader> SelectHeaderCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<DynamicHeader> Headers { get; }

        /// <inheritdoc/>
        public IDynamicVideoModuleViewModel VideoModule { get; }

        /// <inheritdoc/>
        public IDynamicAllModuleViewModel AllModule { get; }
    }
}

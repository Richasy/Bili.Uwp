// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Community;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 动态页面视图模型.
    /// </summary>
    public sealed partial class DynamicPageViewModel
    {
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IAuthorizeProvider _authorizeProvider;

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

        /// <inheritdoc/>
        [ObservableProperty]
        public DynamicHeader CurrentHeader { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowVideo { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowAll { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool NeedSignIn { get; set; }
    }
}

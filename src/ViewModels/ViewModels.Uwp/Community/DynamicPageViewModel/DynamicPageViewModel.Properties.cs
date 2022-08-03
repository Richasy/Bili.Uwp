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
        public ReactiveCommand<Unit, Unit> RefreshModuleCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<DynamicHeader, Unit> SelectHeaderCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<DynamicHeader> Headers { get; }

        /// <inheritdoc/>
        public IDynamicVideoModuleViewModel VideoModule { get; }

        /// <inheritdoc/>
        public IDynamicAllModuleViewModel AllModule { get; }

        /// <inheritdoc/>
        [Reactive]
        public DynamicHeader CurrentHeader { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowVideo { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowAll { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool NeedSignIn { get; set; }
    }
}

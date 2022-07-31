// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Models.App.Other;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Account
{
    /// <summary>
    /// 收藏夹页面视图模型.
    /// </summary>
    public sealed partial class FavoritePageViewModel
    {
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IAppViewModel _appViewModel;

        /// <inheritdoc/>
        public ObservableCollection<FavoriteHeader> TypeCollection { get; }

        /// <inheritdoc/>
        public ReactiveCommand<FavoriteHeader, Unit> SelectTypeCommand { get; }

        /// <inheritdoc/>
        [Reactive]
        public FavoriteHeader CurrentType { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsVideoShown { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsAnimeShown { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsCinemaShown { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsArticleShown { get; set; }
    }
}

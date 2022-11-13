// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.App.Other;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Account
{
    /// <summary>
    /// 收藏夹页面视图模型.
    /// </summary>
    public sealed partial class FavoritePageViewModel
    {
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IAppViewModel _appViewModel;

        [ObservableProperty]
        private FavoriteHeader _currentType;

        [ObservableProperty]
        private bool _isVideoShown;

        [ObservableProperty]
        private bool _isAnimeShown;

        [ObservableProperty]
        private bool _isCinemaShown;

        [ObservableProperty]
        private bool _isArticleShown;

        /// <inheritdoc/>
        public ObservableCollection<FavoriteHeader> TypeCollection { get; }

        /// <inheritdoc/>
        public IRelayCommand<FavoriteHeader> SelectTypeCommand { get; }
    }
}

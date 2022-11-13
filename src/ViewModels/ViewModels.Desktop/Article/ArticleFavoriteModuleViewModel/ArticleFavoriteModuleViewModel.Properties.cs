// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bili.ViewModels.Desktop.Article
{
    /// <summary>
    /// 文章收藏夹视图模型.
    /// </summary>
    public sealed partial class ArticleFavoriteModuleViewModel
    {
        private readonly IFavoriteProvider _favoriteProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private bool _isEnd;

        [ObservableProperty]
        private bool _isEmpty;
    }
}

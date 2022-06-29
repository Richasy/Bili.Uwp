// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Toolkit.Interfaces;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Article
{
    /// <summary>
    /// 文章收藏夹视图模型.
    /// </summary>
    public sealed partial class ArticleFavoriteModuleViewModel
    {
        private readonly IFavoriteProvider _favoriteProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private bool _isEnd;

        /// <summary>
        /// 是否显示空白.
        /// </summary>
        [Reactive]
        public bool IsEmpty { get; set; }
    }
}

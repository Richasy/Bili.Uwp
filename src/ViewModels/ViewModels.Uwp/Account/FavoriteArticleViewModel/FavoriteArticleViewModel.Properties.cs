// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 文章收藏夹视图模型.
    /// </summary>
    public partial class FavoriteArticleViewModel
    {
        private int _pageNumber;
        private bool _isCompleted;

        /// <summary>
        /// 实例.
        /// </summary>
        public static FavoriteArticleViewModel Instance { get; } = new Lazy<FavoriteArticleViewModel>(() => new FavoriteArticleViewModel()).Value;

        /// <summary>
        /// 剧集集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<ArticleViewModel> ArticleCollection { get; set; }

        /// <summary>
        /// 是否显示空白.
        /// </summary>
        [Reactive]
        public bool IsShowEmpty { get; set; }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Article;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Article
{
    /// <summary>
    /// 文章条目视图模型.
    /// </summary>
    public sealed partial class ArticleItemViewModel
    {
        private readonly INumberToolkit _numberToolkit;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IArticleProvider _articleProvider;
        private readonly IFavoriteProvider _favoriteProvider;
        private readonly AppViewModel _appViewModel;
        private readonly ObservableAsPropertyHelper<bool> _isReloading;

        private string _detailContent;
        private bool _disposedValue;
        private Action<ArticleItemViewModel> _additionalAction;

        /// <summary>
        /// 在网页中打开的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> OpenInBroswerCommand { get; }

        /// <summary>
        /// 阅读命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ReadCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <summary>
        /// 取消收藏命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> UnfavoriteCommand { get; }

        /// <summary>
        /// 视频信息.
        /// </summary>
        [Reactive]
        public ArticleInformation Information { get; internal set; }

        /// <summary>
        /// 阅读次数的可读文本.
        /// </summary>
        [Reactive]
        public string ViewCountText { get; internal set; }

        /// <summary>
        /// 点赞数的可读文本.
        /// </summary>
        [Reactive]
        public string LikeCountText { get; internal set; }

        /// <summary>
        /// 评论数的可读文本.
        /// </summary>
        [Reactive]
        public string CommentCountText { get; internal set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsError { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string ErrorText { get; set; }

        /// <summary>
        /// 是否显示社区信息.
        /// </summary>
        [Reactive]
        public bool IsShowCommunity { get; set; }

        /// <inheritdoc/>
        public bool IsReloading => _isReloading.Value;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is ArticleItemViewModel model && EqualityComparer<ArticleInformation>.Default.Equals(Information, model.Information);

        /// <inheritdoc/>
        public override int GetHashCode() => Information.GetHashCode();
    }
}

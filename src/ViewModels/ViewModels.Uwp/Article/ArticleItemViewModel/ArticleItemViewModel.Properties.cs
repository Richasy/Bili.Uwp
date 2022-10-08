// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Article;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Article;
using Bili.ViewModels.Interfaces.Core;
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
        private readonly ICallerViewModel _callerViewModel;

        private string _detailContent;
        private Action<IArticleItemViewModel> _additionalAction;

        /// <inheritdoc/>
        public IRelayCommand OpenInBroswerCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ReadCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand UnfavoriteCommand { get; }

        /// <inheritdoc/>
        [ObservableProperty]
        public ArticleInformation Data { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public IUserItemViewModel Publisher { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string ViewCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string LikeCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string CommentCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsError { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string ErrorText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowCommunity { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsReloading { get; private set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is ArticleItemViewModel model && EqualityComparer<ArticleInformation>.Default.Equals(Data, model.Data);

        /// <inheritdoc/>
        public override int GetHashCode() => Data.GetHashCode();
    }
}

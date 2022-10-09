// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Article;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Article;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

        [ObservableProperty]
        private ArticleInformation _data;

        [ObservableProperty]
        private IUserItemViewModel _publisher;

        [ObservableProperty]
        private string _viewCountText;

        [ObservableProperty]
        private string _likeCountText;

        [ObservableProperty]
        private string _commentCountText;

        [ObservableProperty]
        private bool _isError;

        [ObservableProperty]
        private string _errorText;

        [ObservableProperty]
        private bool _isShowCommunity;

        [ObservableProperty]
        private bool _isReloading;

        /// <inheritdoc/>
        public IAsyncRelayCommand OpenInBroswerCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ReadCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand UnfavoriteCommand { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is ArticleItemViewModel model && EqualityComparer<ArticleInformation>.Default.Equals(Data, model.Data);

        /// <inheritdoc/>
        public override int GetHashCode() => Data.GetHashCode();
    }
}

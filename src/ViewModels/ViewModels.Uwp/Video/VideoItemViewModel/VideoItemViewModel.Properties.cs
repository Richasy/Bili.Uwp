// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Video;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频条目视图模型.
    /// </summary>
    public sealed partial class VideoItemViewModel
    {
        private readonly INumberToolkit _numberToolkit;
        private readonly IAccountProvider _accountProvider;
        private readonly IAuthorizeProvider _authorizeProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IFavoriteProvider _favoriteProvider;
        private readonly INavigationViewModel _navigationViewModel;
        private readonly ICallerViewModel _callerViewModel;
        private Action<IVideoItemViewModel> _additionalAction;
        private object _additionalData;

        /// <inheritdoc/>
        public IRelayCommand AddToViewLaterCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand RemoveFromViewLaterCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand RemoveFromHistoryCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand RemoveFromFavoriteCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand OpenInBroswerCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand PlayCommand { get; }

        /// <inheritdoc/>
        [ObservableProperty]
        public VideoInformation Data { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public IUserItemViewModel Publisher { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string PlayCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string DanmakuCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string LikeCountText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string DurationText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowScore { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string ScoreText { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowCommunity { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsSelected { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is VideoItemViewModel model && EqualityComparer<VideoInformation>.Default.Equals(Data, model.Data);

        /// <inheritdoc/>
        public override int GetHashCode() => Data.GetHashCode();
    }
}

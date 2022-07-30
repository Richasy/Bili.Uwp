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
        public ReactiveCommand<Unit, Unit> AddToViewLaterCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> RemoveFromViewLaterCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> RemoveFromHistoryCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> RemoveFromFavoriteCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> OpenInBroswerCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> PlayCommand { get; }

        /// <inheritdoc/>
        [Reactive]
        public VideoInformation Data { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public IUserItemViewModel Publisher { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string PlayCountText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string DanmakuCountText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string LikeCountText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string DurationText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowScore { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string ScoreText { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowCommunity { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsSelected { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is VideoItemViewModel model && EqualityComparer<VideoInformation>.Default.Equals(Data, model.Data);

        /// <inheritdoc/>
        public override int GetHashCode() => Data.GetHashCode();
    }
}

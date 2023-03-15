// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Video;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Workspace.Core
{
    /// <summary>
    /// 视频条目视图模型.
    /// </summary>
    public sealed partial class VideoItemViewModel
    {
        private readonly INumberToolkit _numberToolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly IAccountProvider _accountProvider;
        private readonly IAuthorizeProvider _authorizeProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IFavoriteProvider _favoriteProvider;
        private readonly ICallerViewModel _callerViewModel;
        private Action<IVideoItemViewModel> _additionalAction;
        private object _additionalData;

        [ObservableProperty]
        private VideoInformation _data;

        [ObservableProperty]
        private IUserItemViewModel _publisher;

        [ObservableProperty]
        private string _playCountText;

        [ObservableProperty]
        private string _danmakuCountText;

        [ObservableProperty]
        private string _likeCountText;

        [ObservableProperty]
        private string _durationText;

        [ObservableProperty]
        private bool _isShowScore;

        [ObservableProperty]
        private string _scoreText;

        [ObservableProperty]
        private bool _isShowCommunity;

        [ObservableProperty]
        private bool _isSelected;

        /// <inheritdoc/>
        public IAsyncRelayCommand AddToViewLaterCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand RemoveFromViewLaterCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand RemoveFromHistoryCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand RemoveFromFavoriteCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand OpenInBroswerCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand PlayInPrivateCommand { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is VideoItemViewModel model && EqualityComparer<VideoInformation>.Default.Equals(Data, model.Data);

        /// <inheritdoc/>
        public override int GetHashCode() => Data.GetHashCode();
    }
}

// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Data.User;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Video;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Account
{
    /// <summary>
    /// 用户空间视图模型.
    /// </summary>
    public sealed partial class UserSpaceViewModel
    {
        private readonly IAccountProvider _accountProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly INavigationViewModel _navigationViewModel;
        private readonly IAccountViewModel _accountViewModel;
        private readonly ICallerViewModel _callerViewModel;
        private UserProfile _userProfile;
        private bool _isSpaceVideoFinished;
        private bool _isSearchVideoFinished;
        private string _requestKeyword;

        [ObservableProperty]
        private IUserItemViewModel _userViewModel;

        [ObservableProperty]
        private bool _isMe;

        [ObservableProperty]
        private bool _isSearchMode;

        [ObservableProperty]
        private string _keyword;

        [ObservableProperty]
        private bool _isSpaceVideoEmpty;

        [ObservableProperty]
        private bool _isSearchVideoEmpty;

        [ObservableProperty]
        private bool _isFixed;

        [ObservableProperty]
        private bool _isSearching;

        [ObservableProperty]
        private bool _canSearch;

        /// <inheritdoc/>
        public IAsyncRelayCommand SearchCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand EnterSearchModeCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ExitSearchModeCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand GotoFansPageCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand GotoFollowsPageCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand FixedCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<IVideoItemViewModel> SearchVideos { get; }
    }
}

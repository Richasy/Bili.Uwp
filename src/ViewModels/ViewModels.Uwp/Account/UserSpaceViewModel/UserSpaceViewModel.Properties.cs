// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.User;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Video;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Account
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

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> SearchCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> EnterSearchModeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ExitSearchModeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> GotoFansPageCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> GotoFollowsPageCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> FixedCommand { get; }

        /// <inheritdoc/>
        public ObservableCollection<IVideoItemViewModel> SearchVideos { get; }

        /// <inheritdoc/>
        [Reactive]
        public IUserItemViewModel UserViewModel { get; internal set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsMe { get; internal set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsSearchMode { get; internal set; }

        /// <inheritdoc/>
        [Reactive]
        public string Keyword { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsSpaceVideoEmpty { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsSearchVideoEmpty { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsFixed { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsSearching { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool CanSearch { get; set; }
    }
}

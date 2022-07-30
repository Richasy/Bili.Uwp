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
        private readonly AccountViewModel _accountViewModel;
        private readonly ICallerViewModel _callerViewModel;
        private readonly ObservableAsPropertyHelper<bool> _isSearching;
        private readonly ObservableAsPropertyHelper<bool> _canSearch;
        private UserProfile _userProfile;
        private bool _isSpaceVideoFinished;
        private bool _isSearchVideoFinished;
        private string _requestKeyword;

        /// <summary>
        /// 搜索命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> SearchCommand { get; }

        /// <summary>
        /// 进入搜索模式的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> EnterSearchModeCommand { get; }

        /// <summary>
        /// 退出搜索模式的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ExitSearchModeCommand { get; }

        /// <summary>
        /// 前往粉丝页面的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> GotoFansPageCommand { get; }

        /// <summary>
        /// 前往关注者页面的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> GotoFollowsPageCommand { get; }

        /// <summary>
        /// 固定条目的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> FixedCommand { get; }

        /// <summary>
        /// 搜索的视频结果.
        /// </summary>
        public ObservableCollection<IVideoItemViewModel> SearchVideos { get; }

        /// <summary>
        /// 账户信息.
        /// </summary>
        [Reactive]
        public IUserItemViewModel UserViewModel { get; internal set; }

        /// <summary>
        /// 是否为我自己（已登录用户）的用户空间.
        /// </summary>
        [Reactive]
        public bool IsMe { get; internal set; }

        /// <summary>
        /// 是否为搜索模式.
        /// </summary>
        [Reactive]
        public bool IsSearchMode { get; internal set; }

        /// <summary>
        /// 搜索关键词.
        /// </summary>
        [Reactive]
        public string Keyword { get; set; }

        /// <summary>
        /// 空间视频是否为空.
        /// </summary>
        [Reactive]
        public bool IsSpaceVideoEmpty { get; set; }

        /// <summary>
        /// 视频搜索结果是否为空.
        /// </summary>
        [Reactive]
        public bool IsSearchVideoEmpty { get; set; }

        /// <summary>
        /// 该用户是否已经被固定在首页.
        /// </summary>
        [Reactive]
        public bool IsFixed { get; set; }

        /// <summary>
        /// 是否正在搜索.
        /// </summary>
        public bool IsSearching => _isSearching.Value;

        /// <summary>
        /// 是否可以搜索.
        /// </summary>
        public bool CanSearch => _canSearch.Value;
    }
}

// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Models.Data.User;
using Bili.Models.Enums.Community;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Account
{
    /// <summary>
    /// 用户条目视图模型.
    /// </summary>
    public sealed partial class UserItemViewModel
    {
        private readonly INumberToolkit _numberToolkit;
        private readonly IAccountProvider _accountProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly ICallerViewModel _callerViewModel;
        private readonly INavigationViewModel _navigationViewModel;
        private readonly IAccountViewModel _accountViewModel;
        private readonly CoreDispatcher _dispatcher;

        /// <summary>
        /// 用户基础信息.
        /// </summary>
        [ObservableProperty]
        private UserProfile _user;

        /// <summary>
        /// 用户自我介绍.
        /// </summary>
        [ObservableProperty]
        private string _introduce;

        /// <summary>
        /// 是否为大会员.
        /// </summary>
        [ObservableProperty]
        private bool _isVip;

        /// <summary>
        /// 等级.
        /// </summary>
        [ObservableProperty]
        private int _level;

        /// <summary>
        /// 角色.
        /// </summary>
        [ObservableProperty]
        private string _role;

        /// <summary>
        /// 关注数的可读文本.
        /// </summary>
        [ObservableProperty]
        private string _followCountText;

        /// <summary>
        /// 粉丝数的可读文本.
        /// </summary>
        [ObservableProperty]
        private string _fansCountText;

        /// <summary>
        /// 硬币数的可读文本.
        /// </summary>
        [ObservableProperty]
        private string _coinCountText;

        [ObservableProperty]
        private string _likeCountText;

        [ObservableProperty]
        private UserRelationStatus _relation;

        [ObservableProperty]
        private bool _isRelationButtonShown;

        [ObservableProperty]
        private bool _isRelationChanging;

        /// <inheritdoc/>
        public IAsyncRelayCommand ToggleRelationCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand InitializeRelationCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand ShowDetailCommand { get; }
    }
}

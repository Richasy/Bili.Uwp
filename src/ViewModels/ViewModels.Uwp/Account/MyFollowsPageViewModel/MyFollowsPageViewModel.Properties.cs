// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Uwp.Account
{
    /// <summary>
    /// 我的关注页面视图模型.
    /// </summary>
    public sealed partial class MyFollowsPageViewModel
    {
        private readonly IAccountProvider _accountProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IAccountViewModel _accountViewModel;

        private readonly Dictionary<string, IEnumerable<IUserItemViewModel>> _cache;

        [ObservableProperty]
        private FollowGroup _currentGroup;

        [ObservableProperty]
        private bool _isCurrentGroupEmpty;

        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private bool _isSwitching;

        /// <inheritdoc/>
        public ObservableCollection<FollowGroup> Groups { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand<FollowGroup> SelectGroupCommand { get; }
    }
}

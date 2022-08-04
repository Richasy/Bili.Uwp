// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

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

        /// <inheritdoc/>
        public ObservableCollection<FollowGroup> Groups { get; }

        /// <inheritdoc/>
        public ReactiveCommand<FollowGroup, Unit> SelectGroupCommand { get; }

        /// <inheritdoc/>
        [Reactive]
        public FollowGroup CurrentGroup { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsCurrentGroupEmpty { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string UserName { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsSwitching { get; set; }
    }
}

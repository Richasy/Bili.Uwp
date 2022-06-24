// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Data.User;
using Bili.Toolkit.Interfaces;
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
        private readonly AccountViewModel _accountViewModel;

        private readonly Dictionary<string, IEnumerable<UserItemViewModel>> _cache;
        private readonly ObservableAsPropertyHelper<bool> _isSwitching;

        /// <summary>
        /// 关注分组列表.
        /// </summary>
        public ObservableCollection<FollowGroup> Groups { get; }

        /// <summary>
        /// 选中分组命令.
        /// </summary>
        public ReactiveCommand<FollowGroup, Unit> SelectGroupCommand { get; }

        /// <summary>
        /// 当前分组.
        /// </summary>
        [Reactive]
        public FollowGroup CurrentGroup { get; internal set; }

        /// <summary>
        /// 当前分组内容是否为空.
        /// </summary>
        [Reactive]
        public bool IsCurrentGroupEmpty { get; internal set; }

        /// <summary>
        /// 用户名.
        /// </summary>
        [Reactive]
        public string UserName { get; internal set; }

        /// <summary>
        /// 是否正在切换分组.
        /// </summary>
        public bool IsSwitching => _isSwitching.Value;
    }
}

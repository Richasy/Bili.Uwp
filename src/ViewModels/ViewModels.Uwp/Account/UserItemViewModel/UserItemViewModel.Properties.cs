// Copyright (c) Richasy. All rights reserved.

using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.User;
using Bili.Models.Enums.Community;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
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
        private readonly AccountViewModel _accountViewModel;
        private readonly CoreDispatcher _dispatcher;

        private readonly ObservableAsPropertyHelper<bool> _isRelationChanging;

        /// <summary>
        /// 切换关系命令（关注或取消关注）.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ToggleRelationCommand { get; }

        /// <summary>
        /// 显示用户资料详情的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShowDetailCommand { get; }

        /// <summary>
        /// 初始化关系命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> InitializeRelationCommand { get; }

        /// <summary>
        /// 用户基础信息.
        /// </summary>
        [Reactive]
        public UserProfile User { get; set; }

        /// <summary>
        /// 用户自我介绍.
        /// </summary>
        [Reactive]
        public string Introduce { get; set; }

        /// <summary>
        /// 是否为大会员.
        /// </summary>
        [Reactive]
        public bool IsVip { get; set; }

        /// <summary>
        /// 等级.
        /// </summary>
        [Reactive]
        public int Level { get; set; }

        /// <summary>
        /// 角色.
        /// </summary>
        [Reactive]
        public string Role { get; set; }

        /// <summary>
        /// 关注数的可读文本.
        /// </summary>
        [Reactive]
        public string FollowCountText { get; set; }

        /// <summary>
        /// 粉丝数的可读文本.
        /// </summary>
        [Reactive]
        public string FansCountText { get; set; }

        /// <summary>
        /// 硬币数的可读文本.
        /// </summary>
        [Reactive]
        public string CoinCountText { get; set; }

        /// <summary>
        /// 点赞文本.
        /// </summary>
        [Reactive]
        public string LikeCountText { get; set; }

        /// <summary>
        /// 与该用户的关系.
        /// </summary>
        [Reactive]
        public UserRelationStatus Relation { get; set; }

        /// <summary>
        /// 是否显示关系按钮.
        /// </summary>
        [Reactive]
        public bool IsRelationButtonShown { get; set; }

        /// <summary>
        /// 是否正在修改关系.
        /// </summary>
        public bool IsRelationChanging => _isRelationChanging.Value;
    }
}

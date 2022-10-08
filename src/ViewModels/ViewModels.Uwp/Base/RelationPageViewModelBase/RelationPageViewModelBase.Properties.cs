// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Models.Data.User;
using Bili.Models.Enums.Bili;
using Bili.Toolkit.Interfaces;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Base
{
    /// <summary>
    /// 关系用户页面视图模型基类，是粉丝/关注页面的基础.
    /// </summary>
    public partial class RelationPageViewModelBase
    {
        private readonly IAccountProvider _accountProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly RelationType _relationType;

        private bool _isEnd = false;

        /// <summary>
        /// 对应的用户基础资料.
        /// </summary>
        [ObservableProperty]
        public UserProfile Profile { get; internal set; }

        /// <summary>
        /// 标题后缀.
        /// </summary>
        [ObservableProperty]
        public string TitleSuffix { get; internal set; }

        /// <summary>
        /// 是否为空.
        /// </summary>
        [ObservableProperty]
        public bool IsEmpty { get; set; }
    }
}

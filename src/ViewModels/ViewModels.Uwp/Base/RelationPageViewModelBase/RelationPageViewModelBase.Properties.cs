// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Models.Data.User;
using Bili.Models.Enums.Bili;
using Bili.Toolkit.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

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

        [ObservableProperty]
        private UserProfile _profile;

        [ObservableProperty]
        private string _titleSuffix;

        [ObservableProperty]
        private bool _isEmpty;
    }
}

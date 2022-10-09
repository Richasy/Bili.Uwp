// Copyright (c) Richasy. All rights reserved.

using Bili.Lib.Interfaces;
using Bili.Models.Data.User;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频收藏夹详情视图模型.
    /// </summary>
    public sealed partial class VideoFavoriteFolderDetailViewModel
    {
        private readonly IFavoriteProvider _favoriteProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly IAccountViewModel _accountViewModel;
        private bool _isEnd;

        [ObservableProperty]
        private UserProfile _user;

        [ObservableProperty]
        private VideoFavoriteFolder _data;

        [ObservableProperty]
        private bool _isEmpty;
    }
}

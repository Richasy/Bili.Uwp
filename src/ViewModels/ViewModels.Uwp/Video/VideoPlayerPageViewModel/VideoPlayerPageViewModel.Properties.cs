// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频播放页面视图模型.
    /// </summary>
    public sealed partial class VideoPlayerPageViewModel
    {
        private readonly IPlayerProvider _playerProvider;
        private readonly IAuthorizeProvider _authorizeProvider;
        private readonly IFavoriteProvider _favoriteProvider;
        private readonly IAccountProvider _accountProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly INumberToolkit _numberToolkit;
        private readonly AppViewModel _appViewModel;
        private readonly NavigationViewModel _navigationViewModel;
        private readonly CoreDispatcher _dispatcher;
        private readonly ObservableAsPropertyHelper<bool> _isReloading;
        private readonly ObservableAsPropertyHelper<bool> _isFavoriteFolderRequesting;

        private string _presetVideoId;

        /// <inheritdoc/>
        public bool IsReloading => _isReloading.Value;

        /// <summary>
        /// 是否正在请求收藏夹信息.
        /// </summary>
        public bool IsFavoriteFolderRequesting => _isFavoriteFolderRequesting.Value;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <summary>
        /// 请求用户已有的收藏夹列表.
        /// </summary>
        public ReactiveCommand<Unit, Unit> RequestFavoriteFolders { get; }

        /// <summary>
        /// 视频协作者.
        /// </summary>
        public ObservableCollection<UserItemViewModel> Collaborators { get; }

        /// <summary>
        /// 视频标签集.
        /// </summary>
        public ObservableCollection<Tag> Tags { get; }

        /// <summary>
        /// 收藏夹列表.
        /// </summary>
        public ObservableCollection<VideoFavoriteFolderSelectableViewModel> FavoriteFolders { get; }

        /// <summary>
        /// 视图信息.
        /// </summary>
        [Reactive]
        public VideoView View { get; set; }

        /// <summary>
        /// 用户是否已登录.
        /// </summary>
        [Reactive]
        public bool IsSignedIn { get; set; }

        /// <summary>
        /// 视频作者.
        /// </summary>
        [Reactive]
        public UserItemViewModel Author { get; set; }

        /// <summary>
        /// 是否为合作视频，<c>True</c> 则显示 <see cref="Collaborators"/>，<c>False</c> 则显示 <see cref="Author"/>.
        /// </summary>
        [Reactive]
        public bool IsCooperationVideo { get; set; }

        /// <summary>
        /// 发布时间的可读文本.
        /// </summary>
        [Reactive]
        public string PublishTime { get; set; }

        /// <summary>
        /// 播放次数的可读文本.
        /// </summary>
        [Reactive]
        public string PlayCountText { get; set; }

        /// <summary>
        /// 弹幕数的可读文本.
        /// </summary>
        [Reactive]
        public string DanmakuCountText { get; set; }

        /// <summary>
        /// 评论数的可读文本.
        /// </summary>
        [Reactive]
        public string CommentCountText { get; set; }

        /// <summary>
        /// 正在观看人数的可读文本.
        /// </summary>
        [Reactive]
        public string WatchingCountText { get; set; }

        /// <summary>
        /// 是否显示标签组.
        /// </summary>
        [Reactive]
        public bool IsShowTags { get; set; }

        /// <summary>
        /// 点赞数的可读文本.
        /// </summary>
        [Reactive]
        public string LikeCountText { get; set; }

        /// <summary>
        /// 投币数的可读文本.
        /// </summary>
        [Reactive]
        public string CoinCountText { get; set; }

        /// <summary>
        /// 收藏数的可读文本.
        /// </summary>
        [Reactive]
        public string FavoriteCountText { get; set; }

        /// <summary>
        /// 是否已点赞.
        /// </summary>
        [Reactive]
        public bool IsLiked { get; set; }

        /// <summary>
        /// 是否已投币.
        /// </summary>
        [Reactive]
        public bool IsCoined { get; set; }

        /// <summary>
        /// 是否已收藏.
        /// </summary>
        [Reactive]
        public bool IsFavorited { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsError { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string ErrorText { get; set; }

        /// <summary>
        /// 收藏夹列表请求是否出错.
        /// </summary>
        [Reactive]
        public bool IsFavoriteFoldersError { get; set; }

        /// <summary>
        /// 收藏夹列表请求错误文本.
        /// </summary>
        [Reactive]
        public string FavoriteFoldersErrorText { get; set; }
    }
}

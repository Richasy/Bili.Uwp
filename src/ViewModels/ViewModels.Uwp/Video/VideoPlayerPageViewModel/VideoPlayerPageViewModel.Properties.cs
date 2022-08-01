// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Data.Community;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Video;
using Bili.ViewModels.Uwp.Community;
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
        private readonly IResourceToolkit _resourceToolkit;
        private readonly INumberToolkit _numberToolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly ICallerViewModel _callerViewModel;
        private readonly IRecordViewModel _recordViewModel;
        private readonly INavigationViewModel _navigationViewModel;
        private readonly IAccountViewModel _accountViewModel;
        private readonly CommentPageViewModel _commentPageViewModel;
        private readonly CoreDispatcher _dispatcher;

        private string _presetVideoId;
        private Action _playNextVideoAction;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <summary>
        /// 请求用户已有的收藏夹列表的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> RequestFavoriteFoldersCommand { get; }

        /// <summary>
        /// 请求获取实时在线观看人数的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> RequestOnlineCountCommand { get; }

        /// <summary>
        /// 改变视频分P的命令.
        /// </summary>
        public ReactiveCommand<VideoIdentifier, Unit> ChangeVideoPartCommand { get; }

        /// <summary>
        /// 搜索标签命令.
        /// </summary>
        public ReactiveCommand<Tag, Unit> SearchTagCommand { get; }

        /// <summary>
        /// 选中视频合集的命令.
        /// </summary>
        public ReactiveCommand<VideoSeason, Unit> SelectSeasonCommand { get; }

        /// <summary>
        /// 收藏视频命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> FavoriteVideoCommand { get; }

        /// <summary>
        /// 投币命令.
        /// </summary>
        public ReactiveCommand<int, Unit> CoinCommand { get; }

        /// <summary>
        /// 点赞/取消点赞命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> LikeCommand { get; }

        /// <summary>
        /// 一键三连命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> TripleCommand { get; }

        /// <summary>
        /// 重置社区信息命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ReloadCommunityInformationCommand { get; }

        /// <summary>
        /// 分享命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShareCommand { get; }

        /// <summary>
        /// 固定条目命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> FixedCommand { get; }

        /// <summary>
        /// 清除数据命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }

        /// <summary>
        /// 清除播放列表命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ClearPlaylistCommand { get; }

        /// <summary>
        /// 视频协作者.
        /// </summary>
        public ObservableCollection<IUserItemViewModel> Collaborators { get; }

        /// <summary>
        /// 视频标签集.
        /// </summary>
        public ObservableCollection<Tag> Tags { get; }

        /// <summary>
        /// 收藏夹列表.
        /// </summary>
        public ObservableCollection<IVideoFavoriteFolderSelectableViewModel> FavoriteFolders { get; }

        /// <summary>
        /// 播放时的关联区块集合.
        /// </summary>
        public ObservableCollection<PlayerSectionHeader> Sections { get; }

        /// <summary>
        /// 关联的视频集合.
        /// </summary>
        public ObservableCollection<IVideoItemViewModel> RelatedVideos { get; }

        /// <summary>
        /// 视频播放列表.
        /// </summary>
        public ObservableCollection<IVideoItemViewModel> VideoPlaylist { get; }

        /// <summary>
        /// 视频分集集合.
        /// </summary>
        public ObservableCollection<IVideoIdentifierSelectableViewModel> VideoParts { get; }

        /// <summary>
        /// 合集集合.
        /// </summary>
        public ObservableCollection<VideoSeason> Seasons { get; }

        /// <summary>
        /// 当前合集下的视频列表.
        /// </summary>
        public ObservableCollection<IVideoItemViewModel> CurrentSeasonVideos { get; set; }

        /// <summary>
        /// 下载模块视图模型.
        /// </summary>
        public IDownloadModuleViewModel DownloadViewModel { get; }

        /// <summary>
        /// 视图信息.
        /// </summary>
        [Reactive]
        public VideoPlayerView View { get; set; }

        /// <summary>
        /// 用户是否已登录.
        /// </summary>
        [Reactive]
        public bool IsSignedIn { get; set; }

        /// <summary>
        /// 视频作者.
        /// </summary>
        [Reactive]
        public IUserItemViewModel Author { get; set; }

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

        /// <summary>
        /// 投币同时是否点赞视频.
        /// </summary>
        [Reactive]
        public bool IsCoinWithLiked { get; set; }

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

        /// <summary>
        /// 该视频是否已经被固定在首页.
        /// </summary>
        [Reactive]
        public bool IsVideoFixed { get; set; }

        /// <summary>
        /// 有分集的时候是否仅显示索引.
        /// </summary>
        [Reactive]
        public bool IsOnlyShowIndex { get; set; }

        /// <summary>
        /// 当前区块.
        /// </summary>
        [Reactive]
        public PlayerSectionHeader CurrentSection { get; set; }

        /// <summary>
        /// 当前的视频合集.
        /// </summary>
        [Reactive]
        public VideoSeason CurrentSeason { get; set; }

        /// <summary>
        /// 当前视频分P.
        /// </summary>
        [Reactive]
        public VideoIdentifier CurrentVideoPart { get; set; }

        /// <summary>
        /// 是否显示视频合集.
        /// </summary>
        [Reactive]
        public bool IsShowUgcSeason { get; set; }

        /// <summary>
        /// 是否显示关联视频.
        /// </summary>
        [Reactive]
        public bool IsShowRelatedVideos { get; set; }

        /// <summary>
        /// 是否显示视频播放列表.
        /// </summary>
        [Reactive]
        public bool IsShowVideoPlaylist { get; set; }

        /// <summary>
        /// 是否显示评论区.
        /// </summary>
        [Reactive]
        public bool IsShowComments { get; set; }

        /// <summary>
        /// 是否显示视频分集.
        /// </summary>
        [Reactive]
        public bool IsShowParts { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsReloading { get; set; }

        /// <summary>
        /// 是否正在请求收藏夹信息.
        /// </summary>
        [ObservableAsProperty]
        public bool IsFavoriteFolderRequesting { get; set; }
    }
}

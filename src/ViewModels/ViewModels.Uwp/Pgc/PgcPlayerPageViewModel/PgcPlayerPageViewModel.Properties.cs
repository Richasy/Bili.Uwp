// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.App.Other;
using Bili.Models.Data.Pgc;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Community;
using Bili.ViewModels.Uwp.Core;
using Bili.ViewModels.Uwp.Video;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Windows.UI.Core;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC 播放页面视图模型.
    /// </summary>
    public sealed partial class PgcPlayerPageViewModel
    {
        private readonly IPlayerProvider _playerProvider;
        private readonly IAuthorizeProvider _authorizeProvider;
        private readonly IFavoriteProvider _favoriteProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly INumberToolkit _numberToolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly IPgcProvider _pgcProvider;
        private readonly AppViewModel _appViewModel;
        private readonly NavigationViewModel _navigationViewModel;
        private readonly AccountViewModel _accountViewModel;
        private readonly CommentPageViewModel _commentPageViewModel;
        private readonly CoreDispatcher _dispatcher;
        private readonly ObservableAsPropertyHelper<bool> _isReloading;
        private readonly ObservableAsPropertyHelper<bool> _isFavoriteFolderRequesting;

        private string _presetEpisodeId;
        private string _presetSeasonId;
        private bool _needBiliPlus;

        /// <inheritdoc/>
        public bool IsReloading => _isReloading.Value;

        /// <summary>
        /// 是否正在请求收藏夹信息.
        /// </summary>
        public bool IsFavoriteFolderRequesting => _isFavoriteFolderRequesting.Value;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <summary>
        /// 请求用户已有的收藏夹列表的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> RequestFavoriteFoldersCommand { get; }

        /// <summary>
        /// 改变视频分P的命令.
        /// </summary>
        public ReactiveCommand<EpisodeInformation, Unit> ChangeEpisodeCommand { get; }

        /// <summary>
        /// 改变剧集季度的命令.
        /// </summary>
        public ReactiveCommand<SeasonInformation, Unit> ChangeSeasonCommand { get; }

        /// <summary>
        /// 收藏视频命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> FavoriteEpisodeCommand { get; }

        /// <summary>
        /// 追番/追剧命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> TrackSeasonCommand { get; }

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
        public ReactiveCommand<Unit, Unit> ReloadInteractionInformationCommand { get; }

        /// <summary>
        /// 分享命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShareCommand { get; }

        /// <summary>
        /// 固定条目命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> FixedCommand { get; }

        /// <summary>
        /// 显示剧集详情的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShowSeasonDetailCommand { get; }

        /// <summary>
        /// 清除播放数据的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ClearCommand { get; }

        /// <summary>
        /// 收藏夹列表.
        /// </summary>
        public ObservableCollection<VideoFavoriteFolderSelectableViewModel> FavoriteFolders { get; }

        /// <summary>
        /// 播放时的关联区块集合.
        /// </summary>
        public ObservableCollection<PlayerSectionHeader> Sections { get; }

        /// <summary>
        /// 分集集合.
        /// </summary>
        public ObservableCollection<EpisodeItemViewModel> Episodes { get; }

        /// <summary>
        /// 剧集集合.
        /// </summary>
        public ObservableCollection<VideoIdentifierSelectableViewModel> Seasons { get; }

        /// <summary>
        /// 附加内容集合.
        /// </summary>
        public ObservableCollection<PgcExtraItemViewModel> Extras { get; }

        /// <summary>
        /// 演员集合.
        /// </summary>
        public ObservableCollection<UserItemViewModel> Celebrities { get; }

        /// <summary>
        /// 媒体播放视图模型.
        /// </summary>
        public MediaPlayerViewModel MediaPlayerViewModel { get; }

        /// <summary>
        /// 视图信息.
        /// </summary>
        [Reactive]
        public PgcPlayerView View { get; set; }

        /// <summary>
        /// 用户是否已登录.
        /// </summary>
        [Reactive]
        public bool IsSignedIn { get; set; }

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
        /// 评分人次的可读文本.
        /// </summary>
        [Reactive]
        public string RatingCountText { get; set; }

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
        /// 是否已追番/追剧.
        /// </summary>
        [Reactive]
        public bool IsTracking { get; set; }

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
        /// 是否显示演员.
        /// </summary>
        [Reactive]
        public bool IsShowCelebrities { get; set; }

        /// <summary>
        /// 当前区块.
        /// </summary>
        [Reactive]
        public PlayerSectionHeader CurrentSection { get; set; }

        /// <summary>
        /// 当前分集.
        /// </summary>
        [Reactive]
        public EpisodeInformation CurrentEpisode { get; set; }

        /// <summary>
        /// 是否显示视频合集.
        /// </summary>
        [Reactive]
        public bool IsShowSeasons { get; set; }

        /// <summary>
        /// 是否显示关联视频.
        /// </summary>
        [Reactive]
        public bool IsShowEpisodes { get; set; }

        /// <summary>
        /// 是否显示评论区.
        /// </summary>
        [Reactive]
        public bool IsShowComments { get; set; }

        /// <summary>
        /// 是否显示附加内容.
        /// </summary>
        [Reactive]
        public bool IsShowExtras { get; set; }
    }
}

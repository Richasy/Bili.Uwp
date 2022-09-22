// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bili.Models.App.Other;
using Bili.Models.Data.Community;
using Bili.Models.Data.Local;
using Bili.Models.Data.Video;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Common;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Video
{
    /// <summary>
    /// 视频播放器页面视图模型的接口定义.
    /// </summary>
    public interface IVideoPlayerPageViewModel : IPlayerPageViewModel, IReloadViewModel, IErrorViewModel
    {
        /// <summary>
        /// 请求用户已有的收藏夹列表的命令.
        /// </summary>
        IRelayCommand RequestFavoriteFoldersCommand { get; }

        /// <summary>
        /// 请求获取实时在线观看人数的命令.
        /// </summary>
        IRelayCommand RequestOnlineCountCommand { get; }

        /// <summary>
        /// 改变视频分P的命令.
        /// </summary>
        IRelayCommand<VideoIdentifier> ChangeVideoPartCommand { get; }

        /// <summary>
        /// 搜索标签命令.
        /// </summary>
        IRelayCommand<Tag> SearchTagCommand { get; }

        /// <summary>
        /// 选中视频合集的命令.
        /// </summary>
        IRelayCommand<VideoSeason> SelectSeasonCommand { get; }

        /// <summary>
        /// 收藏视频命令.
        /// </summary>
        IRelayCommand FavoriteVideoCommand { get; }

        /// <summary>
        /// 投币命令.
        /// </summary>
        IRelayCommand<int> CoinCommand { get; }

        /// <summary>
        /// 点赞/取消点赞命令.
        /// </summary>
        IRelayCommand LikeCommand { get; }

        /// <summary>
        /// 一键三连命令.
        /// </summary>
        IRelayCommand TripleCommand { get; }

        /// <summary>
        /// 重置社区信息命令.
        /// </summary>
        IRelayCommand ReloadCommunityInformationCommand { get; }

        /// <summary>
        /// 分享命令.
        /// </summary>
        IRelayCommand ShareCommand { get; }

        /// <summary>
        /// 固定条目命令.
        /// </summary>
        IRelayCommand FixedCommand { get; }

        /// <summary>
        /// 清除播放列表命令.
        /// </summary>
        IRelayCommand ClearPlaylistCommand { get; }

        /// <summary>
        /// 清除播放数据的命令.
        /// </summary>
        IRelayCommand ClearCommand { get; }

        /// <summary>
        /// 视频协作者.
        /// </summary>
        ObservableCollection<IUserItemViewModel> Collaborators { get; }

        /// <summary>
        /// 视频标签集.
        /// </summary>
        ObservableCollection<Tag> Tags { get; }

        /// <summary>
        /// 收藏夹列表.
        /// </summary>
        ObservableCollection<IVideoFavoriteFolderSelectableViewModel> FavoriteFolders { get; }

        /// <summary>
        /// 播放时的关联区块集合.
        /// </summary>
        ObservableCollection<PlayerSectionHeader> Sections { get; }

        /// <summary>
        /// 关联的视频集合.
        /// </summary>
        ObservableCollection<IVideoItemViewModel> RelatedVideos { get; }

        /// <summary>
        /// 视频播放列表.
        /// </summary>
        ObservableCollection<IVideoItemViewModel> VideoPlaylist { get; }

        /// <summary>
        /// 视频分集集合.
        /// </summary>
        ObservableCollection<IVideoIdentifierSelectableViewModel> VideoParts { get; }

        /// <summary>
        /// 合集集合.
        /// </summary>
        ObservableCollection<VideoSeason> Seasons { get; }

        /// <summary>
        /// 当前合集下的视频列表.
        /// </summary>
        ObservableCollection<IVideoItemViewModel> CurrentSeasonVideos { get; }

        /// <summary>
        /// 下载模块视图模型.
        /// </summary>
        IDownloadModuleViewModel DownloadViewModel { get; }

        /// <summary>
        /// 视图信息.
        /// </summary>
        VideoPlayerView View { get; }

        /// <summary>
        /// 用户是否已登录.
        /// </summary>
        bool IsSignedIn { get; }

        /// <summary>
        /// 视频作者.
        /// </summary>
        IUserItemViewModel Author { get; }

        /// <summary>
        /// 是否为合作视频，<c>True</c> 则显示 <see cref="Collaborators"/>，<c>False</c> 则显示 <see cref="Author"/>.
        /// </summary>
        bool IsCooperationVideo { get; }

        /// <summary>
        /// 发布时间的可读文本.
        /// </summary>
        string PublishTime { get; }

        /// <summary>
        /// 播放次数的可读文本.
        /// </summary>
        string PlayCountText { get; }

        /// <summary>
        /// 弹幕数的可读文本.
        /// </summary>
        string DanmakuCountText { get; }

        /// <summary>
        /// 评论数的可读文本.
        /// </summary>
        string CommentCountText { get; }

        /// <summary>
        /// 正在观看人数的可读文本.
        /// </summary>
        string WatchingCountText { get; set; }

        /// <summary>
        /// 是否显示标签组.
        /// </summary>
        bool IsShowTags { get; }

        /// <summary>
        /// 点赞数的可读文本.
        /// </summary>
        string LikeCountText { get; }

        /// <summary>
        /// 投币数的可读文本.
        /// </summary>
        string CoinCountText { get; }

        /// <summary>
        /// 收藏数的可读文本.
        /// </summary>
        string FavoriteCountText { get; }

        /// <summary>
        /// 是否已点赞.
        /// </summary>
        bool IsLiked { get; set; }

        /// <summary>
        /// 是否已投币.
        /// </summary>
        bool IsCoined { get; set; }

        /// <summary>
        /// 是否已收藏.
        /// </summary>
        bool IsFavorited { get; set; }

        /// <summary>
        /// 投币同时是否点赞视频.
        /// </summary>
        bool IsCoinWithLiked { get; set; }

        /// <summary>
        /// 收藏夹列表请求是否出错.
        /// </summary>
        bool IsFavoriteFoldersError { get; set; }

        /// <summary>
        /// 收藏夹列表请求错误文本.
        /// </summary>
        string FavoriteFoldersErrorText { get; }

        /// <summary>
        /// 该视频是否已经被固定在首页.
        /// </summary>
        bool IsVideoFixed { get; }

        /// <summary>
        /// 有分集的时候是否仅显示索引.
        /// </summary>
        bool IsOnlyShowIndex { get; set; }

        /// <summary>
        /// 当前区块.
        /// </summary>
        PlayerSectionHeader CurrentSection { get; set; }

        /// <summary>
        /// 当前的视频合集.
        /// </summary>
        VideoSeason CurrentSeason { get; }

        /// <summary>
        /// 当前视频分P.
        /// </summary>
        VideoIdentifier CurrentVideoPart { get; }

        /// <summary>
        /// 是否显示视频合集.
        /// </summary>
        bool IsShowUgcSeason { get; }

        /// <summary>
        /// 是否显示关联视频.
        /// </summary>
        bool IsShowRelatedVideos { get; }

        /// <summary>
        /// 是否显示视频播放列表.
        /// </summary>
        bool IsShowVideoPlaylist { get; }

        /// <summary>
        /// 是否显示评论区.
        /// </summary>
        bool IsShowComments { get; }

        /// <summary>
        /// 是否显示视频分集.
        /// </summary>
        bool IsShowParts { get; }

        /// <summary>
        /// 是否正在请求收藏夹信息.
        /// </summary>
        bool IsFavoriteFolderRequesting { get; }

        /// <summary>
        /// 设置视频.
        /// </summary>
        /// <param name="snapshot">视频信息.</param>
        void SetSnapshot(PlaySnapshot snapshot);

        /// <summary>
        /// 设置播放列表.
        /// </summary>
        /// <param name="videos">视频列表.</param>
        /// <param name="playIndex">需要播放的视频索引.</param>
        void SetPlaylist(IEnumerable<VideoInformation> videos, int playIndex = 0);
    }
}

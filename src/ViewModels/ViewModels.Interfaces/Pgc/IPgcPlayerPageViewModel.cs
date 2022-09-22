// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.App.Other;
using Bili.Models.Data.Local;
using Bili.Models.Data.Pgc;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Video;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Pgc
{
    /// <summary>
    /// PGC 播放页面视图模型的接口定义.
    /// </summary>
    public interface IPgcPlayerPageViewModel : IPlayerPageViewModel, IReloadViewModel, IErrorViewModel
    {
        /// <summary>
        /// 是否正在请求收藏夹信息.
        /// </summary>
        bool IsFavoriteFolderRequesting { get; }

        /// <summary>
        /// 请求用户已有的收藏夹列表的命令.
        /// </summary>
        IRelayCommand RequestFavoriteFoldersCommand { get; }

        /// <summary>
        /// 改变视频分P的命令.
        /// </summary>
        IRelayCommand<EpisodeInformation> ChangeEpisodeCommand { get; }

        /// <summary>
        /// 改变剧集季度的命令.
        /// </summary>
        IRelayCommand<SeasonInformation> ChangeSeasonCommand { get; }

        /// <summary>
        /// 收藏视频命令.
        /// </summary>
        IRelayCommand FavoriteEpisodeCommand { get; }

        /// <summary>
        /// 追番/追剧命令.
        /// </summary>
        IRelayCommand TrackSeasonCommand { get; }

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
        /// 显示剧集详情的命令.
        /// </summary>
        IRelayCommand ShowSeasonDetailCommand { get; }

        /// <summary>
        /// 清空播放数据的命令.
        /// </summary>
        IRelayCommand ClearCommand { get; }

        /// <summary>
        /// 收藏夹列表.
        /// </summary>
        ObservableCollection<IVideoFavoriteFolderSelectableViewModel> FavoriteFolders { get; }

        /// <summary>
        /// 播放时的关联区块集合.
        /// </summary>
        ObservableCollection<PlayerSectionHeader> Sections { get; }

        /// <summary>
        /// 分集集合.
        /// </summary>
        ObservableCollection<IEpisodeItemViewModel> Episodes { get; }

        /// <summary>
        /// 剧集集合.
        /// </summary>
        ObservableCollection<IVideoIdentifierSelectableViewModel> Seasons { get; }

        /// <summary>
        /// 附加内容集合.
        /// </summary>
        ObservableCollection<IPgcExtraItemViewModel> Extras { get; }

        /// <summary>
        /// 演员集合.
        /// </summary>
        ObservableCollection<IUserItemViewModel> Celebrities { get; }

        /// <summary>
        /// 下载模块视图模型.
        /// </summary>
        IDownloadModuleViewModel DownloadViewModel { get; }

        /// <summary>
        /// 视图信息.
        /// </summary>
        PgcPlayerView View { get; }

        /// <summary>
        /// 用户是否已登录.
        /// </summary>
        bool IsSignedIn { get; }

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
        /// 评分人次的可读文本.
        /// </summary>
        string RatingCountText { get; }

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
        /// 是否已追番/追剧.
        /// </summary>
        bool IsTracking { get; set; }

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
        string FavoriteFoldersErrorText { get; set; }

        /// <summary>
        /// 该视频是否已经被固定在首页.
        /// </summary>
        bool IsVideoFixed { get; set; }

        /// <summary>
        /// 有分集的时候是否仅显示索引.
        /// </summary>
        bool IsOnlyShowIndex { get; set; }

        /// <summary>
        /// 是否显示演员.
        /// </summary>
        bool IsShowCelebrities { get; }

        /// <summary>
        /// 当前区块.
        /// </summary>
        PlayerSectionHeader CurrentSection { get; set; }

        /// <summary>
        /// 当前分集.
        /// </summary>
        EpisodeInformation CurrentEpisode { get; set; }

        /// <summary>
        /// 是否显示视频合集.
        /// </summary>
        bool IsShowSeasons { get; }

        /// <summary>
        /// 是否显示关联视频.
        /// </summary>
        bool IsShowEpisodes { get; }

        /// <summary>
        /// 是否显示评论区.
        /// </summary>
        bool IsShowComments { get; }

        /// <summary>
        /// 是否显示附加内容.
        /// </summary>
        bool IsShowExtras { get; }

        /// <summary>
        /// 可选分区是否为空.
        /// </summary>
        bool IsSectionsEmpty { get; }

        /// <summary>
        /// 设置视频.
        /// </summary>
        /// <param name="snapshot">视频信息.</param>
        void SetSnapshot(PlaySnapshot snapshot);
    }
}

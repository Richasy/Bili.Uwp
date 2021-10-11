// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bilibili.App.View.V1;
using FFmpegInterop;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Controller.Uwp.Interfaces;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.Media.Playback;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 播放器视图模型.
    /// </summary>
    public partial class PlayerViewModel
    {
        private readonly INumberToolkit _numberToolkit;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly IFileToolkit _fileToolkit;
        private readonly ILoggerModule _logger;

        private readonly FFmpegInteropConfig _liveFFConfig;

        private long _videoId;
        private ViewReply _videoDetail;
        private PgcDisplayInformation _pgcDetail;
        private LiveRoomDetail _liveDetail;
        private PlayerInformation _playerInformation;

        private TimeSpan _lastReportProgress;
        private VideoType _videoType;
        private TimeSpan _initializeProgress;
        private FFmpegInteropMSS _interopMSS;
        private MediaPlaybackItem _currentPlaybackItem;

        private DashItem _currentAudio;
        private DashItem _currentVideo;

        private List<DashItem> _audioList;
        private List<DashItem> _videoList;
        private List<SubtitleItem> _subtitleList;

        private MediaPlayer _currentVideoPlayer;

        private DispatcherTimer _progressTimer;
        private DispatcherTimer _heartBeatTimer;
        private DispatcherTimer _subtitleTimer;

        private long _interactionPartId;
        private long _interactionNodeId;
        private bool _isInteractionChanging;
        private InteractionEdgeResponse _interactionDetail;

        /// <summary>
        /// 让直播消息视图滚动到底部.
        /// </summary>
        public event EventHandler RequestLiveMessageScrollToBottom;

        /// <summary>
        /// 有新的直播弹幕添加.
        /// </summary>
        public event EventHandler<LiveDanmakuMessage> NewLiveDanmakuAdded;

        /// <summary>
        /// 单例.
        /// </summary>
        public static PlayerViewModel Instance { get; } = new Lazy<PlayerViewModel>(() => new PlayerViewModel()).Value;

        /// <summary>
        /// 应用视频播放器.
        /// </summary>
        public MediaPlayerElement BiliPlayer { get; private set; }

        /// <summary>
        /// 经典播放器.
        /// </summary>
        public MediaElement ClassicPlayer { get; private set; }

        /// <summary>
        /// 偏好的解码模式.
        /// </summary>
        public PreferCodec PreferCodec => SettingViewModel.Instance.PreferCodec;

        /// <summary>
        /// 是否自动播放.
        /// </summary>
        public bool IsAutoPlay => SettingViewModel.Instance.IsAutoPlayWhenLoaded;

        /// <summary>
        /// 初始选中的分区.
        /// </summary>
        public string InitializeSection { get; set; } = string.Empty;

        /// <summary>
        /// 调度器.
        /// </summary>
        public CoreDispatcher Dispatcher { get; set; }

        /// <summary>
        /// 详情是否可以加载（用于优化页面跳转的加载时间）.
        /// </summary>
        [Reactive]
        public bool IsDetailCanLoaded { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <summary>
        /// 副标题，发布时间.
        /// </summary>
        [Reactive]
        public string Subtitle { get; set; }

        /// <summary>
        /// 说明.
        /// </summary>
        [Reactive]
        public string Description { get; set; }

        /// <summary>
        /// AV Id.
        /// </summary>
        [Reactive]
        public string AvId { get; set; }

        /// <summary>
        /// BV Id.
        /// </summary>
        [Reactive]
        public string BvId { get; set; }

        /// <summary>
        /// PGC单集Id.
        /// </summary>
        [Reactive]
        public string EpisodeId { get; set; }

        /// <summary>
        /// PGC剧集/系列Id.
        /// </summary>
        [Reactive]
        public string SeasonId { get; set; }

        /// <summary>
        /// 直播间Id.
        /// </summary>
        [Reactive]
        public string RoomId { get; set; }

        /// <summary>
        /// 播放数.
        /// </summary>
        [Reactive]
        public string PlayCount { get; set; }

        /// <summary>
        /// 弹幕数.
        /// </summary>
        [Reactive]
        public string DanmakuCount { get; set; }

        /// <summary>
        /// 点赞数.
        /// </summary>
        [Reactive]
        public string LikeCount { get; set; }

        /// <summary>
        /// 硬币数.
        /// </summary>
        [Reactive]
        public string CoinCount { get; set; }

        /// <summary>
        /// 收藏数.
        /// </summary>
        [Reactive]
        public string FavoriteCount { get; set; }

        /// <summary>
        /// 转发数.
        /// </summary>
        [Reactive]
        public string ShareCount { get; set; }

        /// <summary>
        /// 评论数.
        /// </summary>
        [Reactive]
        public string ReplyCount { get; set; }

        /// <summary>
        /// 在线观看人数.
        /// </summary>
        [Reactive]
        public string ViewerCount { get; set; }

        /// <summary>
        /// 播放器地址.
        /// </summary>
        [Reactive]
        public string CoverUrl { get; set; }

        /// <summary>
        /// 发布者.
        /// </summary>
        [Reactive]
        public UserViewModel Publisher { get; set; }

        /// <summary>
        /// 是否显示为参演人员列表.
        /// </summary>
        [Reactive]
        public bool IsShowStaff { get; set; }

        /// <summary>
        /// 音量.
        /// </summary>
        [Reactive]
        public double Volume { get; set; }

        /// <summary>
        /// 参演人员集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<UserViewModel> StaffCollection { get; set; }

        /// <summary>
        /// 关联视频集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<VideoViewModel> RelatedVideoCollection { get; set; }

        /// <summary>
        /// 分集视频集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<VideoPartViewModel> VideoPartCollection { get; set; }

        /// <summary>
        /// 视频清晰度集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<VideoFormatViewModel> FormatCollection { get; set; }

        /// <summary>
        /// 直播清晰度集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<LiveQualityViewModel> LiveQualityCollection { get; set; }

        /// <summary>
        /// 直播播放线路集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<LivePlayLineViewModel> LivePlayLineCollection { get; set; }

        /// <summary>
        /// PGC区块（比如PV）集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<PgcSectionViewModel> PgcSectionCollection { get; set; }

        /// <summary>
        /// PGC分集集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<PgcEpisodeViewModel> EpisodeCollection { get; set; }

        /// <summary>
        /// PGC剧集/系列集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<PgcSeasonViewModel> SeasonCollection { get; set; }

        /// <summary>
        /// 直播弹幕集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<LiveDanmakuMessage> LiveDanmakuCollection { get; set; }

        /// <summary>
        /// 收藏夹集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<FavoriteMetaViewModel> FavoriteMetaCollection { get; set; }

        /// <summary>
        /// 当前分P.
        /// </summary>
        [Reactive]
        public ViewPage CurrentVideoPart { get; set; }

        /// <summary>
        /// 当前分集.
        /// </summary>
        [Reactive]
        public PgcEpisodeDetail CurrentPgcEpisode { get; set; }

        /// <summary>
        /// 当前清晰度.
        /// </summary>
        [Reactive]
        public VideoFormat CurrentFormat { get; set; }

        /// <summary>
        /// 当前直播播放线路.
        /// </summary>
        [Reactive]
        public LivePlayLine CurrentPlayLine { get; set; }

        /// <summary>
        /// 当前直播清晰度.
        /// </summary>
        [Reactive]
        public LiveQualityDescription CurrentLiveQuality { get; set; }

        /// <summary>
        /// 是否正在加载.
        /// </summary>
        [Reactive]
        public bool IsDetailLoading { get; set; }

        /// <summary>
        /// 是否出错.
        /// </summary>
        [Reactive]
        public bool IsDetailError { get; set; }

        /// <summary>
        /// 错误文本.
        /// </summary>
        [Reactive]
        public string DetailErrorText { get; set; }

        /// <summary>
        /// 播放信息是否正在加载中.
        /// </summary>
        [Reactive]
        public bool IsPlayInformationLoading { get; set; }

        /// <summary>
        /// 是否在请求播放信息的过程中出错.
        /// </summary>
        [Reactive]
        public bool IsPlayInformationError { get; set; }

        /// <summary>
        /// 请求播放信息出错的错误文本.
        /// </summary>
        [Reactive]
        public string PlayInformationErrorText { get; set; }

        /// <summary>
        /// 播放器显示模式.
        /// </summary>
        [Reactive]
        public PlayerDisplayMode PlayerDisplayMode { get; set; }

        /// <summary>
        /// PGC动态标签页名.
        /// </summary>
        [Reactive]
        public string PgcActivityTab { get; set; }

        /// <summary>
        /// 是否显示PGC动态标签页.
        /// </summary>
        [Reactive]
        public bool IsShowPgcActivityTab { get; set; }

        /// <summary>
        /// 是否显示分P.
        /// </summary>
        [Reactive]
        public bool IsShowParts { get; set; }

        /// <summary>
        /// 是否显示关联视频.
        /// </summary>
        [Reactive]
        public bool IsShowRelatedVideos { get; set; }

        /// <summary>
        /// 是否显示系列.
        /// </summary>
        [Reactive]
        public bool IsShowSeason { get; set; }

        /// <summary>
        /// 是否显示分集列表.
        /// </summary>
        [Reactive]
        public bool IsShowEpisode { get; set; }

        /// <summary>
        /// 是否显示区块.
        /// </summary>
        [Reactive]
        public bool IsShowSection { get; set; }

        /// <summary>
        /// 是否显示聊天室.
        /// </summary>
        [Reactive]
        public bool IsShowChat { get; set; }

        /// <summary>
        /// 是否回复.
        /// </summary>
        [Reactive]
        public bool IsShowReply { get; set; }

        /// <summary>
        /// 点赞按钮是否被选中.
        /// </summary>
        [Reactive]
        public bool IsLikeChecked { get; set; }

        /// <summary>
        /// 投币按钮是否被选中.
        /// </summary>
        [Reactive]
        public bool IsCoinChecked { get; set; }

        /// <summary>
        /// 是否允许长按点赞.
        /// </summary>
        [Reactive]
        public bool IsEnableLikeHolding { get; set; }

        /// <summary>
        /// 收藏按钮是否被选中.
        /// </summary>
        [Reactive]
        public bool IsFavoriteChecked { get; set; }

        /// <summary>
        /// 是否已追番/关注.
        /// </summary>
        [Reactive]
        public bool IsFollow { get; set; }

        /// <summary>
        /// 是否为PGC内容.
        /// </summary>
        [Reactive]
        public bool IsPgc { get; set; }

        /// <summary>
        /// 是否为直播.
        /// </summary>
        [Reactive]
        public bool IsLive { get; set; }

        /// <summary>
        /// 是否是互动视频.
        /// </summary>
        [Reactive]
        public bool IsInteraction { get; set; }

        /// <summary>
        /// 播放器状态.
        /// </summary>
        [Reactive]
        public PlayerStatus PlayerStatus { get; set; }

        /// <summary>
        /// 是否显示直播消息空白占位.
        /// </summary>
        [Reactive]
        public bool IsShowEmptyLiveMessage { get; set; }

        /// <summary>
        /// 当前的分集是否在PGC关联内容里（比如PV）.
        /// </summary>
        [Reactive]
        public bool IsCurrentEpisodeInPgcSection { get; set; }

        /// <summary>
        /// 显示的播放历史文本.
        /// </summary>
        [Reactive]
        public string HistoryText { get; set; }

        /// <summary>
        /// 是否显示播放历史跳转提醒.
        /// </summary>
        [Reactive]
        public bool IsShowHistory { get; set; }

        /// <summary>
        /// 请求收藏夹出错.
        /// </summary>
        [Reactive]
        public bool IsRequestFavoritesError { get; set; }

        /// <summary>
        /// 是否正在请求收藏夹列表.
        /// </summary>
        [Reactive]
        public bool IsRequestingFavorites { get; set; }

        /// <summary>
        /// 是否显示徽章文本.
        /// </summary>
        [Reactive]
        public bool IsShowBadge { get; set; }

        /// <summary>
        /// 徽章文本.
        /// </summary>
        [Reactive]
        public string BadgeText { get; set; }

        /// <summary>
        /// 是否显示演职人员.
        /// </summary>
        [Reactive]
        public bool IsShowCelebrity { get; set; }

        /// <summary>
        /// 演职人员.
        /// </summary>
        [Reactive]
        public ObservableCollection<PgcCelebrity> CelebrityCollection { get; set; }

        /// <summary>
        /// 发布日期.
        /// </summary>
        [Reactive]
        public string DisplayProgress { get; set; }

        /// <summary>
        /// 发布时间.
        /// </summary>
        [Reactive]
        public string PublishDate { get; set; }

        /// <summary>
        /// 是否显示原名.
        /// </summary>
        [Reactive]
        public bool IsShowOriginName { get; set; }

        /// <summary>
        /// 原名.
        /// </summary>
        [Reactive]
        public string OriginName { get; set; }

        /// <summary>
        /// 是否显示别名.
        /// </summary>
        [Reactive]
        public bool IsShowAlias { get; set; }

        /// <summary>
        /// 别名.
        /// </summary>
        [Reactive]
        public string Alias { get; set; }

        /// <summary>
        /// 是否显示演员信息.
        /// </summary>
        [Reactive]
        public bool IsShowActor { get; set; }

        /// <summary>
        /// 声优/演员标题.
        /// </summary>
        [Reactive]
        public string ActorTitle { get; set; }

        /// <summary>
        /// 声优/演员详情.
        /// </summary>
        [Reactive]
        public string ActorInformation { get; set; }

        /// <summary>
        /// 是否显示制作信息.
        /// </summary>
        [Reactive]
        public bool IsShowEditor { get; set; }

        /// <summary>
        /// 制作信息标题.
        /// </summary>
        [Reactive]
        public string EditorTitle { get; set; }

        /// <summary>
        /// 制作信息详情.
        /// </summary>
        [Reactive]
        public string EditorInformation { get; set; }

        /// <summary>
        /// 评价/简介.
        /// </summary>
        [Reactive]
        public string Evaluate { get; set; }

        /// <summary>
        /// PGC内容类型.
        /// </summary>
        [Reactive]
        public string PgcTypeName { get; set; }

        /// <summary>
        /// 评分.
        /// </summary>
        [Reactive]
        public double Rating { get; set; }

        /// <summary>
        /// 评分人数.
        /// </summary>
        [Reactive]
        public string RatedCount { get; set; }

        /// <summary>
        /// 是否显示评分.
        /// </summary>
        [Reactive]
        public bool IsShowRating { get; set; }

        /// <summary>
        /// 直播分区.
        /// </summary>
        [Reactive]
        public string LivePartition { get; set; }

        /// <summary>
        /// 直播消息是否自动滚动.
        /// </summary>
        [Reactive]
        public bool IsLiveMessageAutoScroll { get; set; }

        /// <summary>
        /// 是否显示弹幕条.
        /// </summary>
        [Reactive]
        public bool IsShowDanmakuBar { get; set; }

        /// <summary>
        /// 当前字幕.
        /// </summary>
        [Reactive]
        public string CurrentSubtitle { get; set; }

        /// <summary>
        /// 当前字幕索引.
        /// </summary>
        [Reactive]
        public SubtitleIndexItem CurrentSubtitleIndex { get; set; }

        /// <summary>
        /// 是否显示字幕.
        /// </summary>
        [Reactive]
        public bool IsShowSubtitle { get; set; }

        /// <summary>
        /// 是否显示字幕按钮.
        /// </summary>
        [Reactive]
        public bool IsShowSubtitleButton { get; set; }

        /// <summary>
        /// 是否可以显示字幕.
        /// </summary>
        [Reactive]
        public bool CanShowSubtitle { get; set; }

        /// <summary>
        /// 是否显示选项.
        /// </summary>
        [Reactive]
        public bool IsShowChoice { get; set; }

        /// <summary>
        /// 是否显示互动视频结束.
        /// </summary>
        [Reactive]
        public bool IsShowInteractionEnd { get; set; }

        /// <summary>
        /// 索引列表.
        /// </summary>
        [Reactive]
        public ObservableCollection<SubtitleIndexItemViewModel> SubtitleIndexCollection { get; set; }

        /// <summary>
        /// 播放速率.
        /// </summary>
        [Reactive]
        public double PlaybackRate { get; set; }

        /// <summary>
        /// 选项集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<InteractionChoice> ChoiceCollection { get; set; }

        private BiliController Controller { get; } = BiliController.Instance;
    }
}

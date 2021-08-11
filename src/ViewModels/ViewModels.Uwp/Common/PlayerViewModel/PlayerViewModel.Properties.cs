// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bilibili.App.View.V1;
using FFmpegInterop;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;
using Windows.Media.Playback;
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

        private long _videoId;
        private ViewReply _videoDetail;
        private PgcDisplayInformation _pgcDetail;
        private PlayerDashInformation _dashInformation;
        private LivePlayInformation _livePlayInformation;
        private TimeSpan _lastReportProgress;
        private VideoType _videoType;
        private TimeSpan _initializeProgress;
        private FFmpegInteropConfig _liveFFConfig;

        private DashItem _currentAudio;
        private DashItem _currentVideo;

        private List<DashItem> _audioList;
        private List<DashItem> _videoList;

        private MediaPlayer _currentVideoPlayer;
        private MediaPlayer _currentAudioPlayer;

        private DispatcherTimer _progressTimer;

        /// <summary>
        /// 单例.
        /// </summary>
        public static PlayerViewModel Instance { get; } = new Lazy<PlayerViewModel>(() => new PlayerViewModel()).Value;

        /// <summary>
        /// 应用视频播放器.
        /// </summary>
        public MediaPlayerElement BiliPlayer { get; private set; }

        /// <summary>
        /// 偏好的解码模式.
        /// </summary>
        public PreferCodec PreferCodec => SettingViewModel.Instance.PreferCodec;

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
        /// 播放器地址.
        /// </summary>
        [Reactive]
        public string CoverUrl { get; set; }

        /// <summary>
        /// 发布者.
        /// </summary>
        [Reactive]
        public PublisherViewModel Publisher { get; set; }

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
        /// 直播线路集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<LivePlayLineViewModel> PlayLineCollection { get; set; }

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
        /// 当前播放线路.
        /// </summary>
        [Reactive]
        public LivePlayLine CurrentPlayLine { get; set; }

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
        /// 当前的分集是否在PGC关联内容里（比如PV）.
        /// </summary>
        [Reactive]
        public bool IsCurrentEpisodeInPgcSection { get; set; }

        /// <summary>
        /// 是否显示直播线路切换.
        /// </summary>
        [Reactive]
        public bool IsShowLivePlayLine { get; set; }

        private BiliController Controller { get; } = BiliController.Instance;
    }
}

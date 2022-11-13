// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Community;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Pgc;
using Bili.ViewModels.Interfaces.Video;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Desktop.Base
{
    /// <summary>
    /// 动漫页面视图模型基类.
    /// </summary>
    public partial class AnimePageViewModelBase
    {
        private readonly IAuthorizeProvider _authorizeProvider;
        private readonly IPgcProvider _pgcProvider;
        private readonly IHomeProvider _homeProvider;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly INavigationViewModel _navigationViewModel;
        private readonly Dictionary<Partition, PgcPageView> _viewCaches;
        private readonly Dictionary<string, IEnumerable<VideoInformation>> _videoCaches;
        private readonly PgcType _type;

        private string _currentVideoPartitionId;

        [ObservableProperty]
        private Partition _currentPartition;

        [ObservableProperty]
        private bool _isLoggedIn;

        [ObservableProperty]
        private bool _isShowRank;

        [ObservableProperty]
        private bool _isShowPlaylist;

        [ObservableProperty]
        private bool _isShowBanner;

        [ObservableProperty]
        private bool _isShowVideo;

        [ObservableProperty]
        private string _title;

        [ObservableProperty]
        private bool _isError;

        [ObservableProperty]
        private string _errorText;

        [ObservableProperty]
        private bool _isReloading;

        [ObservableProperty]
        private bool _isIncrementalLoading;

        /// <summary>
        /// 顶部分区集合.
        /// </summary>
        public ObservableCollection<Partition> Partitions { get; }

        /// <summary>
        /// 横幅集合.
        /// </summary>
        public ObservableCollection<IBannerViewModel> Banners { get; }

        /// <summary>
        /// 排行榜集合.
        /// </summary>
        public ObservableCollection<IPgcRankViewModel> Ranks { get; }

        /// <summary>
        /// 播放列表集合.
        /// </summary>
        public ObservableCollection<IPgcPlaylistViewModel> Playlists { get; }

        /// <summary>
        /// 相关的视频集合.
        /// </summary>
        public ObservableCollection<IVideoItemViewModel> Videos { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand InitializeCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand IncrementalCommand { get; }

        /// <summary>
        /// 前往追番页面的命令.
        /// </summary>
        public IRelayCommand GotoFavoritePageCommand { get; }

        /// <summary>
        /// 前往动漫索引页面的命令.
        /// </summary>
        public IRelayCommand GotoIndexPageCommand { get; }

        /// <summary>
        /// 前往动漫时间线页面的命令.
        /// </summary>
        public IRelayCommand GotoTimeLinePageCommand { get; }

        /// <summary>
        /// 选择分区命令.
        /// </summary>
        public IAsyncRelayCommand<Partition> SelectPartitionCommand { get; }
    }
}

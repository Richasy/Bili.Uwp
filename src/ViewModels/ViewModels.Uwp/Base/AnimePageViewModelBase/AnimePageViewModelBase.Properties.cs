// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
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
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Base
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
        public IRelayCommand ReloadCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand InitializeCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand IncrementalCommand { get; }

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
        public IRelayCommand<Partition> SelectPartitionCommand { get; }

        /// <summary>
        /// 当前选中标签.
        /// </summary>
        [ObservableProperty]
        public Partition CurrentPartition { get; set; }

        /// <summary>
        /// 是否显示我的追番按钮.
        /// </summary>
        [ObservableProperty]
        public bool IsLoggedIn { get; set; }

        /// <summary>
        /// 是否显示排行榜.
        /// </summary>
        [ObservableProperty]
        public bool IsShowRank { get; set; }

        /// <summary>
        /// 是否显示播放列表.
        /// </summary>
        [ObservableProperty]
        public bool IsShowPlaylist { get; set; }

        /// <summary>
        /// 是否显示横幅.
        /// </summary>
        [ObservableProperty]
        public bool IsShowBanner { get; set; }

        /// <summary>
        /// 是否显示视频.
        /// </summary>
        [ObservableProperty]
        public bool IsShowVideo { get; set; }

        /// <summary>
        /// 页面标题.
        /// </summary>
        [ObservableProperty]
        public string Title { get; set; }

        /// <summary>
        /// 是否出错.
        /// </summary>
        [ObservableProperty]
        public bool IsError { get; private set; }

        /// <summary>
        /// 错误文本.
        /// </summary>
        [ObservableProperty]
        public string ErrorText { get; set; }

        /// <summary>
        /// 是否正在重载.
        /// </summary>
        [ObservableAsProperty]
        public bool IsReloading { get; set; }

        /// <inheritdoc/>
        [ObservableAsProperty]
        public bool IsIncrementalLoading { get; set; }
    }
}

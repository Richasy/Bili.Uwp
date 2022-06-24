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
using Bili.ViewModels.Uwp.Core;
using Bili.ViewModels.Uwp.Pgc;
using Bili.ViewModels.Uwp.Video;
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
        private readonly NavigationViewModel _navigationViewModel;
        private readonly Dictionary<Partition, PgcPageView> _viewCaches;
        private readonly Dictionary<string, IEnumerable<VideoInformation>> _videoCaches;
        private readonly ObservableAsPropertyHelper<bool> _isReloading;
        private readonly ObservableAsPropertyHelper<bool> _isIncrementalLoading;
        private readonly PgcType _type;

        private string _currentVideoPartitionId;

        /// <summary>
        /// 顶部分区集合.
        /// </summary>
        public ObservableCollection<Partition> Partitions { get; }

        /// <summary>
        /// 横幅集合.
        /// </summary>
        public ObservableCollection<BannerViewModel> Banners { get; }

        /// <summary>
        /// 排行榜集合.
        /// </summary>
        public ObservableCollection<PgcRankViewModel> Ranks { get; }

        /// <summary>
        /// 播放列表集合.
        /// </summary>
        public ObservableCollection<PgcPlaylistViewModel> Playlists { get; }

        /// <summary>
        /// 相关的视频集合.
        /// </summary>
        public ObservableCollection<VideoItemViewModel> Videos { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> InitializeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> IncrementalCommand { get; }

        /// <summary>
        /// 前往追番页面的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> GotoFavoritePageCommand { get; }

        /// <summary>
        /// 前往动漫索引页面的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> GotoIndexPageCommand { get; }

        /// <summary>
        /// 前往动漫时间线页面的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> GotoTimeLinePageCommand { get; }

        /// <summary>
        /// 选择分区命令.
        /// </summary>
        public ReactiveCommand<Partition, Unit> SelectPartitionCommand { get; }

        /// <summary>
        /// 当前选中标签.
        /// </summary>
        [Reactive]
        public Partition CurrentPartition { get; set; }

        /// <summary>
        /// 是否显示我的追番按钮.
        /// </summary>
        [Reactive]
        public bool IsLoggedIn { get; set; }

        /// <summary>
        /// 是否显示排行榜.
        /// </summary>
        [Reactive]
        public bool IsShowRank { get; set; }

        /// <summary>
        /// 是否显示播放列表.
        /// </summary>
        [Reactive]
        public bool IsShowPlaylist { get; set; }

        /// <summary>
        /// 是否显示横幅.
        /// </summary>
        [Reactive]
        public bool IsShowBanner { get; set; }

        /// <summary>
        /// 是否显示视频.
        /// </summary>
        [Reactive]
        public bool IsShowVideo { get; set; }

        /// <summary>
        /// 页面标题.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <summary>
        /// 是否出错.
        /// </summary>
        [Reactive]
        public bool IsError { get; private set; }

        /// <summary>
        /// 错误文本.
        /// </summary>
        [Reactive]
        public string ErrorText { get; set; }

        /// <summary>
        /// 是否正在重载.
        /// </summary>
        public bool IsReloading => _isReloading.Value;

        /// <inheritdoc/>
        public bool IsIncrementalLoading => _isIncrementalLoading.Value;
    }
}

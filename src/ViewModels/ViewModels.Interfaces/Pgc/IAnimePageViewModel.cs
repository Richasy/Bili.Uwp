// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Models.Data.Community;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Video;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces.Pgc
{
    /// <summary>
    /// 动漫页面视图模型基类的接口定义.
    /// </summary>
    public interface IAnimePageViewModel : IReactiveObject, IInitializeViewModel, IReloadViewModel, IIncrementalViewModel, IErrorViewModel
    {
        /// <summary>
        /// 顶部分区集合.
        /// </summary>
        ObservableCollection<Partition> Partitions { get; }

        /// <summary>
        /// 横幅集合.
        /// </summary>
        ObservableCollection<IBannerViewModel> Banners { get; }

        /// <summary>
        /// 排行榜集合.
        /// </summary>
        ObservableCollection<IPgcRankViewModel> Ranks { get; }

        /// <summary>
        /// 播放列表集合.
        /// </summary>
        ObservableCollection<IPgcPlaylistViewModel> Playlists { get; }

        /// <summary>
        /// 相关的视频集合.
        /// </summary>
        ObservableCollection<IVideoItemViewModel> Videos { get; }

        /// <summary>
        /// 前往追番页面的命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> GotoFavoritePageCommand { get; }

        /// <summary>
        /// 前往动漫索引页面的命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> GotoIndexPageCommand { get; }

        /// <summary>
        /// 前往动漫时间线页面的命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> GotoTimeLinePageCommand { get; }

        /// <summary>
        /// 选择分区命令.
        /// </summary>
        ReactiveCommand<Partition, Unit> SelectPartitionCommand { get; }

        /// <summary>
        /// 当前选中标签.
        /// </summary>
        Partition CurrentPartition { get; }

        /// <summary>
        /// 是否显示我的追番按钮.
        /// </summary>
        bool IsLoggedIn { get; }

        /// <summary>
        /// 是否显示排行榜.
        /// </summary>
        bool IsShowRank { get; }

        /// <summary>
        /// 是否显示播放列表.
        /// </summary>
        bool IsShowPlaylist { get; }

        /// <summary>
        /// 是否显示横幅.
        /// </summary>
        bool IsShowBanner { get; }

        /// <summary>
        /// 是否显示视频.
        /// </summary>
        bool IsShowVideo { get; }

        /// <summary>
        /// 页面标题.
        /// </summary>
        string Title { get; }
    }
}

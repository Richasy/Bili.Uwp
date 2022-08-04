// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Models.Data.Pgc;
using ReactiveUI;

namespace Bili.ViewModels.Interfaces.Pgc
{
    /// <summary>
    /// PGC 播放列表的视图模型接口定义.
    /// </summary>
    public interface IPgcPlaylistViewModel : IInjectDataViewModel<PgcPlaylist>, IInitializeViewModel, IReloadViewModel, IErrorViewModel
    {
        /// <summary>
        /// 副标题.
        /// </summary>
        string Subtitle { get; }

        /// <summary>
        /// 是否显示详情按钮.
        /// </summary>
        bool IsShowDetailButton { get; }

        /// <summary>
        /// 剧集集合.
        /// </summary>
        ObservableCollection<ISeasonItemViewModel> Seasons { get; }

        /// <summary>
        /// 显示更多的命令.
        /// </summary>
        ReactiveCommand<Unit, Unit> ShowMoreCommand { get; }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Pgc;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC 的播放列表视图模型.
    /// </summary>
    public sealed partial class PgcPlaylistViewModel
    {
        private readonly AppViewModel _appViewModel;
        private readonly ObservableAsPropertyHelper<bool> _isReloading;
        private readonly IPgcProvider _pgcProvider;
        private readonly IResourceToolkit _resourceToolkit;

        /// <summary>
        /// 初始数据.
        /// </summary>
        [Reactive]
        public PgcPlaylist Data { get; set; }

        /// <summary>
        /// 副标题.
        /// </summary>
        [Reactive]
        public string Subtitle { get; set; }

        /// <summary>
        /// 是否显示详情按钮.
        /// </summary>
        [Reactive]
        public bool IsShowDetailButton { get; set; }

        /// <summary>
        /// 是否错误.
        /// </summary>
        [Reactive]
        public bool IsError { get; set; }

        /// <summary>
        /// 错误文本.
        /// </summary>
        [Reactive]
        public string ErrorText { get; set; }

        /// <summary>
        /// 剧集集合.
        /// </summary>
        public ObservableCollection<SeasonItemViewModel> Seasons { get; }

        /// <summary>
        /// 显示更多的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> ShowMoreCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> InitializeCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> ReloadCommand { get; }

        /// <inheritdoc/>
        public bool IsReloading => _isReloading.Value;
    }
}

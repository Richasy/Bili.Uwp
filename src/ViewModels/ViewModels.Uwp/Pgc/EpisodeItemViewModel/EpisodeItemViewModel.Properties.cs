// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Pgc;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// 剧集单集视图模型.
    /// </summary>
    public sealed partial class EpisodeItemViewModel
    {
        private readonly INumberToolkit _numberToolkit;
        private readonly INavigationViewModel _navigationViewModel;

        [ObservableProperty]
        private EpisodeInformation _data;

        [ObservableProperty]
        private string _playCountText;

        [ObservableProperty]
        private string _danmakuCountText;

        [ObservableProperty]
        private string _trackCountText;

        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private string _durationText;

        /// <summary>
        /// 在网页中打开的命令.
        /// </summary>
        public IAsyncRelayCommand OpenInBroswerCommand { get; }

        /// <summary>
        /// 播放命令.
        /// </summary>
        public IAsyncRelayCommand PlayCommand { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is EpisodeItemViewModel model && EqualityComparer<EpisodeInformation>.Default.Equals(Data, model.Data);

        /// <inheritdoc/>
        public override int GetHashCode() => Data.GetHashCode();
    }
}

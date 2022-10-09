// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.Data.Live;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Uwp.Live
{
    /// <summary>
    /// 直播间条目视图模型.
    /// </summary>
    public sealed partial class LiveItemViewModel
    {
        private readonly INumberToolkit _numberToolkit;
        private readonly INavigationViewModel _navigationViewModel;

        [ObservableProperty]
        private LiveInformation _data;

        [ObservableProperty]
        private string _viewerCountText;

        /// <summary>
        /// 在网页中打开的命令.
        /// </summary>
        public IAsyncRelayCommand OpenInBroswerCommand { get; }

        /// <summary>
        /// 播放命令.
        /// </summary>
        public IRelayCommand PlayCommand { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is LiveItemViewModel model && EqualityComparer<LiveInformation>.Default.Equals(Data, model.Data);

        /// <inheritdoc/>
        public override int GetHashCode() => Data.GetHashCode();
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Pgc;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Pgc;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// 剧集条目视图模型.
    /// </summary>
    public sealed partial class SeasonItemViewModel
    {
        private readonly INumberToolkit _numberToolkit;
        private readonly IFavoriteProvider _favoriteProvider;
        private readonly INavigationViewModel _navigationViewModel;
        private Action<ISeasonItemViewModel> _additionalAction;

        /// <inheritdoc/>
        public IRelayCommand OpenInBroswerCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand PlayCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand UnfollowCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand<int> ChangeFavoriteStatusCommand { get; }

        /// <inheritdoc/>
        [ObservableProperty]
        public SeasonInformation Data { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsSelected { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public bool IsShowRating { get; set; }

        /// <inheritdoc/>
        [ObservableProperty]
        public string TrackCountText { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SeasonItemViewModel model && EqualityComparer<SeasonInformation>.Default.Equals(Data, model.Data);

        /// <inheritdoc/>
        public override int GetHashCode() => Data.GetHashCode();
    }
}

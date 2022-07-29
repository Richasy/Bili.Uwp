// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Pgc;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Pgc;
using Bili.ViewModels.Uwp.Core;
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
        private readonly NavigationViewModel _navigationViewModel;
        private Action<ISeasonItemViewModel> _additionalAction;

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> OpenInBroswerCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> PlayCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<Unit, Unit> UnfollowCommand { get; }

        /// <inheritdoc/>
        public ReactiveCommand<int, Unit> ChangeFavoriteStatusCommand { get; }

        /// <inheritdoc/>
        [Reactive]
        public SeasonInformation Data { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsSelected { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public bool IsShowRating { get; set; }

        /// <inheritdoc/>
        [Reactive]
        public string TrackCountText { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SeasonItemViewModel model && EqualityComparer<SeasonInformation>.Default.Equals(Data, model.Data);

        /// <inheritdoc/>
        public override int GetHashCode() => Data.GetHashCode();
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Pgc;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Pgc;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

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

        [ObservableProperty]
        private SeasonInformation _data;

        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private bool _isShowRating;

        [ObservableProperty]
        private string _trackCountText;

        /// <inheritdoc/>
        public IAsyncRelayCommand OpenInBroswerCommand { get; }

        /// <inheritdoc/>
        public IRelayCommand PlayCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand UnfollowCommand { get; }

        /// <inheritdoc/>
        public IAsyncRelayCommand<int> ChangeFavoriteStatusCommand { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SeasonItemViewModel model && EqualityComparer<SeasonInformation>.Default.Equals(Data, model.Data);

        /// <inheritdoc/>
        public override int GetHashCode() => Data.GetHashCode();
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Pgc;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Pgc;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using Windows.System;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// 剧集条目视图模型.
    /// </summary>
    public sealed partial class SeasonItemViewModel : ViewModelBase, ISeasonItemViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SeasonItemViewModel"/> class.
        /// </summary>
        public SeasonItemViewModel(
            INumberToolkit numberToolkit,
            IFavoriteProvider favoriteProvider,
            NavigationViewModel navigationViewModel)
        {
            _numberToolkit = numberToolkit;
            _favoriteProvider = favoriteProvider;
            _navigationViewModel = navigationViewModel;

            OpenInBroswerCommand = ReactiveCommand.CreateFromTask(OpenInBroswerAsync);
            PlayCommand = ReactiveCommand.Create(Play);
            UnfollowCommand = ReactiveCommand.CreateFromTask(UnfollowAsync);
            ChangeFavoriteStatusCommand = ReactiveCommand.CreateFromTask<int>(ChangeFavoriteStatusAsync);
        }

        /// <inheritdoc/>
        public void InjectData(SeasonInformation information)
        {
            Data = information;
            InitializeData();
        }

        /// <inheritdoc/>
        public void InjectAction(Action<ISeasonItemViewModel> action)
            => _additionalAction = action;

        private void InitializeData()
        {
            IsShowRating = Data.CommunityInformation?.Score > 0;

            if (Data.CommunityInformation?.TrackCount > 0)
            {
                TrackCountText = _numberToolkit.GetCountText(Data.CommunityInformation.TrackCount);
            }
        }

        private void Play()
            => _navigationViewModel.NavigateToPlayView(new Models.Data.Local.PlaySnapshot(default, Data.Identifier.Id, Models.Enums.VideoType.Pgc)
            {
                Title = Data.Identifier.Title,
            });

        private async Task OpenInBroswerAsync()
        {
            var uri = $"https://www.bilibili.com/bangumi/play/ss{Data.Identifier.Id}";
            await Launcher.LaunchUriAsync(new Uri(uri));
        }

        private async Task UnfollowAsync()
        {
            var result = await _favoriteProvider.RemoveFavoritePgcAsync(Data.Identifier.Id);
            if (result)
            {
                _additionalAction?.Invoke(this);
            }
        }

        private async Task ChangeFavoriteStatusAsync(int status)
        {
            var result = await _favoriteProvider.UpdateFavoritePgcStatusAsync(Data.Identifier.Id, status);
            if (result)
            {
                _additionalAction?.Invoke(this);
            }
        }
    }
}

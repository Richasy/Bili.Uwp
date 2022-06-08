// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using Windows.System;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// 剧集条目视图模型.
    /// </summary>
    public sealed partial class SeasonItemViewModel : ViewModelBase, IVideoBaseViewModel
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

            OpenInBroswerCommand = ReactiveCommand.CreateFromTask(OpenInBroswerAsync, outputScheduler: RxApp.MainThreadScheduler);
            PlayCommand = ReactiveCommand.Create(Play, outputScheduler: RxApp.MainThreadScheduler);
            UnfollowCommand = ReactiveCommand.CreateFromTask(UnfollowAsync, outputScheduler: RxApp.MainThreadScheduler);
            ChangeFavoriteStatusCommand = ReactiveCommand.CreateFromTask<int>(ChangeFavoriteStatusAsync, outputScheduler: RxApp.MainThreadScheduler);
        }

        /// <summary>
        /// 设置单集信息，并进行视图模型的初始化.
        /// </summary>
        /// <param name="information">单集信息.</param>
        public void SetInformation(IVideoBase information)
        {
            Information = information as SeasonInformation;
            InitializeData();
        }

        /// <summary>
        /// 设置附加动作，该动作通常发生在删除视频的过程中，连带删除调用源集合中的视频.
        /// </summary>
        /// <param name="action">附加动作.</param>
        public void SetAdditionalAction(Action<SeasonItemViewModel> action)
            => _additionalAction = action;

        private void InitializeData()
        {
            IsShowRating = Information.CommunityInformation?.Score > 0;

            if (Information.CommunityInformation?.TrackCount > 0)
            {
                TrackCountText = _numberToolkit.GetCountText(Information.CommunityInformation.TrackCount);
            }
        }

        private void Play()
            => _navigationViewModel.NavigateToPlayView(Information);

        private async Task OpenInBroswerAsync()
        {
            var uri = $"https://www.bilibili.com/bangumi/play/ss{Information.Identifier.Id}";
            await Launcher.LaunchUriAsync(new Uri(uri));
        }

        private async Task UnfollowAsync()
        {
            var result = await _favoriteProvider.RemoveFavoritePgcAsync(Information.Identifier.Id);
            if (result)
            {
                _additionalAction?.Invoke(this);
            }
        }

        private async Task ChangeFavoriteStatusAsync(int status)
        {
            var result = await _favoriteProvider.UpdateFavoritePgcStatusAsync(Information.Identifier.Id, status);
            if (result)
            {
                _additionalAction?.Invoke(this);
            }
        }
    }
}

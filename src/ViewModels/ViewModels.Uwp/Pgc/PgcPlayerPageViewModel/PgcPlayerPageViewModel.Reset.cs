// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC 播放页面视图模型.
    /// </summary>
    public sealed partial class PgcPlayerPageViewModel
    {
        private void ResetOverview()
        {
            Celebrities.Clear();
            IsShowCelebrities = false;
        }

        private async Task ResetOperationAsync()
        {
            IsLiked = false;
            IsCoined = false;
            IsFavorited = false;
            IsTracking = false;
            FavoriteFoldersErrorText = default;
            IsFavoriteFoldersError = false;

            await _dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                FavoriteFolders.Clear();
            });
        }

        private void ResetCommunityInformation()
        {
            PlayCountText = default;
            DanmakuCountText = default;
            CommentCountText = default;
            LikeCountText = default;
            CoinCountText = default;
            FavoriteCountText = default;
            RatingCountText = default;
        }

        private void ResetInterop()
        {
            _playNextEpisodeAction = default;
            MediaPlayerViewModel.SetPlayNextAction(default);
            IsVideoFixed = false;
        }

        private void ResetSections()
        {
            Sections.Clear();
            Episodes.Clear();
            Seasons.Clear();
            Extras.Clear();
            Seasons.Clear();
            CurrentSection = null;
            IsShowEpisodes = false;
            IsShowSeasons = false;
            IsShowComments = false;
            IsShowExtras = false;
            _commentPageViewModel.ClearData();
        }
    }
}

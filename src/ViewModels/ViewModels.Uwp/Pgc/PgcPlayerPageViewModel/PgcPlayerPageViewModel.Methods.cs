// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.Data.Local;
using Bili.Models.Data.Pgc;
using Bili.Models.Enums.Bili;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC 播放页面视图模型.
    /// </summary>
    public sealed partial class PgcPlayerPageViewModel
    {
        private void CheckSectionVisibility()
        {
            IsShowSeasons = CurrentSection.Type == PlayerSectionType.Seasons;
            IsShowEpisodes = CurrentSection.Type == PlayerSectionType.Episodes;
            IsShowExtras = CurrentSection.Type == PlayerSectionType.Extras;
            IsShowComments = CurrentSection.Type == PlayerSectionType.Comments;
        }

        private void Share()
        {
            var dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += OnShareDataRequested;
            DataTransferManager.ShowShareUI();
        }

        private void SelectSeason(SeasonInformation season)
            => SetSnapshot(new PlaySnapshot(default, season.Identifier.Id, Models.Enums.VideoType.Pgc));

        private void SelectEpisode(EpisodeInformation episode)
        {
            CurrentEpisode = episode;
            foreach (var item in Episodes)
            {
                item.IsSelected = episode.Identifier.Id == item.Information.Identifier.Id;
            }

            if (Extras.Count > 0)
            {
                foreach (var extra in Extras)
                {
                    foreach (var item in extra.Episodes)
                    {
                        item.IsSelected = episode.Identifier.Id == item.Information.Identifier.Id;
                    }
                }
            }

            ReloadInteractionInformationCommand.Execute().Subscribe();
            MediaPlayerViewModel.SetPgcData(View, CurrentEpisode);
            _commentPageViewModel.SetData(CurrentEpisode.VideoId, CommentType.Video);
        }

        private void ShowSeasonDetail()
            => _appViewModel.ShowPgcSeasonDetail();

        private async Task FixAsync()
        {
            if (IsVideoFixed)
            {
                await _accountViewModel.RemoveFixedItemAsync(View.Information.Identifier.Id);
                IsVideoFixed = false;
            }
            else
            {
                await _accountViewModel.AddFixedItemAsync(new FixedItem(
                    View.Information.Identifier.Cover.Uri,
                    View.Information.Identifier.Title,
                    View.Information.Identifier.Id,
                    Models.Enums.App.FixedType.Pgc));
                IsVideoFixed = true;
            }
        }

        private void OnAuthorizeStateChanged(object sender, AuthorizeStateChangedEventArgs e)
            => IsSignedIn = e.NewState == Models.Enums.AuthorizeState.SignedIn;

        private void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var request = args.Request;
            var url = $"https://www.bilibili.com/bangumi/play/ss{View.Information.Identifier.Id}";

            request.Data.Properties.Title = View.Information.Identifier.Title;
            request.Data.Properties.Description = View.Information.Description;
            request.Data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromUri(View.Information.Identifier.Cover.GetSourceUri());
            request.Data.Properties.ContentSourceWebLink = new Uri(url);

            request.Data.SetText(View.Information.Description);
            request.Data.SetWebLink(new Uri(url));
            request.Data.SetBitmap(RandomAccessStreamReference.CreateFromUri(View.Information.Identifier.Cover.GetSourceUri()));
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using System.ComponentModel;
using System.Linq;
using Bili.Models.App.Args;
using Bili.Models.Data.Local;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
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
                item.IsSelected = episode.Identifier.Id == item.Data.Identifier.Id;
            }

            if (Extras.Count > 0)
            {
                foreach (var extra in Extras)
                {
                    foreach (var item in extra.Episodes)
                    {
                        item.IsSelected = episode.Identifier.Id == item.Data.Identifier.Id;
                    }
                }
            }

            ReloadCommunityInformationCommand.ExecuteAsync(null);
            MediaPlayerViewModel.SetPgcData(View, CurrentEpisode);
            _commentPageViewModel.SetData(CurrentEpisode.VideoId, CommentType.Video);
            CreatePlayNextAction();
        }

        private void CreatePlayNextAction()
        {
            _playNextEpisodeAction = null;

            // 当前分集为空时不处理.
            if (CurrentEpisode == null)
            {
                return;
            }

            MediaPlayerViewModel.CanPlayNextPart = CurrentEpisode.IsPreviewVideo
                ? !Extras.LastOrDefault()?.Equals(CurrentEpisode) ?? false
                : !Episodes.LastOrDefault()?.Equals(CurrentEpisode) ?? false;

            EpisodeInformation nextPart = default;
            var isPreview = CurrentEpisode.IsPreviewVideo;
            if (!isPreview && Sections.Any(p => p.Type == PlayerSectionType.Episodes))
            {
                var canContinue = Episodes.Count > 1 && CurrentEpisode != Episodes.Last().Data;
                if (canContinue)
                {
                    nextPart = Episodes.FirstOrDefault(p => p.Data.Index == CurrentEpisode.Index + 1)?.Data;
                }
            }
            else if (isPreview && Sections.Any(p => p.Type == PlayerSectionType.Extras))
            {
                var extras = Extras.SelectMany(p => p.Episodes).ToList();
                var index = extras.IndexOf(extras.FirstOrDefault(p => p.Equals(CurrentEpisode)));
                var canContinue = index != -1 && extras.Count > 1 && CurrentEpisode != extras.Last().Data;
                if (canContinue)
                {
                    nextPart = extras[index + 1].Data;
                }
            }

            if (nextPart == null)
            {
                return;
            }

            MediaPlayerViewModel.NextVideoTipText = nextPart.Identifier.Title;
            _playNextEpisodeAction = () =>
            {
                SelectEpisode(nextPart);
            };

            MediaPlayerViewModel.SetPlayNextAction(_playNextEpisodeAction);
        }

        private void ShowSeasonDetail()
            => _callerViewModel.ShowPgcSeasonDetail();

        private void Fix()
        {
            if (_accountViewModel.State != AuthorizeState.SignedIn)
            {
                _callerViewModel.ShowTip(_resourceToolkit.GetLocaleString(LanguageNames.NeedLoginFirst), Models.Enums.App.InfoType.Warning);
                return;
            }

            if (IsVideoFixed)
            {
                _accountViewModel.RemoveFixedItemCommand.ExecuteAsync(View.Information.Identifier.Id);
                IsVideoFixed = false;
            }
            else
            {
                _accountViewModel.AddFixedItemCommand.ExecuteAsync(new FixedItem(
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

        private void OnMediaEnded(object sender, EventArgs e)
        {
            if (_playNextEpisodeAction == null)
            {
                return;
            }

            var isContinue = _settingsToolkit.ReadLocalSetting(SettingNames.IsContinusPlay, true);
            if (isContinue)
            {
                _playNextEpisodeAction?.Invoke();
            }
            else
            {
                MediaPlayerViewModel.ShowNextVideoTipCommand.Execute(null);
            }
        }

        private void OnInternalPartChanged(object sender, VideoIdentifier e)
        {
            var episode = Episodes.FirstOrDefault(p => p.Data.Identifier.Id == e.Id);
            if (episode == null && Extras.Count > 0)
            {
                episode = Extras.SelectMany(p => p.Episodes).FirstOrDefault(p => p.Data.Identifier.Id == e.Id);
            }

            if (episode == null)
            {
                return;
            }

            ChangeEpisodeCommand.Execute(episode.Data);
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(IsOnlyShowIndex))
            {
                _settingsToolkit.WriteLocalSetting(SettingNames.IsOnlyShowIndex, IsOnlyShowIndex);
            }
        }
    }
}

﻿// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.Data.Community;
using Bili.Models.Data.Local;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Models.Enums.Bili;
using Splat;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频播放页面视图模型.
    /// </summary>
    public sealed partial class VideoPlayerPageViewModel
    {
        private VideoItemViewModel GetItemViewModel(VideoInformation information)
        {
            var vm = Splat.Locator.Current.GetService<VideoItemViewModel>();
            vm.SetInformation(information);
            return vm;
        }

        private void CheckSectionVisibility()
        {
            IsShowUgcSeason = CurrentSection.Type == PlayerSectionType.UgcSeason;
            IsShowRelatedVideos = CurrentSection.Type == PlayerSectionType.RelatedVideos;
            IsShowParts = CurrentSection.Type == PlayerSectionType.VideoParts;
            IsShowComments = CurrentSection.Type == PlayerSectionType.Comments;
        }

        private void SearchTag(Tag tag)
            => _navigationViewModel.NavigateToSecondaryView(Models.Enums.PageIds.Search, tag.Name);

        private void SelectSeason(VideoSeason season)
        {
            CurrentSeason = season;
            CurrentSeasonVideos.Clear();
            if (season == null)
            {
                return;
            }

            foreach (var item in CurrentSeason.Videos)
            {
                var vm = GetItemViewModel(item);
                vm.IsSelected = vm.Information.Equals(View.Information);
                CurrentSeasonVideos.Add(vm);
            }
        }

        private void Share()
        {
            var dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += OnShareDataRequested;
            DataTransferManager.ShowShareUI();
        }

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
                    Models.Enums.App.FixedType.Video));
                IsVideoFixed = true;
            }
        }

        private void ChangeVideoPart(VideoIdentifier identifier)
        {
            foreach (var item in VideoParts)
            {
                item.IsSelected = item.Data.Equals(identifier);
            }

            CurrentVideoPart = VideoParts.FirstOrDefault(p => p.IsSelected).Data;
            CreatePlayNextAction();
            MediaPlayerViewModel.ChangePartCommand.Execute(identifier).Subscribe();
        }

        private void CreatePlayNextAction()
        {
            MediaPlayerViewModel.CanPlayNextPart = View.InteractionVideo == null
                && VideoParts.FirstOrDefault(p => p.IsSelected).Index < VideoParts.Last().Index;
            _playNextVideoAction = null;

            // 不处理互动视频.
            if (View.InteractionVideo != null)
            {
                return;
            }

            var isNewVideo = false;
            VideoIdentifier? nextPart = default;
            if (Sections.Any(p => p.Type == PlayerSectionType.VideoParts))
            {
                var canContinue = VideoParts.Count > 1 && CurrentVideoPart.Id != VideoParts.Last().Data.Id;
                if (canContinue)
                {
                    var currentPart = VideoParts.First(p => p.Data.Id == CurrentVideoPart.Id);
                    nextPart = VideoParts.FirstOrDefault(p => p.Index == currentPart.Index + 1)?.Data;
                }
            }
            else if (Sections.Any(p => p.Type == PlayerSectionType.UgcSeason))
            {
                var currentVideo = CurrentSeasonVideos.FirstOrDefault(p => p.IsSelected);
                if (currentVideo != null)
                {
                    var canContinue = CurrentSeasonVideos.Count > 1 && !CurrentSeasonVideos.Last().Equals(currentVideo);
                    if (canContinue)
                    {
                        var index = CurrentSeasonVideos.IndexOf(currentVideo);
                        nextPart = CurrentSeasonVideos[index + 1].Information.Identifier;
                        isNewVideo = true;
                    }
                }
            }
            else if (_settingsToolkit.ReadLocalSetting(SettingNames.IsAutoPlayNextRelatedVideo, false)
                && RelatedVideos.Count > 0)
            {
                nextPart = RelatedVideos.First().Information.Identifier;
                isNewVideo = true;
            }

            if (nextPart == null)
            {
                return;
            }

            MediaPlayerViewModel.NextVideoTipText = nextPart.Value.Title;
            _playNextVideoAction = () =>
            {
                if (isNewVideo)
                {
                    SetSnapshot(new PlaySnapshot(nextPart.Value.Id, default, VideoType.Video));
                }
                else
                {
                    ChangeVideoPart(nextPart.Value);
                }
            };

            MediaPlayerViewModel.SetPlayNextAction(_playNextVideoAction);
        }

        private void OnAuthorizeStateChanged(object sender, AuthorizeStateChangedEventArgs e)
            => IsSignedIn = e.NewState == AuthorizeState.SignedIn;

        private void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var request = args.Request;
            var url = $"https://www.bilibili.com/video/{View.Information.AlternateId}";

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
            if (_playNextVideoAction == null)
            {
                return;
            }

            var isContinue = _settingsToolkit.ReadLocalSetting(SettingNames.IsContinusPlay, true);
            if (isContinue)
            {
                _playNextVideoAction?.Invoke();
            }
            else
            {
                MediaPlayerViewModel.ShowNextVideoTipCommand.Execute().Subscribe();
            }
        }
    }
}

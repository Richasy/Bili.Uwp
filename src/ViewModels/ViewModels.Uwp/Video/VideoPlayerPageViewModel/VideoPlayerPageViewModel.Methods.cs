// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Models.App.Args;
using Bili.Models.Data.Community;
using Bili.Models.Data.Local;
using Bili.Models.Data.Video;
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

        private void OnAuthorizeStateChanged(object sender, AuthorizeStateChangedEventArgs e)
            => IsSignedIn = e.NewState == Models.Enums.AuthorizeState.SignedIn;

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
    }
}

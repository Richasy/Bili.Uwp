// Copyright (c) Richasy. All rights reserved.

using Bili.Models.Data.Community;
using Bili.Models.Data.Video;
using Bili.Models.Enums.Bili;
using Splat;

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
    }
}

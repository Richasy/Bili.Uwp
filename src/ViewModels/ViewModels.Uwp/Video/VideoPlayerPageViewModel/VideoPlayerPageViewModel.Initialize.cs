// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using Bili.Models.App.Other;
using Bili.Models.Enums;
using Bili.Models.Enums.Bili;
using Bili.ViewModels.Uwp.Account;
using Humanizer;
using Splat;

namespace Bili.ViewModels.Uwp.Video
{
    /// <summary>
    /// 视频播放页面视图模型.
    /// </summary>
    public sealed partial class VideoPlayerPageViewModel
    {
        private void InitializePublisher()
        {
            IsCooperationVideo = View.Information.Collaborators != null;
            if (IsCooperationVideo)
            {
                foreach (var profile in View.Information.Collaborators)
                {
                    var userVM = Splat.Locator.Current.GetService<UserItemViewModel>();
                    userVM.SetProfile(profile);
                    Collaborators.Add(userVM);
                }
            }
            else
            {
                var myId = _authorizeProvider.CurrentUserId;
                var userVM = Splat.Locator.Current.GetService<UserItemViewModel>();
                userVM.SetProfile(View.Information.Publisher);
                userVM.Relation = View.PublisherCommunityInformation.Relation;
                userVM.IsRelationButtonShown = !string.IsNullOrEmpty(myId)
                    && myId != View.Information.Publisher.User.Id;
                Author = userVM;
            }
        }

        private void InitializeOverview()
        {
            PublishTime = View.Information.PublishTime.Humanize();
            WatchingCountText = "--";

            if (View.Tags != null)
            {
                View.Tags.ToList().ForEach(p => Tags.Add(p));
            }

            IsShowTags = Tags.Count > 0;
        }

        private void InitializeOperation()
        {
            IsLiked = View.Operation.IsLiked;
            IsCoined = View.Operation.IsCoined;
            IsFavorited = View.Operation.IsFavorited;
            IsCoinWithLiked = true;
        }

        private void InitializeCommunityInformation()
        {
            var communityInfo = View.Information.CommunityInformation;
            PlayCountText = _numberToolkit.GetCountText(communityInfo.PlayCount);
            DanmakuCountText = _numberToolkit.GetCountText(communityInfo.DanmakuCount);
            CommentCountText = _numberToolkit.GetCountText(communityInfo.CommentCount);
            LikeCountText = _numberToolkit.GetCountText(communityInfo.LikeCount);
            CoinCountText = _numberToolkit.GetCountText(communityInfo.CoinCount);
            FavoriteCountText = _numberToolkit.GetCountText(communityInfo.FavoriteCount);
        }

        private void InitializeInterop()
        {
            var downloadParam = string.IsNullOrEmpty(View.Information.AlternateId)
                ? $"av{View.Information.Identifier.Id}"
                : View.Information.AlternateId;
            var downloadParts = VideoParts.Select((_, index) => index + 1).ToList();
            DownloadViewModel.SetData(downloadParam, downloadParts);
            IsOnlyShowIndex = _settingsToolkit.ReadLocalSetting(SettingNames.IsOnlyShowIndex, false);
            var fixedItems = _accountViewModel.FixedItemCollection;
            IsVideoFixed = fixedItems.Any(p => p.Type == Models.Enums.App.FixedType.Video && p.Id == View.Information.Identifier.Id);
        }

        private void InitializeSections()
        {
            // 处理顶部标签.
            var hasVideoParts = View.SubVideos != null && View.SubVideos.Count() > 0;
            var hasSeason = View.Seasons != null && View.Seasons.Count() > 0;
            var hasRelatedVideos = View.RelatedVideos != null && View.RelatedVideos.Count() > 0;

            if (hasVideoParts)
            {
                // 只有分P数大于1时才提供切换功能.
                if (View.SubVideos.Count() > 1 && View.InteractionVideo == null)
                {
                    Sections.Add(new PlayerSectionHeader(PlayerSectionType.VideoParts, _resourceToolkit.GetLocaleString(LanguageNames.Parts)));
                }

                var subVideos = View.SubVideos.ToList();
                CurrentVideoPart = subVideos.First();
                for (var i = 0; i < subVideos.Count; i++)
                {
                    var item = subVideos[i];
                    VideoParts.Add(new VideoIdentifierSelectableViewModel(item, i + 1, item.Equals(CurrentVideoPart)));
                }
            }

            if (hasSeason)
            {
                View.Seasons.ToList().ForEach(p => Seasons.Add(p));
                var season = Seasons.FirstOrDefault(p => p.Videos != null && p.Videos.Any(j => j.Equals(View.Information)));
                if (season != null)
                {
                    // 只有确定当前合集包含正在播放的视频时才显示合集标头
                    Sections.Add(new PlayerSectionHeader(PlayerSectionType.UgcSeason, _resourceToolkit.GetLocaleString(LanguageNames.UgcEpisode)));
                    SelectSeason(season);
                }
            }

            if (hasRelatedVideos)
            {
                Sections.Add(new PlayerSectionHeader(PlayerSectionType.RelatedVideos, _resourceToolkit.GetLocaleString(LanguageNames.RelatedVideos)));
                View.RelatedVideos.ToList().ForEach(p => RelatedVideos.Add(GetItemViewModel(p)));
            }

            // 评论区常显，但位于最后一个.
            Sections.Add(new PlayerSectionHeader(PlayerSectionType.Comments, _resourceToolkit.GetLocaleString(LanguageNames.Reply)));
            _commentPageViewModel.SetData(View.Information.Identifier.Id, CommentType.Video);

            CurrentSection = Sections.First();
            RequestOnlineCountCommand.Execute().Subscribe();
        }
    }
}

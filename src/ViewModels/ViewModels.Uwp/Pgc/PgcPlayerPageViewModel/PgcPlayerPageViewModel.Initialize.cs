// Copyright (c) Richasy. All rights reserved.

using System;
using System.Linq;
using Bili.Models.App.Other;
using Bili.Models.Enums;
using Bili.Models.Enums.Bili;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Video;
using Splat;

namespace Bili.ViewModels.Uwp.Pgc
{
    /// <summary>
    /// PGC 播放页面视图模型.
    /// </summary>
    public sealed partial class PgcPlayerPageViewModel
    {
        private void InitializeOverview()
        {
            var actors = View.Information.Celebrities;
            if (actors != null)
            {
                foreach (var item in actors)
                {
                    var vm = Splat.Locator.Current.GetService<UserItemViewModel>();
                    vm.SetProfile(item);
                    Celebrities.Add(vm);
                }
            }

            IsShowCelebrities = Celebrities.Count > 0;
        }

        private void InitializeOperation()
        {
            IsTracking = View.Information.IsTracking;
            IsCoinWithLiked = true;
            ReloadInteractionInformationCommand.Execute().Subscribe();
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

            if (communityInfo.Score > 0)
            {
                RatingCountText = _numberToolkit.GetCountText(View.Information.RatingCount) + _resourceToolkit.GetLocaleString(LanguageNames.PeopleCount);
            }
        }

        private void InitializeInterop()
        {
            var downloadParam = $"ss{View.Information.Identifier.Id}";
            var downloadParts = Episodes.Select((_, index) => index + 1).ToList();
            DownloadViewModel.SetData(downloadParam, downloadParts);
            IsOnlyShowIndex = _settingsToolkit.ReadLocalSetting(SettingNames.IsOnlyShowIndex, false);
            var fixedItems = _accountViewModel.FixedItemCollection;
            IsVideoFixed = fixedItems.Any(p => p.Type == Models.Enums.App.FixedType.Pgc && p.Id == View.Information.Identifier.Id);
        }

        private void InitializeSections()
        {
            // 处理顶部标签.
            var hasEpisodes = View.Episodes != null && View.Episodes.Count() > 0;
            var hasSeason = View.Seasons != null && View.Seasons.Count() > 0;
            var hasExtras = View.Extras != null && View.Extras.Count > 0;
            var isShowExtraSection = false;

            if (hasEpisodes)
            {
                CurrentEpisode = View.Episodes.FirstOrDefault(p => p.Identifier.Id == _presetEpisodeId);
            }

            if (CurrentEpisode == null && hasExtras)
            {
                CurrentEpisode = View.Extras.SelectMany(p => p.Value).FirstOrDefault(p => p.Identifier.Id == _presetEpisodeId);
                isShowExtraSection = CurrentEpisode != null;
            }

            if (CurrentEpisode == null)
            {
                if (hasEpisodes)
                {
                    CurrentEpisode = View.Episodes.First();
                }
                else if (hasExtras)
                {
                    CurrentEpisode = View.Extras.First().Value.FirstOrDefault();
                }
            }

            if (hasEpisodes)
            {
                // 只有分集数大于1时才提供切换功能.
                if (View.Episodes.Count() > 1)
                {
                    Sections.Add(new PlayerSectionHeader(PlayerSectionType.Episodes, _resourceToolkit.GetLocaleString(LanguageNames.Episodes)));
                }

                var subVideos = View.Episodes.ToList();

                for (var i = 0; i < subVideos.Count; i++)
                {
                    var item = subVideos[i];
                    var vm = Splat.Locator.Current.GetService<EpisodeItemViewModel>();
                    vm.SetInformation(item);
                    vm.IsSelected = item.Equals(CurrentEpisode);
                    Episodes.Add(vm);
                }
            }

            if (hasSeason)
            {
                Sections.Add(new PlayerSectionHeader(PlayerSectionType.Seasons, _resourceToolkit.GetLocaleString(LanguageNames.Seasons)));
                var seasons = View.Seasons.ToList();
                for (var i = 0; i < seasons.Count; i++)
                {
                    var item = seasons[i];
                    var vm = new VideoIdentifierSelectableViewModel(item, i + 1, item.Id == View.Information.Identifier.Id);
                    Seasons.Add(vm);
                }
            }

            if (hasExtras)
            {
                Sections.Add(new PlayerSectionHeader(PlayerSectionType.Extras, _resourceToolkit.GetLocaleString(LanguageNames.Sections)));
                var currentId = CurrentEpisode == null ? string.Empty : CurrentEpisode.Identifier.Id;
                View.Extras.ToList().ForEach(p => Extras.Add(new PgcExtraItemViewModel(p.Key, p.Value, currentId)));
            }

            if (CurrentEpisode != null)
            {
                Sections.Add(new PlayerSectionHeader(PlayerSectionType.Comments, _resourceToolkit.GetLocaleString(LanguageNames.Reply)));
                _commentPageViewModel.SetData(CurrentEpisode.VideoId, CommentType.Video);
            }

            CreatePlayNextAction();
            IsSectionsEmpty = Sections.Count == 0;
            CurrentSection = isShowExtraSection
                ? Sections.FirstOrDefault(p => p.Type == PlayerSectionType.Extras)
                : Sections.FirstOrDefault();
        }
    }
}

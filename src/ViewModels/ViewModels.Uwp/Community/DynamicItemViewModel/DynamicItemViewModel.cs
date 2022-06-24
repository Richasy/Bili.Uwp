// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.App.Args;
using Bili.Models.Data.Appearance;
using Bili.Models.Data.Article;
using Bili.Models.Data.Dynamic;
using Bili.Models.Data.Local;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;
using Bili.Models.Enums.Community;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Article;
using Bili.ViewModels.Uwp.Core;
using Bili.ViewModels.Uwp.Video;
using ReactiveUI;
using Splat;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 动态条目视图模型.
    /// </summary>
    public sealed partial class DynamicItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicItemViewModel"/> class.
        /// </summary>
        public DynamicItemViewModel(
            ICommunityProvider communityProvider,
            INumberToolkit numberToolkit,
            IResourceToolkit resourceToolkit,
            AppViewModel appViewModel,
            NavigationViewModel navigationViewModel)
        {
            _communityProvider = communityProvider;
            _numberToolkit = numberToolkit;
            _resourceToolkit = resourceToolkit;
            _appViewModel = appViewModel;
            _navigationViewModel = navigationViewModel;

            ToggleLikeCommand = ReactiveCommand.CreateFromTask(ToggleLikeAsync, outputScheduler: RxApp.MainThreadScheduler);
            ActiveCommand = ReactiveCommand.Create(Active, outputScheduler: RxApp.MainThreadScheduler);
            AddToViewLaterCommand = ReactiveCommand.Create(AddToViewLater, outputScheduler: RxApp.MainThreadScheduler);
            ShowUserDetailCommand = ReactiveCommand.Create(ShowUserDetail, outputScheduler: RxApp.MainThreadScheduler);
            ShowCommentDetailCommand = ReactiveCommand.Create(ShowCommentDetail, outputScheduler: RxApp.MainThreadScheduler);
            ShareCommand = ReactiveCommand.Create(ShowShareUI, outputScheduler: RxApp.MainThreadScheduler);
        }

        /// <summary>
        /// 设置信息.
        /// </summary>
        /// <param name="information">动态信息.</param>
        public void SetInformation(DynamicInformation information)
        {
            Information = information;
            InitializeData();
        }

        private void InitializeData()
        {
            IsShowCommunity = Information.CommunityInformation != null;
            if (IsShowCommunity)
            {
                IsLiked = Information.CommunityInformation.IsLiked;
                LikeCountText = _numberToolkit.GetCountText(Information.CommunityInformation.LikeCount);
                CommentCountText = _numberToolkit.GetCountText(Information.CommunityInformation.CommentCount);
            }

            if (Information.User != null)
            {
                var userVM = Splat.Locator.Current.GetService<UserItemViewModel>();
                userVM.SetProfile(Information.User);
                Publisher = userVM;
            }

            CanAddViewLater = Information.Data is VideoInformation;
        }

        private async Task ToggleLikeAsync()
        {
            var isLike = !IsLiked;
            var result = await _communityProvider.LikeDynamicAsync(Information.Id, isLike, Publisher.User.Id, Information.CommentId);
            if (result)
            {
                IsLiked = isLike;
                if (isLike)
                {
                    Information.CommunityInformation.LikeCount += 1;
                }
                else
                {
                    Information.CommunityInformation.LikeCount -= 1;
                }

                LikeCountText = _numberToolkit.GetCountText(Information.CommunityInformation.LikeCount);
            }
            else
            {
                _appViewModel.ShowTip(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.SetFailed), Models.Enums.App.InfoType.Error);
            }
        }

        private void Active()
            => ActiveData(Information.Data);

        private void ActiveData(object data)
        {
            if (data == null || data is IEnumerable<Image>)
            {
                return;
            }

            if (data is VideoInformation video)
            {
                var playSnapshot = new PlaySnapshot(video.Identifier.Id, "0", Models.Enums.VideoType.Video);
                _navigationViewModel.NavigateToPlayView(playSnapshot);
            }
            else if (data is EpisodeInformation episode)
            {
                var needBiliPlus = episode.Identifier.Id == "0";
                var id = needBiliPlus
                    ? episode.VideoId
                    : episode.Identifier.Id;

                var playSnapshot = new PlaySnapshot(id, episode.SeasonId, Models.Enums.VideoType.Pgc)
                {
                    Title = episode.Identifier.Title,
                    NeedBiliPlus = needBiliPlus,
                };
                _navigationViewModel.NavigateToPlayView(playSnapshot);
            }
            else if (data is ArticleInformation article)
            {
                var articleVM = Splat.Locator.Current.GetService<ArticleItemViewModel>();
                articleVM.SetInformation(article);
                _appViewModel.ShowArticleReader(articleVM);
            }
            else if (data is DynamicInformation dynamic)
            {
                ActiveData(dynamic.Data);
            }
        }

        private void AddToViewLater()
        {
            if (Information.Data is VideoInformation videoInfo)
            {
                var videoVM = Splat.Locator.Current.GetService<VideoItemViewModel>();
                videoVM.SetInformation(videoInfo);
                videoVM.AddToViewLaterCommand.Execute().Subscribe();
            }
        }

        private void ShowUserDetail()
        {
            if (Information.User != null)
            {
                var userVM = Splat.Locator.Current.GetService<UserItemViewModel>();
                userVM.SetProfile(Information.User);
                _appViewModel.ShowUserDetail(userVM);
            }
        }

        private void ShowCommentDetail()
        {
            var args = new ShowCommentEventArgs(Information.CommentType, Models.Enums.Bili.CommentSortType.Hot, Information.CommentId);
            _appViewModel.ShowReply(args);
        }

        private void ShowShareUI()
        {
            var dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += OnShareDataRequested;
            DataTransferManager.ShowShareUI();
        }

        private void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var request = args.Request;
            var url = string.Empty;
            Uri coverUri = null;
            var title = string.Empty;

            request.Data.SetText(Information.Description?.Text ?? string.Empty);
            if (Information.DynamicType == DynamicItemType.Video)
            {
                var videoInfo = Information.Data as VideoInformation;
                title = videoInfo.Identifier.Title;
                coverUri = videoInfo.Identifier.Cover.GetSourceUri();
                url = $"https://www.bilibili.com/video/{videoInfo.AlternateId}";
            }
            else if (Information.DynamicType == DynamicItemType.Pgc)
            {
                var episodeInfo = Information.Data as EpisodeInformation;
                title = episodeInfo.Identifier.Title;
                coverUri = episodeInfo.Identifier.Cover.GetSourceUri();
                url = $"https://www.bilibili.com/bangumi/play/ss{episodeInfo.SeasonId}";
            }

            request.Data.Properties.Title = title;
            if (!string.IsNullOrEmpty(url))
            {
                request.Data.SetWebLink(new Uri(url));
            }

            if (coverUri != null)
            {
                request.Data.SetBitmap(RandomAccessStreamReference.CreateFromUri(coverUri));
            }
        }
    }
}

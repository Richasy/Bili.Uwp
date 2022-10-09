// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.App.Args;
using Bili.Models.Data.Appearance;
using Bili.Models.Data.Article;
using Bili.Models.Data.Dynamic;
using Bili.Models.Data.Local;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Models.Enums.Community;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Article;
using Bili.ViewModels.Interfaces.Community;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Video;
using CommunityToolkit.Mvvm.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;

namespace Bili.ViewModels.Uwp.Community
{
    /// <summary>
    /// 动态条目视图模型.
    /// </summary>
    public sealed partial class DynamicItemViewModel : ViewModelBase, IDynamicItemViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DynamicItemViewModel"/> class.
        /// </summary>
        public DynamicItemViewModel(
            ICommunityProvider communityProvider,
            INumberToolkit numberToolkit,
            IResourceToolkit resourceToolkit,
            ICallerViewModel callerViewModel,
            INavigationViewModel navigationViewModel)
        {
            _communityProvider = communityProvider;
            _numberToolkit = numberToolkit;
            _resourceToolkit = resourceToolkit;
            _callerViewModel = callerViewModel;
            _navigationViewModel = navigationViewModel;

            ToggleLikeCommand = new AsyncRelayCommand(ToggleLikeAsync);
            ActiveCommand = new RelayCommand(Active);
            AddToViewLaterCommand = new RelayCommand(AddToViewLater);
            ShowUserDetailCommand = new RelayCommand(ShowUserDetail);
            ShowCommentDetailCommand = new RelayCommand(ShowCommentDetail);
            ShareCommand = new RelayCommand(ShowShareUI);
        }

        /// <summary>
        /// 设置信息.
        /// </summary>
        /// <param name="information">动态信息.</param>
        public void InjectData(DynamicInformation information)
        {
            Data = information;
            InitializeData();
        }

        private void InitializeData()
        {
            IsShowCommunity = Data.CommunityInformation != null;
            if (IsShowCommunity)
            {
                IsLiked = Data.CommunityInformation.IsLiked;
                LikeCountText = _numberToolkit.GetCountText(Data.CommunityInformation.LikeCount);
                CommentCountText = _numberToolkit.GetCountText(Data.CommunityInformation.CommentCount);
            }

            if (Data.User != null)
            {
                var userVM = Locator.Instance.GetService<IUserItemViewModel>();
                userVM.SetProfile(Data.User);
                Publisher = userVM;
            }

            CanAddViewLater = Data.Data is VideoInformation;
        }

        private async Task ToggleLikeAsync()
        {
            var isLike = !IsLiked;
            var result = await _communityProvider.LikeDynamicAsync(Data.Id, isLike, Publisher.User.Id, Data.CommentId);
            if (result)
            {
                IsLiked = isLike;
                if (isLike)
                {
                    Data.CommunityInformation.LikeCount += 1;
                }
                else
                {
                    Data.CommunityInformation.LikeCount -= 1;
                }

                LikeCountText = _numberToolkit.GetCountText(Data.CommunityInformation.LikeCount);
            }
            else
            {
                _callerViewModel.ShowTip(_resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.SetFailed), Models.Enums.App.InfoType.Error);
            }
        }

        private void Active()
            => ActiveData(Data.Data);

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

                var playSnapshot = new PlaySnapshot(id, episode.SeasonId, VideoType.Pgc)
                {
                    Title = episode.Identifier.Title,
                    NeedBiliPlus = needBiliPlus,
                };
                _navigationViewModel.NavigateToPlayView(playSnapshot);
            }
            else if (data is ArticleInformation article)
            {
                var articleVM = Locator.Instance.GetService<IArticleItemViewModel>();
                articleVM.InjectData(article);
                _callerViewModel.ShowArticleReader(articleVM);
            }
            else if (data is DynamicInformation dynamic)
            {
                ActiveData(dynamic.Data);
            }
        }

        private void AddToViewLater()
        {
            if (Data.Data is VideoInformation videoInfo)
            {
                var videoVM = Locator.Instance.GetService<IVideoItemViewModel>();
                videoVM.InjectData(videoInfo);
                videoVM.AddToViewLaterCommand.Execute(null);
            }
        }

        private void ShowUserDetail()
        {
            if (Data.User != null)
            {
                _navigationViewModel.Navigate(PageIds.UserSpace, Data.User);
            }
        }

        private void ShowCommentDetail()
        {
            var args = new ShowCommentEventArgs(Data.CommentType, Models.Enums.Bili.CommentSortType.Hot, Data.CommentId);
            _callerViewModel.ShowReply(args);
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

            request.Data.SetText(Data.Description?.Text ?? string.Empty);
            if (Data.DynamicType == DynamicItemType.Video)
            {
                var videoInfo = Data.Data as VideoInformation;
                title = videoInfo.Identifier.Title;
                coverUri = videoInfo.Identifier.Cover.GetSourceUri();
                url = $"https://www.bilibili.com/video/{videoInfo.AlternateId}";
            }
            else if (Data.DynamicType == DynamicItemType.Pgc)
            {
                var episodeInfo = Data.Data as EpisodeInformation;
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

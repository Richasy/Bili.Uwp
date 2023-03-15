// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Appearance;
using Bili.Models.Data.Dynamic;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;
using Bili.Models.Enums;
using Bili.Models.Enums.Workspace;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Community;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Video;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Workspace.Community
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
            ICallerViewModel callerViewModel)
        {
            _communityProvider = communityProvider;
            _numberToolkit = numberToolkit;
            _resourceToolkit = resourceToolkit;
            _callerViewModel = callerViewModel;

            ToggleLikeCommand = new AsyncRelayCommand(ToggleLikeAsync);
            ActiveCommand = new AsyncRelayCommand(ActiveAsync);
            AddToViewLaterCommand = new RelayCommand(AddToViewLater);
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
                _callerViewModel.ShowTip(_resourceToolkit.GetLocaleString(LanguageNames.SetFailed), Models.Enums.App.InfoType.Error);
            }
        }

        private Task ActiveAsync()
            => ActiveDataAsync(Data.Data);

        private async Task ActiveDataAsync(object data)
        {
            if (data == null || data is IEnumerable<Image>)
            {
                return;
            }

            if (data is VideoInformation video)
            {
                await Utilities.PlayVideoWithIdAsync(video.Identifier.Id);
            }
            else if (data is EpisodeInformation episode)
            {
                if (episode.Identifier.Id == "0")
                {
                    await Utilities.PlayVideoWithIdAsync(episode.VideoId);
                }
                else
                {
                    await Utilities.PlayEpisodeWithIdAsync(episode.Identifier.Id);
                }
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
    }
}

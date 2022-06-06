// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Appearance;
using Bili.Models.Data.Article;
using Bili.Models.Data.Dynamic;
using Bili.Models.Data.Pgc;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Article;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using Splat;

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
        internal DynamicItemViewModel(
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
        }

        private async Task ToggleLikeAsync()
        {
            var isLike = !IsLiked;
            var result = await _communityProvider.LikeDynamicAsync(Information.Id, isLike, Publisher.User.Id, Information.ReplyId);
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
                _navigationViewModel.NavigateToPlayView(video);
            }
            else if (data is EpisodeInformation episode)
            {
                _navigationViewModel.NavigateToPlayView(episode);
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
    }
}

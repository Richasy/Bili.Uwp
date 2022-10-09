// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.DI.Container;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Article;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Article;
using Bili.ViewModels.Interfaces.Core;
using CommunityToolkit.Mvvm.Input;
using Windows.System;

namespace Bili.ViewModels.Uwp.Article
{
    /// <summary>
    /// 文章条目视图模型.
    /// </summary>
    public sealed partial class ArticleItemViewModel : ViewModelBase, IArticleItemViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleItemViewModel"/> class.
        /// </summary>
        public ArticleItemViewModel(
            INumberToolkit numberToolkit,
            IResourceToolkit resourceToolkit,
            IArticleProvider articleProvider,
            IFavoriteProvider favoriteProvider,
            ICallerViewModel callerViewModel)
        {
            _numberToolkit = numberToolkit;
            _resourceToolkit = resourceToolkit;
            _callerViewModel = callerViewModel;
            _articleProvider = articleProvider;
            _favoriteProvider = favoriteProvider;

            ReadCommand = new RelayCommand(Read);
            OpenInBroswerCommand = new AsyncRelayCommand(OpenInBroswerAsync);
            ReloadCommand = new AsyncRelayCommand(ReloadAsync);
            UnfavoriteCommand = new AsyncRelayCommand(UnfavoriteAsync);

            AttachIsRunningToAsyncCommand(p => IsReloading = p, ReloadCommand);
            AttachExceptionHandlerToAsyncCommand(DisplayException, ReloadCommand);
        }

        /// <summary>
        /// 设置视频信息，并进行视图模型的初始化.
        /// </summary>
        /// <param name="information">视频信息.</param>
        public void InjectData(ArticleInformation information)
        {
            Data = information;
            InitializeData();
        }

        /// <summary>
        /// 设置附加动作.
        /// </summary>
        /// <param name="action">附加动作.</param>
        public void InjectAction(Action<IArticleItemViewModel> action)
            => _additionalAction = action;

        /// <summary>
        /// 获取文章详情内容.
        /// </summary>
        /// <returns>文章HTML文本.</returns>
        public async Task<string> GetDetailAsync()
        {
            IsError = false;
            ErrorText = string.Empty;

            if (string.IsNullOrEmpty(_detailContent))
            {
                _detailContent = await _articleProvider.GetArticleContentAsync(Data.Identifier.Id);
            }

            return _detailContent;
        }

        /// <inheritdoc/>
        public void DisplayException(Exception exception)
        {
            IsError = true;
            var msg = _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestArticleFailed);
            ErrorText = msg;
            LogException(exception);
        }

        private async Task ReloadAsync()
        {
            if (!IsReloading)
            {
                _detailContent = string.Empty;
                await GetDetailAsync();
            }
        }

        private void InitializeData()
        {
            IsShowCommunity = Data.CommunityInformation != null;
            var userVM = Locator.Instance.GetService<IUserItemViewModel>();
            userVM.SetProfile(Data.Publisher);
            Publisher = userVM;
            if (IsShowCommunity)
            {
                ViewCountText = _numberToolkit.GetCountText(Data.CommunityInformation.ViewCount);
                CommentCountText = _numberToolkit.GetCountText(Data.CommunityInformation.CommentCount);
                LikeCountText = _numberToolkit.GetCountText(Data.CommunityInformation.LikeCount);
            }
        }

        private void Read()
            => _callerViewModel.ShowArticleReader(this);

        private async Task OpenInBroswerAsync()
        {
            var uri = $"https://www.bilibili.com/read/cv{Data.Identifier.Id}";
            await Launcher.LaunchUriAsync(new Uri(uri));
        }

        private async Task UnfavoriteAsync()
        {
            var result = await _favoriteProvider.RemoveFavoriteArticleAsync(Data.Identifier.Id);
            if (result)
            {
                _additionalAction?.Invoke(this);
            }
        }
    }
}

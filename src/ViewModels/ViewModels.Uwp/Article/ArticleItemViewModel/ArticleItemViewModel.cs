// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading.Tasks;
using Bili.Lib.Interfaces;
using Bili.Models.Data.Article;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp.Core;
using ReactiveUI;
using Windows.System;

namespace Bili.ViewModels.Uwp.Article
{
    /// <summary>
    /// 文章条目视图模型.
    /// </summary>
    public sealed partial class ArticleItemViewModel : ViewModelBase, IReloadViewModel, IErrorViewModel, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleItemViewModel"/> class.
        /// </summary>
        /// <param name="numberToolkit">数字转换工具.</param>
        /// <param name="resourceToolkit">本地资源工具.</param>
        /// <param name="articleProvider">文章服务提供工具.</param>
        /// <param name="favoriteProvider">收藏服务工具.</param>
        /// <param name="appViewModel">应用视图模型.</param>
        public ArticleItemViewModel(
            INumberToolkit numberToolkit,
            IResourceToolkit resourceToolkit,
            IArticleProvider articleProvider,
            IFavoriteProvider favoriteProvider,
            AppViewModel appViewModel)
        {
            _numberToolkit = numberToolkit;
            _resourceToolkit = resourceToolkit;
            _appViewModel = appViewModel;
            _articleProvider = articleProvider;
            _favoriteProvider = favoriteProvider;

            ReadCommand = ReactiveCommand.Create(Read, outputScheduler: RxApp.MainThreadScheduler);
            OpenInBroswerCommand = ReactiveCommand.CreateFromTask(OpenInBroswerAsync, outputScheduler: RxApp.MainThreadScheduler);
            ReloadCommand = ReactiveCommand.CreateFromTask(ReloadAsync, outputScheduler: RxApp.MainThreadScheduler);
            UnfavoriteCommand = ReactiveCommand.CreateFromTask(UnfavoriteAsync, outputScheduler: RxApp.MainThreadScheduler);

            _isReloading = ReloadCommand.IsExecuting.ToProperty(this, x => x.IsReloading, scheduler: RxApp.MainThreadScheduler);

            ReloadCommand.ThrownExceptions.Subscribe(DisplayException);
        }

        /// <summary>
        /// 设置视频信息，并进行视图模型的初始化.
        /// </summary>
        /// <param name="information">视频信息.</param>
        public void SetInformation(ArticleInformation information)
        {
            Information = information;
            InitializeData();
        }

        /// <summary>
        /// 设置附加动作.
        /// </summary>
        /// <param name="action">附加动作.</param>
        public void SetAdditionalAction(Action<ArticleItemViewModel> action)
            => _additionalAction = action;

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

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
                _detailContent = await _articleProvider.GetArticleContentAsync(Information.Identifier.Id);
            }

            return _detailContent;
        }

        /// <inheritdoc/>
        public void DisplayException(Exception exception)
        {
            IsError = true;
            var msg = _resourceToolkit.GetLocaleString(Models.Enums.LanguageNames.RequestArticleFailed);
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
            IsShowCommunity = Information.CommunityInformation != null;
            if (IsShowCommunity)
            {
                ViewCountText = _numberToolkit.GetCountText(Information.CommunityInformation.ViewCount);
                CommentCountText = _numberToolkit.GetCountText(Information.CommunityInformation.CommentCount);
                LikeCountText = _numberToolkit.GetCountText(Information.CommunityInformation.LikeCount);
            }
        }

        private void Read()
            => _appViewModel.ShowArticleReader(this);

        private async Task OpenInBroswerAsync()
        {
            var uri = $"https://www.bilibili.com/read/cv{Information.Identifier.Id}";
            await Launcher.LaunchUriAsync(new Uri(uri));
        }

        private async Task UnfavoriteAsync()
        {
            var result = await _favoriteProvider.RemoveFavoriteArticleAsync(Information.Identifier.Id);
            if (result)
            {
                _additionalAction?.Invoke(this);
            }
        }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _isReloading.Dispose();
                }

                _disposedValue = true;
            }
        }
    }
}

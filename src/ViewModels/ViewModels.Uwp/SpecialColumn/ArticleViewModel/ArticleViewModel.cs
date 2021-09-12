// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 文章的视图模型.
    /// </summary>
    public partial class ArticleViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleViewModel"/> class.
        /// </summary>
        /// <param name="article">文章类.</param>
        public ArticleViewModel(Article article)
            : this()
        {
            Id = article.Id.ToString();
            var cover = string.Empty;
            var hasCover = article.CoverUrls?.Any() ?? false;
            if (hasCover)
            {
                cover = article.CoverUrls.First();
            }

            Title = article.Title;
            Description = article.Summary;
            Publisher = new UserViewModel(article.Publisher.Publisher, article.Publisher.PublisherAvatar, article.Publisher.Mid);
            PublishTime = DateTimeOffset.FromUnixTimeSeconds(article.PublishTime).ToString("yy/MM/dd HH:mm");
            article.RelatedCategories.ForEach(p => CategoryCollection.Add(p));

            ViewCount = _numberToolkit.GetCountText(article.Stats.ViewCount);
            ReplyCount = _numberToolkit.GetCountText(article.Stats.ReplyCount);
            LikeCount = _numberToolkit.GetCountText(article.Stats.LikeCount);

            LimitCover(cover);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleViewModel"/> class.
        /// </summary>
        /// <param name="item">文章信息.</param>
        public ArticleViewModel(ArticleSearchItem item)
            : this()
        {
            Id = item.Id.ToString();
            var cover = string.Empty;
            var hasCover = item.CoverUrls?.Any() ?? false;
            if (hasCover)
            {
                cover = item.CoverUrls.First();
            }
            else
            {
                cover = item.Cover;
            }

            Title = item.Title;
            Description = item.Description;
            Publisher = new UserViewModel(item.Name, userId: item.UserId);
            ViewCount = _numberToolkit.GetCountText(item.ViewCount);
            ReplyCount = _numberToolkit.GetCountText(item.ReplyCount);
            LikeCount = _numberToolkit.GetCountText(item.LikeCount);

            LimitCover(cover);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleViewModel"/> class.
        /// </summary>
        /// <param name="item">收藏文章.</param>
        public ArticleViewModel(FavoriteArticleItem item)
            : this()
        {
            Id = item.Id.ToString();
            var cover = string.Empty;
            var hasCover = item.Images?.Any() ?? false;
            if (hasCover)
            {
                cover = item.Images.First();
            }
            else
            {
                cover = item.Banner;
            }

            Title = item.Title;
            Description = item.Summary;
            Publisher = new UserViewModel(item.PublisherName, userId: item.PublisherId);
            CollectTime = DateTimeOffset.FromUnixTimeSeconds(item.CollectTime).ToString("yy/MM/dd");

            LimitCover(cover);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArticleViewModel"/> class.
        /// </summary>
        protected ArticleViewModel()
        {
            CategoryCollection = new ObservableCollection<ArticleCategory>();
            _controller = Controller.Uwp.BiliController.Instance;
            ServiceLocator.Instance.LoadService(out _numberToolkit)
                                   .LoadService(out _resourceToolkit);
        }

        /// <summary>
        /// 初始化文章内容.
        /// </summary>
        /// <returns><see cref="Task"/>.</returns>
        public async Task InitializeArticleContentAsync()
        {
            if (string.IsNullOrEmpty(ArticleContent) && !IsLoading)
            {
                IsLoading = true;
                try
                {
                    var content = await _controller.GetArticleContentAsync(Convert.ToInt32(Id));
                    ArticleContent = content;
                }
                catch (ServiceException ex)
                {
                    IsError = true;
                    ErrorText = $"{_resourceToolkit.GetLocaleString(LanguageNames.RequestArticleFailed)}\n{ex.Error?.Message ?? ex.Message}";
                }
                catch (Exception invalidEx)
                {
                    IsError = true;
                    ErrorText = invalidEx.Message;
                }

                IsLoading = false;
            }
        }

        /// <summary>
        /// 限制图片分辨率以减轻UI和内存压力.
        /// </summary>
        private void LimitCover(string coverUrl)
        {
            if (!string.IsNullOrEmpty(coverUrl))
            {
                CoverUrl = coverUrl + "@400w_250h_1c_100q.jpg";
            }
        }
    }
}

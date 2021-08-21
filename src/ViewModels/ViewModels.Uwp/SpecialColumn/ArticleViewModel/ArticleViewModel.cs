// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.BiliBili;

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
        protected ArticleViewModel()
        {
            CategoryCollection = new ObservableCollection<ArticleCategory>();
            ServiceLocator.Instance.LoadService(out _numberToolkit);
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

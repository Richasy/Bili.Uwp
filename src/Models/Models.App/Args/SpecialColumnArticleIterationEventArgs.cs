// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Bili.Models.BiliBili;

namespace Bili.Models.App.Args
{
    /// <summary>
    /// 专栏文章列表更改数据参数.
    /// </summary>
    public class SpecialColumnArticleIterationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentaryArticleIterationEventArgs"/> class.
        /// </summary>
        protected SpecialColumnArticleIterationEventArgs()
        {
        }

        /// <summary>
        /// 文章列表.
        /// </summary>
        public List<Article> Articles { get; set; }

        /// <summary>
        /// 下一页码.
        /// </summary>
        public int NextPageNumber { get; set; }

        /// <summary>
        /// 分类Id.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 根据推荐文章的响应结果创建事件参数.
        /// </summary>
        /// <param name="response"><see cref="ArticleRecommendResponse"/>.</param>
        /// <param name="nextPageNumber">下一页页码.</param>
        /// <returns><see cref="SpecialColumnArticleIterationEventArgs"/>.</returns>
        public static SpecialColumnArticleIterationEventArgs Create(ArticleRecommendResponse response, int nextPageNumber)
        {
            var args = new SpecialColumnArticleIterationEventArgs();
            args.Articles = response.Articles;
            args.CategoryId = 0;
            args.NextPageNumber = nextPageNumber;
            return args;
        }

        /// <summary>
        /// 根据分类文章的结果创建事件参数.
        /// </summary>
        /// <param name="list">分类文章列表.</param>
        /// <param name="categoryId">分类ID.</param>
        /// <param name="nextPageNumber">下一页码.</param>
        /// <returns><see cref="SpecialColumnArticleIterationEventArgs"/>.</returns>
        public static SpecialColumnArticleIterationEventArgs Create(List<Article> list, int categoryId, int nextPageNumber)
        {
            var args = new SpecialColumnArticleIterationEventArgs();
            args.Articles = list;
            args.CategoryId = categoryId;
            args.NextPageNumber = nextPageNumber;
            return args;
        }
    }
}

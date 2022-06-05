// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using Bili.Models.BiliBili;
using Bili.Models.Data.Article;

namespace Bili.Adapter.Interfaces
{
    /// <summary>
    /// 文章数据适配器接口.
    /// </summary>
    public interface IArticleAdapter
    {
        /// <summary>
        /// 将专栏文章 <see cref="Article"/> 转换成文章信息.
        /// </summary>
        /// <param name="article">专栏文章.</param>
        /// <returns><see cref="ArticleInformation"/>.</returns>
        ArticleInformation ConvertToArticleInformation(Article article);

        /// <summary>
        /// 将专栏文章搜索结果 <see cref="ArticleSearchItem"/> 转换成文章信息.
        /// </summary>
        /// <param name="item">专栏文章搜索结果.</param>
        /// <returns><see cref="ArticleInformation"/>.</returns>
        ArticleInformation ConvertToArticleInformation(ArticleSearchItem item);

        /// <summary>
        /// 将收藏的专栏文章 <see cref="ArticleSearchItem"/> 转换成文章信息.
        /// </summary>
        /// <param name="item">收藏的专栏文章.</param>
        /// <returns><see cref="ArticleInformation"/>.</returns>
        ArticleInformation ConvertToArticleInformation(FavoriteArticleItem item);

        /// <summary>
        /// 将专栏推荐响应结果 <see cref="ArticleRecommendResponse"/> 转换成分区文章视图.
        /// </summary>
        /// <param name="response">专栏推荐响应结果.</param>
        /// <returns><see cref="ArticlePartitionView"/>.</returns>
        ArticlePartitionView ConvertToArticlePartitionView(ArticleRecommendResponse response);

        /// <summary>
        /// 将专栏文章列表转换成分区文章视图.
        /// </summary>
        /// <param name="articles">文章列表.</param>
        /// <returns><see cref="ArticlePartitionView"/>.</returns>
        ArticlePartitionView ConvertToArticlePartitionView(IEnumerable<Article> articles);

        /// <summary>
        /// 将收藏的专栏文章响应 <see cref="ArticleFavoriteListResponse"/> 转换为文章集.
        /// </summary>
        /// <param name="response">收藏的专栏文章响应.</param>
        /// <returns><see cref="ArticleSet"/>.</returns>
        ArticleSet ConvertToArticleSet(ArticleFavoriteListResponse response);
    }
}

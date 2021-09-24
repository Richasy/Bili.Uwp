// Copyright (c) GodLeaveMe. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.Lib.Interfaces
{
    /// <summary>
    /// 文章专栏数据处理.
    /// </summary>
    public interface ISpecialColumnProvider
    {
        /// <summary>
        /// 获取专栏的全部分区/标签列表.
        /// </summary>
        /// <returns>标签列表.</returns>
        Task<List<ArticleCategory>> GetCategoriesAsync();

        /// <summary>
        /// 获取推荐的文章.
        /// </summary>
        /// <param name="pageNumber">页码.</param>
        /// <param name="pageSize">每页数据，默认是20.</param>
        /// <returns>推荐文章响应结果.</returns>
        Task<ArticleRecommendResponse> GetRecommendArticlesAsync(int pageNumber, int pageSize = 20);

        /// <summary>
        /// 获取分类下的文章.
        /// </summary>
        /// <param name="categoryId">分类Id.</param>
        /// <param name="pageNumber">页码.</param>
        /// <param name="sortType">排序方式.</param>
        /// <param name="pageSize">每页数据，默认是20.</param>
        /// <returns>文章列表.</returns>
        Task<List<Article>> GetCategoryArticlesAsync(int categoryId, int pageNumber, ArticleSortType sortType, int pageSize = 20);

        /// <summary>
        /// 获取文章内容.
        /// </summary>
        /// <param name="articleId">文章Id.</param>
        /// <returns>文章内容.</returns>
        Task<string> GetArticleContentAsync(int articleId);
    }
}

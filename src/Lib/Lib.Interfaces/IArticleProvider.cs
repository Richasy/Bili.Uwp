// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Threading.Tasks;
using Bili.Models.Data.Article;
using Bili.Models.Data.Community;
using Bili.Models.Enums;

namespace Bili.Lib.Interfaces
{
    /// <summary>
    /// 文章专栏数据处理.
    /// </summary>
    public interface IArticleProvider
    {
        /// <summary>
        /// 获取专栏的全部分区/标签列表.
        /// </summary>
        /// <returns>标签列表.</returns>
        Task<IEnumerable<Partition>> GetPartitionsAsync();

        /// <summary>
        /// 获取推荐的文章.
        /// </summary>
        /// <param name="partitionId">分区标识符.</param>
        /// <param name="sortType">排序方式.</param>
        /// <returns>推荐文章响应结果.</returns>
        Task<ArticlePartitionView> GetPartitionArticlesAsync(string partitionId, ArticleSortType sortType);

        /// <summary>
        /// 获取文章内容.
        /// </summary>
        /// <param name="articleId">文章Id.</param>
        /// <returns>文章内容.</returns>
        Task<string> GetArticleContentAsync(string articleId);

        /// <summary>
        /// 重置分区请求状态.
        /// </summary>
        /// <param name="partitionId">分区 Id.</param>
        void ResetPartitionStatus(string partitionId);
    }
}

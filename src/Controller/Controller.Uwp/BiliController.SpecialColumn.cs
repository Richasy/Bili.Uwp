// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.App.Other;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using static Richasy.Bili.Models.App.Constants.ControllerConstants;

namespace Richasy.Bili.Controller.Uwp
{
    /// <summary>
    /// 控制器中处理专栏文档的部分.
    /// </summary>
    public partial class BiliController
    {
        /// <summary>
        /// 获取专栏分类.
        /// </summary>
        /// <returns>分类列表.</returns>
        public async Task<List<ArticleCategory>> GetSpecialColumnCategoriesAsync()
        {
            var cacheData = await _fileToolkit.ReadLocalDataAsync<LocalCache<List<ArticleCategory>>>(Names.DocumentaryCategories, folderName: Names.ServerFolder);
            var needRequest = true;
            List<ArticleCategory> data;
            if (cacheData != null)
            {
                needRequest = cacheData.ExpiryTime < DateTimeOffset.Now;
            }

            if (needRequest)
            {
                ThrowWhenNetworkUnavaliable();
                try
                {
                    data = await _specialColumnProvider.GetCategoriesAsync();
                    var localCache = new LocalCache<List<ArticleCategory>>(DateTimeOffset.Now.AddDays(1), data);
                    await _fileToolkit.WriteLocalDataAsync(Names.DocumentaryCategories, localCache, Names.ServerFolder);
                }
                catch (Exception ex)
                {
                    _loggerModule.LogError(ex);
                    throw;
                }
            }
            else
            {
                data = cacheData.Data;
            }

            return data;
        }

        /// <summary>
        /// 请求分类下的文章.
        /// </summary>
        /// <param name="categoryId">分类Id.</param>
        /// <param name="pageNumber">页码.</param>
        /// <param name="sortType">排序类型.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestCategoryArticlesAsync(int categoryId, int pageNumber, ArticleSortType sortType)
        {
            ThrowWhenNetworkUnavaliable();
            try
            {
                var data = await _specialColumnProvider.GetCategoryArticlesAsync(categoryId, pageNumber, sortType);
                var iterationArgs = SpecialColumnArticleIterationEventArgs.Create(data, categoryId, pageNumber + 1);
                SpecialColumnArticleIteration?.Invoke(this, iterationArgs);
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, pageNumber > 1);
                if (pageNumber == 1)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 请求推荐文章.
        /// </summary>
        /// <param name="pageNumber">页码.</param>
        /// <returns><see cref="Task"/>.</returns>
        public async Task RequestRecommendArticlesAsync(int pageNumber)
        {
            ThrowWhenNetworkUnavaliable();
            try
            {
                var data = await _specialColumnProvider.GetRecommendArticlesAsync(pageNumber);
                var additionalArgs = SpecialColumnAdditionalDataChangedEventArgs.Create(data);
                if (additionalArgs != null && pageNumber == 1)
                {
                    SpecialColumnAdditionalDataChanged?.Invoke(this, additionalArgs);
                }

                var iterationArgs = SpecialColumnArticleIterationEventArgs.Create(data, pageNumber + 1);
                SpecialColumnArticleIteration?.Invoke(this, iterationArgs);
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex, pageNumber > 1);
                if (pageNumber == 1)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// 获取文章内容.
        /// </summary>
        /// <param name="articleId">文章Id.</param>
        /// <returns>文章内容.</returns>
        public async Task<string> GetArticleContentAsync(int articleId)
        {
            ThrowWhenNetworkUnavaliable();

            try
            {
                return await _specialColumnProvider.GetArticleContentAsync(articleId);
            }
            catch (Exception ex)
            {
                _loggerModule.LogError(ex);
                throw;
            }
        }
    }
}

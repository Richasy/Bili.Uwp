// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 专栏附加数据（横幅，排行榜）更改事件.
    /// </summary>
    public class SpecialColumnAdditionalDataChangedEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialColumnAdditionalDataChangedEventArgs"/> class.
        /// </summary>
        /// <param name="response">推荐的响应结果.</param>
        protected SpecialColumnAdditionalDataChangedEventArgs(ArticleRecommendResponse response)
        {
            Banners = response.Banners;
            Ranks = response.Ranks;
        }

        /// <summary>
        /// 横幅.
        /// </summary>
        public List<PartitionBanner> Banners { get; set; }

        /// <summary>
        /// 排行榜.
        /// </summary>
        public List<Article> Ranks { get; set; }

        /// <summary>
        /// 从推荐数据的响应结果创建事件参数.
        /// </summary>
        /// <param name="response"><see cref="ArticleRecommendResponse"/>.</param>
        /// <returns>包含所需数据的事件参数，如果没有所需数据，则为<c>null</c>.</returns>
        public static SpecialColumnAdditionalDataChangedEventArgs Create(ArticleRecommendResponse response)
        {
            if (response.Banners == null || response.Ranks == null)
            {
                return null;
            }

            return new SpecialColumnAdditionalDataChangedEventArgs(response);
        }
    }
}

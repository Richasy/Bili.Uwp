// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 搜索模块元数据事件参数.
    /// </summary>
    public class SearchMetaEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchMetaEventArgs"/> class.
        /// </summary>
        /// <param name="response">响应结果.</param>
        public SearchMetaEventArgs(ComprehensiveSearchResultResponse response)
        {
            MetaList = new Dictionary<SearchModuleType, int>();
            foreach (var item in response.SubModuleList)
            {
                MetaList.Add((SearchModuleType)item.Type, item.Total);
            }

            Keyword = response.Keyword;
        }

        /// <summary>
        /// 搜索模块元数据.
        /// </summary>
        public Dictionary<SearchModuleType, int> MetaList { get; set; }

        /// <summary>
        /// 搜索关键词.
        /// </summary>
        public string Keyword { get; set; }
    }
}

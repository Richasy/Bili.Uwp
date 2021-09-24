// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Collections.Generic;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums.App;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// PGC收藏夹内容更新事件参数.
    /// </summary>
    public class FavoritePgcIterationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FavoritePgcIterationEventArgs"/> class.
        /// </summary>
        /// <param name="response">响应结果.</param>
        /// <param name="pageNumebr">页码.</param>
        /// <param name="type">类型.</param>
        public FavoritePgcIterationEventArgs(PgcFavoriteListResponse response, int pageNumebr, FavoriteType type)
        {
            List = response.FollowList;
            TotalCount = response.Total;
            HasMore = response.HasMore == 1;
            Type = type;
            if (HasMore)
            {
                NextPageNumber = pageNumebr + 1;
            }
        }

        /// <summary>
        /// 条目列表.
        /// </summary>
        public List<FavoritePgcItem> List { get; set; }

        /// <summary>
        /// 下一页页码.
        /// </summary>
        public int NextPageNumber { get; set; }

        /// <summary>
        /// 是否还有更多.
        /// </summary>
        public bool HasMore { get; set; }

        /// <summary>
        /// 类型.
        /// </summary>
        public FavoriteType Type { get; set; }

        /// <summary>
        /// 总数.
        /// </summary>
        public int TotalCount { get; set; }
    }
}

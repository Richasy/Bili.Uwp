// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Collections.Generic;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 相关用户（粉丝，关注）列表更改事件参数.
    /// </summary>
    public class RelatedUserIterationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RelatedUserIterationEventArgs"/> class.
        /// </summary>
        /// <param name="response">粉丝响应结果.</param>
        /// <param name="pageNumber">页码.</param>
        /// <param name="userId">用户Id.</param>
        public RelatedUserIterationEventArgs(RelatedUserResponse response, int pageNumber, int userId)
        {
            List = response.UserList;
            NextPageNumber = pageNumber + 1;
            TotalCount = response.TotalCount;
            UserId = userId;
        }

        /// <summary>
        /// 粉丝列表.
        /// </summary>
        public List<RelatedUser> List { get; set; }

        /// <summary>
        /// 下一页页码.
        /// </summary>
        public int NextPageNumber { get; set; }

        /// <summary>
        /// 总数.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 查询的用户Id.
        /// </summary>
        public int UserId { get; set; }
    }
}

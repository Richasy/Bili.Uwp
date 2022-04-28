// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Bilibili.App.Interfaces.V1;

namespace Bili.Models.App.Args
{
    /// <summary>
    /// 用户空间视频搜索迭代事件参数.
    /// </summary>
    public class UserSpaceSearchVideoIterationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSpaceSearchVideoIterationEventArgs"/> class.
        /// </summary>
        /// <param name="reply">响应结果.</param>
        /// <param name="userId">对应的用户Id.</param>
        public UserSpaceSearchVideoIterationEventArgs(SearchArchiveReply reply, int userId)
        {
            UserId = userId;
            List = reply.Archives.ToList();
            TotalCount = Convert.ToInt32(reply.Total);
        }

        /// <summary>
        /// 用户Id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 视频列表.
        /// </summary>
        public List<Arc> List { get; set; }

        /// <summary>
        /// 全部个数.
        /// </summary>
        public int TotalCount { get; set; }
    }
}

// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 用户空间视频更改事件参数.
    /// </summary>
    public class UserSpaceVideoIterationEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserSpaceVideoIterationEventArgs"/> class.
        /// </summary>
        /// <param name="set">视频集.</param>
        /// <param name="userId">用户Id.</param>
        public UserSpaceVideoIterationEventArgs(UserSpaceVideoSet set, int userId)
        {
            UserId = userId;
            List = set.List;
            TotalCount = set.Count;

            if (set.List != null && set.List.Count > 0)
            {
                NextOffsetId = set.List.Last().Id;
            }
        }

        /// <summary>
        /// 用户Id.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 视频列表.
        /// </summary>
        public List<UserSpaceVideoItem> List { get; set; }

        /// <summary>
        /// 总个数.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 下次查询的偏移Id.
        /// </summary>
        public string NextOffsetId { get; set; }
    }
}

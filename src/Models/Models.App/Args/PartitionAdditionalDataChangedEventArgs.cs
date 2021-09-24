// Copyright (c) GodLeaveMe. All rights reserved.

using System;
using System.Collections.Generic;
using Richasy.Bili.Models.BiliBili;

namespace Richasy.Bili.Models.App.Args
{
    /// <summary>
    /// 分区附加数据改变时的事件参数.
    /// </summary>
    public class PartitionAdditionalDataChangedEventArgs : PartitionEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PartitionAdditionalDataChangedEventArgs"/> class.
        /// </summary>
        /// <param name="subPartitionId">子分区Id.</param>
        /// <param name="requestDateTime">请求发生的时间.</param>
        /// <param name="bannerList">横幅列表.</param>
        /// <param name="tagList">标签列表.</param>
        public PartitionAdditionalDataChangedEventArgs(
            int subPartitionId,
            DateTimeOffset requestDateTime,
            IEnumerable<PartitionBanner> bannerList = null,
            IEnumerable<Tag> tagList = null)
            : base(subPartitionId, requestDateTime)
        {
            this.BannerList = bannerList;
            this.TagList = tagList;
        }

        /// <summary>
        /// 横幅列表.
        /// </summary>
        public IEnumerable<PartitionBanner> BannerList { get; set; }

        /// <summary>
        /// 热门标签列表.
        /// </summary>
        public IEnumerable<Tag> TagList { get; set; }
    }
}

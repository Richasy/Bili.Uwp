// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Pgc
{
    /// <summary>
    /// 时间线视图.
    /// </summary>
    public sealed class TimelineView
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimelineView"/> class.
        /// </summary>
        /// <param name="title">标题.</param>
        /// <param name="description">描述文本.</param>
        /// <param name="timelines">时间线数据.</param>
        public TimelineView(
            string title,
            string description,
            IEnumerable<TimelineInformation> timelines)
        {
            Title = title;
            Description = description;
            Timelines = timelines;
        }

        /// <summary>
        /// 标题.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// 描述.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// 时间线信息集合.
        /// </summary>
        public IEnumerable<TimelineInformation> Timelines { get; }
    }
}

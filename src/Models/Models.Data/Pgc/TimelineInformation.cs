// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;

namespace Bili.Models.Data.Pgc
{
    /// <summary>
    /// 时间线条目信息.
    /// </summary>
    public sealed class TimelineInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimelineInformation"/> class.
        /// </summary>
        /// <param name="date">发布日期.</param>
        /// <param name="dayOfWeek">周几.</param>
        /// <param name="isToday">是否是今天.</param>
        /// <param name="seasons">剧集列表.</param>
        public TimelineInformation(
            string date,
            string dayOfWeek,
            bool isToday,
            IEnumerable<SeasonInformation> seasons = default)
        {
            Date = date;
            DayOfWeek = dayOfWeek;
            IsToday = isToday;
            Seasons = seasons;
        }

        /// <summary>
        /// 发布时间.
        /// </summary>
        public string Date { get; }

        /// <summary>
        /// 发布时间在周几.
        /// </summary>
        public string DayOfWeek { get; }

        /// <summary>
        /// 是否是今天.
        /// </summary>
        public bool IsToday { get; }

        /// <summary>
        /// 下属剧集.
        /// </summary>
        public IEnumerable<SeasonInformation> Seasons { get; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is TimelineInformation information && Date == information.Date;

        /// <inheritdoc/>
        public override int GetHashCode() => Date.GetHashCode();
    }
}

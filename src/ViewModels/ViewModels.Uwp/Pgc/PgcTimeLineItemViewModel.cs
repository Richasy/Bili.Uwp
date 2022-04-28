// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.BiliBili;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// PGC时间线条目视图模型.
    /// </summary>
    public class PgcTimeLineItemViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PgcTimeLineItemViewModel"/> class.
        /// </summary>
        /// <param name="item">条目.</param>
        /// <param name="isSelected">是否选中.</param>
        public PgcTimeLineItemViewModel(PgcTimeLineItem item)
        {
            IsToday = item.IsToday == 1;
            Date = item.Date;
            DayOfWeek = ConvertDayOfWeek(item.DayOfWeek);
            EpisodeCollection = new ObservableCollection<SeasonViewModel>();
            IsShowEmpty = item.Episodes == null || item.Episodes.Count == 0;
            EmptyText = item.HolderText;

            if (!IsShowEmpty)
            {
                item.Episodes.ForEach(p => EpisodeCollection.Add(new SeasonViewModel(p)));
            }
        }

        /// <summary>
        /// 是否是今天.
        /// </summary>
        [Reactive]
        public bool IsToday { get; set; }

        /// <summary>
        /// 日期.
        /// </summary>
        [Reactive]
        public string Date { get; set; }

        /// <summary>
        /// 星期几.
        /// </summary>
        [Reactive]
        public string DayOfWeek { get; set; }

        /// <summary>
        /// 剧集集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<SeasonViewModel> EpisodeCollection { get; set; }

        /// <summary>
        /// 是否显示空白.
        /// </summary>
        [Reactive]
        public bool IsShowEmpty { get; set; }

        /// <summary>
        /// 空白占位符.
        /// </summary>
        [Reactive]
        public string EmptyText { get; set; }

        private string ConvertDayOfWeek(int day)
        {
            var dayOfWeek = string.Empty;
            switch (day)
            {
                case 1:
                    dayOfWeek = "一";
                    break;
                case 2:
                    dayOfWeek = "二";
                    break;
                case 3:
                    dayOfWeek = "三";
                    break;
                case 4:
                    dayOfWeek = "四";
                    break;
                case 5:
                    dayOfWeek = "五";
                    break;
                case 6:
                    dayOfWeek = "六";
                    break;
                case 7:
                    dayOfWeek = "日";
                    break;
                default:
                    break;
            }

            return "周" + dayOfWeek;
        }
    }
}

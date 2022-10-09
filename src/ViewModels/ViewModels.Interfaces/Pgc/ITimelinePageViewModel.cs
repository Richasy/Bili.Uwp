// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using System.ComponentModel;
using Bili.Models.Data.Pgc;
using Bili.Models.Enums;

namespace Bili.ViewModels.Interfaces.Pgc
{
    /// <summary>
    /// 时间线页面视图模型.
    /// </summary>
    public interface ITimelinePageViewModel : INotifyPropertyChanged, IInitializeViewModel, IReloadViewModel, IErrorViewModel
    {
        /// <summary>
        /// 时间线集合.
        /// </summary>
        public ObservableCollection<TimelineInformation> Timelines { get; }

        /// <summary>
        /// 标题.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 描述.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// 设置 PGC 类型.
        /// </summary>
        /// <param name="type">PGC 类型.</param>
        void SetType(PgcType type);
    }
}

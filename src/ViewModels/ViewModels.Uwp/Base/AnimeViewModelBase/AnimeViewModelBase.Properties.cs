// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 动漫视图模型基类.
    /// </summary>
    public partial class AnimeViewModelBase
    {
        /// <summary>
        /// 标签集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<PgcTabViewModel> TabCollection { get; set; }

        /// <summary>
        /// 当前选中标签.
        /// </summary>
        [Reactive]
        public PgcTabViewModel CurrentTab { get; set; }

        /// <summary>
        /// 时间线集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<PgcTimeLineItemViewModel> TimeLineCollection { get; set; }

        /// <summary>
        /// 时间线标题.
        /// </summary>
        [Reactive]
        public string TimeLineTitle { get; set; }

        /// <summary>
        /// 时间线副标题.
        /// </summary>
        [Reactive]
        public string TimeLineSubtitle { get; set; }

        /// <summary>
        /// 时间线是否正在加载.
        /// </summary>
        [Reactive]
        public bool IsTimeLineInitializing { get; set; }

        /// <summary>
        /// 时间线请求是否出错.
        /// </summary>
        [Reactive]
        public bool IsTimeLineError { get; set; }

        /// <summary>
        /// 时间线请求错误文本.
        /// </summary>
        [Reactive]
        public string TimeLineErrorText { get; set; }
    }
}

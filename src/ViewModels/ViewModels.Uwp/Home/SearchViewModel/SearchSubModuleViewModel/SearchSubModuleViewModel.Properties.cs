// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.Enums;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 搜索模块视图模型.
    /// </summary>
    public partial class SearchSubModuleViewModel
    {
        /// <summary>
        /// 类型.
        /// </summary>
        [Reactive]
        public SearchModuleType Type { get; set; }

        /// <summary>
        /// 搜索关键词.
        /// </summary>
        [Reactive]
        public string Keyword { get; set; }

        /// <summary>
        /// 页码.
        /// </summary>
        [Reactive]
        public int PageNumber { get; set; }

        /// <summary>
        /// 是否启用，即是否包含搜索结果.
        /// </summary>
        [Reactive]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 当前排序方式.
        /// </summary>
        [Reactive]
        public string CurrentOrder { get; set; }

        /// <summary>
        /// 视频/直播集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<VideoViewModel> VideoCollection { get; set; }

        /// <summary>
        /// PGC内容集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<SeasonViewModel> PgcCollection { get; set; }

        /// <summary>
        /// 文章集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<ArticleViewModel> ArticleCollection { get; set; }

        /// <summary>
        /// 是否已经全部加载完成.
        /// </summary>
        [Reactive]
        public bool IsLoadCompleted { get; set; }
    }
}

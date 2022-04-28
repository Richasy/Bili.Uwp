// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.App.Other;
using Bili.Models.Enums;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
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
        public string Keyword => SearchModuleViewModel.Instance.Keyword;

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
        /// 条目总数.
        /// </summary>
        [Reactive]
        public int Total { get; set; }

        /// <summary>
        /// 当前排序方式.
        /// </summary>
        [Reactive]
        public KeyValue<string> CurrentOrder { get; set; }

        /// <summary>
        /// 当前选中的分区.
        /// </summary>
        [Reactive]
        public KeyValue<string> CurrentPartitionId { get; set; }

        /// <summary>
        /// 当前选中的时长.
        /// </summary>
        [Reactive]
        public KeyValue<string> CurrentDuration { get; set; }

        /// <summary>
        /// 当前选中的用户类型.
        /// </summary>
        [Reactive]
        public KeyValue<string> CurrentUserType { get; set; }

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
        /// 用户集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<UserViewModel> UserCollection { get; set; }

        /// <summary>
        /// 排序值集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<KeyValue<string>> OrderCollection { get; set; }

        /// <summary>
        /// 视频时长值集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<KeyValue<string>> VideoDurationCollection { get; set; }

        /// <summary>
        /// 视频分区值集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<KeyValue<string>> PartitionCollection { get; set; }

        /// <summary>
        /// 文章分区值集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<KeyValue<string>> ArticlePartitionCollection { get; set; }

        /// <summary>
        /// 用户类型值集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<KeyValue<string>> UserTypeCollection { get; set; }

        /// <summary>
        /// 是否显示空白提示.
        /// </summary>
        [Reactive]
        public bool IsShowEmpty { get; set; }

        /// <summary>
        /// 是否已经全部加载完成.
        /// </summary>
        [Reactive]
        public bool IsLoadCompleted { get; set; }
    }
}

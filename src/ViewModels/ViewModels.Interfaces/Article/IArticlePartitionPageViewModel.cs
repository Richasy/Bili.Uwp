// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using Bili.Models.Data.Community;
using Bili.Models.Enums;
using Bili.ViewModels.Interfaces.Common;
using CommunityToolkit.Mvvm.Input;

namespace Bili.ViewModels.Interfaces.Article
{
    /// <summary>
    /// 文章分区页面视图模型的接口定义.
    /// </summary>
    public interface IArticlePartitionPageViewModel : IInformationFlowViewModel<IArticleItemViewModel>
    {
        /// <summary>
        /// 横幅集合.
        /// </summary>
        ObservableCollection<IBannerViewModel> Banners { get; }

        /// <summary>
        /// 横幅集合.
        /// </summary>
        ObservableCollection<IArticleItemViewModel> Ranks { get; }

        /// <summary>
        /// 子分区集合.
        /// </summary>
        ObservableCollection<Partition> Partitions { get; }

        /// <summary>
        /// 文章排序方式集合.
        /// </summary>
        ObservableCollection<ArticleSortType> SortTypes { get; }

        /// <summary>
        /// 选中子分区命令.
        /// </summary>
        IRelayCommand<Partition> SelectPartitionCommand { get; }

        /// <summary>
        /// 当前选中的子分区.
        /// </summary>
        Partition CurrentPartition { get; set; }

        /// <summary>
        /// 排序方式.
        /// </summary>
        ArticleSortType SortType { get; set; }

        /// <summary>
        /// 是否为推荐子分区.
        /// </summary>
        bool IsRecommendPartition { get; }

        /// <summary>
        /// 是否显示横幅内容.
        /// </summary>
        bool IsShowBanner { get; }
    }
}

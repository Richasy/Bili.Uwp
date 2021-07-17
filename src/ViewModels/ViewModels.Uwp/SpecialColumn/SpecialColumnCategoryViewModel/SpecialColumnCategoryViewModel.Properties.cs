// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Models.Enums;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 专栏分类视图模型.
    /// </summary>
    public partial class SpecialColumnCategoryViewModel
    {
        private readonly IResourceToolkit _resourceToolkit;
        private readonly BiliController _controller;

        private int _pageNumber = 1;

        /// <summary>
        /// 标识该分类是否已经加载过数据.
        /// </summary>
        public bool IsRequested => _pageNumber > 1;

        /// <summary>
        /// 是否为推荐分类.
        /// </summary>
        [Reactive]
        public bool IsRecommend { get; set; }

        /// <summary>
        /// 是否显示横幅.
        /// </summary>
        [Reactive]
        public bool IsShowBanner { get; set; }

        /// <summary>
        /// 是否显示排行榜.
        /// </summary>
        [Reactive]
        public bool IsShowRank { get; set; }

        /// <summary>
        /// 是否处于激活状态.
        /// </summary>
        [Reactive]
        public bool IsActive { get; internal set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <summary>
        /// 分类Id.
        /// </summary>
        [Reactive]
        public int Id { get; set; }

        /// <summary>
        /// 父分类Id.
        /// </summary>
        [Reactive]
        public int ParentId { get; set; }

        /// <summary>
        /// 分类源数据.
        /// </summary>
        [Reactive]
        public ArticleCategory Category { get; set; }

        /// <summary>
        /// 是否在执行初始化数据加载.
        /// </summary>
        [Reactive]
        public bool IsInitializeLoading { get; set; }

        /// <summary>
        /// 是否在执行增量加载.
        /// </summary>
        [Reactive]
        public bool IsDeltaLoading { get; set; }

        /// <summary>
        /// 当前排序方式.
        /// </summary>
        [Reactive]
        public ArticleSortType CurrentSortType { get; set; }

        /// <summary>
        /// 当前选中的分类.
        /// </summary>
        [Reactive]
        public SpecialColumnCategoryViewModel CurrentCategory { get; set; }

        /// <summary>
        /// 子分类.
        /// </summary>
        [Reactive]
        public ObservableCollection<SpecialColumnCategoryViewModel> Children { get; set; }

        /// <summary>
        /// 文章列表.
        /// </summary>
        [Reactive]
        public ObservableCollection<ArticleViewModel> ArticleCollection { get; set; }

        /// <summary>
        /// 横幅集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<BannerViewModel> BannerCollection { get; set; }

        /// <summary>
        /// 排行榜集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<ArticleViewModel> RankCollection { get; set; }

        /// <summary>
        /// 排序方式集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<ArticleSortType> SortTypeCollection { get; set; }

        /// <summary>
        /// 是否显示错误信息.
        /// </summary>
        [Reactive]
        public bool IsError { get; set; }

        /// <summary>
        /// 错误文本.
        /// </summary>
        [Reactive]
        public string ErrorText { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is SpecialColumnCategoryViewModel model && Id == model.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => 2108858624 + Id.GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => Title;
    }
}

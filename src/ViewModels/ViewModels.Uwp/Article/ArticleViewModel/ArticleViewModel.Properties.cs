// Copyright (c) Richasy. All rights reserved.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using Bili.Controller.Uwp;
using Bili.Models.BiliBili;
using Bili.Toolkit.Interfaces;
using ReactiveUI.Fody.Helpers;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 文章视图模型.
    /// </summary>
    public partial class ArticleViewModel
    {
        private readonly INumberToolkit _numberToolkit;
        private readonly IResourceToolkit _resourceToolkit;
        private readonly BiliController _controller;

        /// <summary>
        /// 封面.
        /// </summary>
        [Reactive]
        public string CoverUrl { get; set; }

        /// <summary>
        /// 文章标签.
        /// </summary>
        [Reactive]
        public ObservableCollection<ArticleCategory> CategoryCollection { get; set; }

        /// <summary>
        /// 标题.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <summary>
        /// 描述.
        /// </summary>
        [Reactive]
        public string Description { get; set; }

        /// <summary>
        /// 发布者信息.
        /// </summary>
        [Reactive]
        public UserViewModel Publisher { get; set; }

        /// <summary>
        /// 发布时间.
        /// </summary>
        [Reactive]
        public string PublishTime { get; set; }

        /// <summary>
        /// 查看次数.
        /// </summary>
        [Reactive]
        public string ViewCount { get; set; }

        /// <summary>
        /// 回复次数.
        /// </summary>
        [Reactive]
        public string ReplyCount { get; set; }

        /// <summary>
        /// 点赞数.
        /// </summary>
        [Reactive]
        public string LikeCount { get; set; }

        /// <summary>
        /// 文章Id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 收藏时间.
        /// </summary>
        [Reactive]
        public string CollectTime { get; set; }

        /// <summary>
        /// 原始封面地址.
        /// </summary>
        public string SourceCoverUrl { get; set; }

        /// <summary>
        /// 文章内容.
        /// </summary>
        [Reactive]
        public string ArticleContent { get; set; }

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

        /// <summary>
        /// 是否正在加载文章.
        /// </summary>
        [Reactive]
        public bool IsLoading { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is ArticleViewModel model && Id == model.Id;

        /// <inheritdoc/>
        public override int GetHashCode() => 2108858624 + EqualityComparer<string>.Default.GetHashCode(Id);
    }
}

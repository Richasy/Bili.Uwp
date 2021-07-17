// Copyright (c) Richasy. All rights reserved.

using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 文章视图模型.
    /// </summary>
    public partial class ArticleViewModel
    {
        private readonly INumberToolkit _numberToolkit;

        /// <summary>
        /// 封面.
        /// </summary>
        [Reactive]
        public string Cover { get; set; }

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
        /// 作者.
        /// </summary>
        [Reactive]
        public string Publisher { get; set; }

        /// <summary>
        /// 作者头像.
        /// </summary>
        [Reactive]
        public string PublisherAvatar { get; set; }

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
        /// 文章原始数据.
        /// </summary>
        public Article Source { get; private set; }
    }
}

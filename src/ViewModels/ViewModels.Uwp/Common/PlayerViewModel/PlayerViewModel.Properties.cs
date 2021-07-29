// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.ObjectModel;
using Bilibili.App.View.V1;
using ReactiveUI.Fody.Helpers;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.Toolkit.Interfaces;

namespace Richasy.Bili.ViewModels.Uwp
{
    /// <summary>
    /// 播放器视图模型.
    /// </summary>
    public partial class PlayerViewModel
    {
        private readonly INumberToolkit _numberToolkit;
        private readonly IResourceToolkit _resourceToolkit;
        private ViewReply _detail;

        /// <summary>
        /// 单例.
        /// </summary>
        public static PlayerViewModel Instance { get; } = new Lazy<PlayerViewModel>(() => new PlayerViewModel()).Value;

        /// <summary>
        /// 标题.
        /// </summary>
        [Reactive]
        public string Title { get; set; }

        /// <summary>
        /// 副标题，发布时间.
        /// </summary>
        [Reactive]
        public string Subtitle { get; set; }

        /// <summary>
        /// 说明.
        /// </summary>
        [Reactive]
        public string Description { get; set; }

        /// <summary>
        /// AV Id.
        /// </summary>
        [Reactive]
        public string AvId { get; set; }

        /// <summary>
        /// BV Id.
        /// </summary>
        [Reactive]
        public string BvId { get; set; }

        /// <summary>
        /// 播放数.
        /// </summary>
        [Reactive]
        public string PlayCount { get; set; }

        /// <summary>
        /// 弹幕数.
        /// </summary>
        [Reactive]
        public string DanmakuCount { get; set; }

        /// <summary>
        /// 点赞数.
        /// </summary>
        [Reactive]
        public string LikeCount { get; set; }

        /// <summary>
        /// 硬币数.
        /// </summary>
        [Reactive]
        public string CoinCount { get; set; }

        /// <summary>
        /// 收藏数.
        /// </summary>
        [Reactive]
        public string FavoriteCount { get; set; }

        /// <summary>
        /// 转发数.
        /// </summary>
        [Reactive]
        public string ShareCount { get; set; }

        /// <summary>
        /// 评论数.
        /// </summary>
        [Reactive]
        public string ReplyCount { get; set; }

        /// <summary>
        /// 发布者.
        /// </summary>
        [Reactive]
        public PublisherViewModel Publisher { get; set; }

        /// <summary>
        /// 关联视频集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<VideoViewModel> RelatedVideoCollection { get; set; }

        /// <summary>
        /// 分集视频集合.
        /// </summary>
        [Reactive]
        public ObservableCollection<ViewPage> PartCollection { get; set; }

        /// <summary>
        /// 是否正在加载.
        /// </summary>
        [Reactive]
        public bool IsLoading { get; set; }

        /// <summary>
        /// 是否出错.
        /// </summary>
        [Reactive]
        public bool IsError { get; set; }

        /// <summary>
        /// 错误文本.
        /// </summary>
        [Reactive]
        public string ErrorText { get; set; }

        private BiliController Controller { get; } = BiliController.Instance;
    }
}

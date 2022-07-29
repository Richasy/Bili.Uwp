// Copyright (c) Richasy. All rights reserved.

using System;
using Bili.Models.App.Args;
using Bili.ViewModels.Interfaces.Article;
using Bili.ViewModels.Interfaces.Pgc;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// 负责跨组件呼叫以显示一些提示或UI组件的视图模型.
    /// </summary>
    public sealed partial class CallerViewModel
    {
        /// <summary>
        /// 请求显示提醒.
        /// </summary>
        public event EventHandler<AppTipNotificationEventArgs> RequestShowTip;

        /// <summary>
        /// 请求显示升级提示.
        /// </summary>
        public event EventHandler<UpdateEventArgs> RequestShowUpdateDialog;

        /// <summary>
        /// 请求进行之前的播放.
        /// </summary>
        public event EventHandler RequestContinuePlay;

        /// <summary>
        /// 请求显示图片列表.
        /// </summary>
        public event EventHandler<ShowImageEventArgs> RequestShowImages;

        /// <summary>
        /// 请求显示 PGC 的播放列表.
        /// </summary>
        public event EventHandler<IPgcPlaylistViewModel> RequestShowPgcPlaylist;

        /// <summary>
        /// 请求显示文章阅读器.
        /// </summary>
        public event EventHandler<IArticleItemViewModel> RequestShowArticleReader;

        /// <summary>
        /// 请求显示评论回复详情.
        /// </summary>
        public event EventHandler<ShowCommentEventArgs> RequestShowReplyDetail;

        /// <summary>
        /// 请求显示正在播放的剧集信息详情.
        /// </summary>
        public event EventHandler RequestShowPgcSeasonDetail;
    }
}

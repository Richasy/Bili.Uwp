// Copyright (c) Richasy. All rights reserved.

using System;
using System.Collections.Generic;
using Bili.Models.App.Args;
using Bili.Models.Data.Appearance;
using Bili.Models.Enums.App;
using Bili.ViewModels.Interfaces.Article;
using Bili.ViewModels.Interfaces.Pgc;

namespace Bili.ViewModels.Interfaces.Core
{
    /// <summary>
    /// 负责跨组件呼叫以显示一些提示或UI组件的视图模型的接口定义.
    /// </summary>
    public interface ICallerViewModel
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

        /// <summary>
        /// 显示提示.
        /// </summary>
        /// <param name="message">消息内容.</param>
        /// <param name="type">消息类型.</param>
        void ShowTip(string message, InfoType type = InfoType.Information);

        /// <summary>
        /// 显示升级提示.
        /// </summary>
        /// <param name="args">升级事件参数.</param>
        void ShowUpdateDialog(UpdateEventArgs args);

        /// <summary>
        /// 显示继续上一次播放的对话框.
        /// </summary>
        void ShowContinuePlayDialog();

        /// <summary>
        /// 显示图片.
        /// </summary>
        /// <param name="images">图片列表.</param>
        /// <param name="firstIndex">初始索引.</param>
        void ShowImages(IEnumerable<Image> images, int firstIndex);

        /// <summary>
        /// 显示 PGC 播放列表.
        /// </summary>
        /// <param name="vm">播放列表视图模型.</param>
        void ShowPgcPlaylist(IPgcPlaylistViewModel vm);

        /// <summary>
        /// 显示文章阅读器.
        /// </summary>
        /// <param name="article">文章信息.</param>
        void ShowArticleReader(IArticleItemViewModel article);

        /// <summary>
        /// 显示评论详情.
        /// </summary>
        /// <param name="args">评论信息.</param>
        void ShowReply(ShowCommentEventArgs args);

        /// <summary>
        /// 显示正在播放的剧集信息详情.
        /// </summary>
        void ShowPgcSeasonDetail();
    }
}

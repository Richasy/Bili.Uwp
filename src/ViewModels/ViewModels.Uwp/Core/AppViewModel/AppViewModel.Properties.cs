// Copyright (c) Richasy. All rights reserved.

using System;
using System.Reactive;
using Bili.Lib.Interfaces;
using Bili.Models.App.Args;
using Bili.Models.Data.Local;
using Bili.Models.Data.Video;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Article;
using Bili.ViewModels.Uwp.Pgc;
using Microsoft.Toolkit.Uwp.Connectivity;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Windows.UI.Xaml;

namespace Bili.ViewModels.Uwp.Core
{
    /// <summary>
    /// <see cref="AppViewModel"/>的属性集.
    /// </summary>
    public sealed partial class AppViewModel
    {
        private readonly IResourceToolkit _resourceToolkit;
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly IFileToolkit _fileToolkit;
        private readonly IAppToolkit _appToolkit;
        private readonly IUpdateProvider _updateProvider;
        private readonly NavigationViewModel _navigationViewModel;
        private readonly NetworkHelper _networkHelper;

        private bool? _isWide;

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
        public event EventHandler<PgcPlaylistViewModel> RequestShowPgcPlaylist;

        /// <summary>
        /// 请求显示文章阅读器.
        /// </summary>
        public event EventHandler<ArticleItemViewModel> RequestShowArticleReader;

        /// <summary>
        /// 请求显示评论回复详情.
        /// </summary>
        public event EventHandler<ShowCommentEventArgs> RequestShowReplyDetail;

        /// <summary>
        /// 请求显示用户详情.
        /// </summary>
        public event EventHandler<UserItemViewModel> RequestShowUserDetail;

        /// <summary>
        /// 请求显示视频收藏夹详情详情.
        /// </summary>
        public event EventHandler<VideoFavoriteFolder> RequestShowVideoFavoriteFolderDetail;

        /// <summary>
        /// 请求显示正在播放的剧集信息详情.
        /// </summary>
        public event EventHandler RequestShowPgcSeasonDetail;

        /// <summary>
        /// 检查应用更新命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CheckUpdateCommand { get; }

        /// <summary>
        /// 检查继续播放的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CheckContinuePlayCommand { get; }

        /// <summary>
        /// 检查新动态通知的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CheckNewDynamicRegistrationCommand { get; }

        /// <summary>
        /// 添加继续播放视图模型的命令.
        /// </summary>
        public ReactiveCommand<PlaySnapshot, Unit> AddLastPlayItemCommand { get; }

        /// <summary>
        /// 清除本地的继续播放视图模型的命令.
        /// </summary>
        public ReactiveCommand<Unit, Unit> DeleteLastPlayItemCommand { get; }

        /// <summary>
        /// 导航面板是否已展开.
        /// </summary>
        [Reactive]
        public bool IsNavigatePaneOpen { get; set; }

        /// <summary>
        /// 是否可以显示返回首页按钮.
        /// </summary>
        [Reactive]
        public bool CanShowHomeButton { get; set; }

        /// <summary>
        /// 页面标题文本.
        /// </summary>
        [Reactive]
        public string HeaderText { get; set; }

        /// <summary>
        /// 主题.
        /// </summary>
        [Reactive]
        public ElementTheme Theme { get; set; }

        /// <summary>
        /// 是否在Xbox上运行.
        /// </summary>
        [Reactive]
        public bool IsXbox { get; set; }

        /// <summary>
        /// 页面横向边距.
        /// </summary>
        [Reactive]
        public Thickness PageHorizontalPadding { get; set; }

        /// <summary>
        /// 页面顶部边距.
        /// </summary>
        [Reactive]
        public Thickness PageTopPadding { get; set; }

        /// <summary>
        /// 是否显示标题栏.
        /// </summary>
        [Reactive]
        public bool IsShowTitleBar { get; set; }

        /// <summary>
        /// 是否显示菜单按钮.
        /// </summary>
        [Reactive]
        public bool IsShowMenuButton { get; set; }

        /// <summary>
        /// 网络是否可用.
        /// </summary>
        [Reactive]
        public bool IsNetworkAvaliable { get; set; }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Threading;
using System.Threading.Tasks;
using Bili.Adapter;
using Bili.Adapter.Interfaces;
using Bili.Controller.Uwp.Interfaces;
using Bili.Controller.Uwp.Modules;
using Bili.Lib.Interfaces;
using Bili.Lib.Uwp;
using Bili.Locator.Uwp;
using Bili.Models.App.Args;
using Bili.Models.Data.User;
using Bili.SignIn.Uwp;
using Bili.Toolkit.Interfaces;
using Bili.Toolkit.Uwp;
using Microsoft.Extensions.DependencyInjection;
using Websocket.Client;

namespace Bili.Controller.Uwp
{
    /// <summary>
    /// 应用控制器，连接Lib层与ViewModel层的中间计算层.
    /// </summary>
    public partial class BiliController
    {
        private readonly IFileToolkit _fileToolkit;
        private readonly ISettingsToolkit _settingToolkit;

        private readonly IAuthorizeProvider _authorizeProvider;
        private readonly IAccountProvider _accountProvider;
        private readonly IHomeProvider _partitionProvider;
        private readonly ILiveProvider _liveProvider;
        private readonly IArticleProvider _specialColumnProvider;
        private readonly IPgcProvider _pgcProvider;
        private readonly IPlayerProvider _playerProvider;
        private readonly ISearchProvider _searchProvider;
        private readonly ICommunityProvider _communityProvider;
        private readonly IUpdateProvider _updateProvider;

        private readonly INetworkModule _networkModule;
        private readonly ILoggerModule _loggerModule;

        /// <summary>
        /// 直播间套接字.
        /// </summary>
        private WebsocketClient _liveWebSocket;
        private CancellationTokenSource _liveCancellationToken;
        private bool _isLiveSocketConnected;
        private Task _liveConnectionTask;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiliController"/> class.
        /// </summary>
        internal BiliController()
        {
            RegisterToolkitServices();
            ServiceLocator.Instance.LoadService(out _fileToolkit)
                .LoadService(out _settingToolkit)
                .LoadService(out _networkModule)
                .LoadService(out _loggerModule)
                .LoadService(out _authorizeProvider)
                .LoadService(out _accountProvider)
                .LoadService(out _partitionProvider)
                .LoadService(out _liveProvider)
                .LoadService(out _specialColumnProvider)
                .LoadService(out _pgcProvider)
                .LoadService(out _playerProvider)
                .LoadService(out _searchProvider)
                .LoadService(out _communityProvider)
                .LoadService(out _updateProvider);

            InitializeLiveSocket();
            RegisterEvents();
            _loggerModule.LogInformation("控制器加载完成");
        }

        /// <summary>
        /// 在用户成功登录后发生.
        /// </summary>
        public event EventHandler Logged;

        /// <summary>
        /// 在用户登出时发生.
        /// </summary>
        public event EventHandler LoggedOut;

        /// <summary>
        /// 在用户登录失败时发生.
        /// </summary>
        public event EventHandler<Exception> LoggedFailed;

        /// <summary>
        /// 在登录账户数据发生改变时发生.
        /// </summary>
        public event EventHandler<AccountInformation> AccountChanged;

        /// <summary>
        /// 在网络状态改变时发生，将返回网络可用性.
        /// </summary>
        public event EventHandler<bool> NetworkChanged;

        /// <summary>
        /// 在有分片弹幕更新时发生.
        /// </summary>
        public event EventHandler<SegmentDanmakuIterationEventArgs> SegmentDanmakuIteration;

        /// <summary>
        /// 在搜索元数据更改时发生.
        /// </summary>
        public event EventHandler<SearchMetaEventArgs> SearchMetaChanged;

        /// <summary>
        /// 在视频搜索结果更新时发生.
        /// </summary>
        public event EventHandler<VideoSearchIterationEventArgs> VideoSearchIteration;

        /// <summary>
        /// 在番剧搜索结果更新时发生.
        /// </summary>
        public event EventHandler<PgcSearchIterationEventArgs> BangumiSearchIteration;

        /// <summary>
        /// 在电影/电视剧搜索结果更新时发生.
        /// </summary>
        public event EventHandler<PgcSearchIterationEventArgs> MovieSearchIteration;

        /// <summary>
        /// 在文章搜索结果更新时发生.
        /// </summary>
        public event EventHandler<ArticleSearchIterationEventArgs> ArticleSearchIteration;

        /// <summary>
        /// 在用户搜索结果更新时发生.
        /// </summary>
        public event EventHandler<UserSearchIterationEventArgs> UserSearchIteration;

        /// <summary>
        /// 在直播搜索结果更新时发生.
        /// </summary>
        public event EventHandler<LiveSearchIterationEventArgs> LiveSearchIteration;

        /// <summary>
        /// 用户空间的视频列表更新时发生.
        /// </summary>
        public event EventHandler<UserSpaceVideoIterationEventArgs> UserSpaceVideoIteration;

        /// <summary>
        /// 收到直播消息时发生.
        /// </summary>
        public event EventHandler<LiveMessageEventArgs> LiveMessageReceived;

        /// <summary>
        /// 在PGC收藏夹内容更新时发生.
        /// </summary>
        public event EventHandler<FavoritePgcIterationEventArgs> PgcFavoriteIteration;

        /// <summary>
        /// 在文章收藏夹内容更新时发生.
        /// </summary>
        public event EventHandler<FavoriteArticleIterationEventArgs> ArticleFavoriteIteration;

        /// <summary>
        /// 在评论回复更新时发生.
        /// </summary>
        public event EventHandler<ReplyIterationEventArgs> ReplyIteration;

        /// <summary>
        /// 在评论详情更新时发生.
        /// </summary>
        public event EventHandler<ReplyIterationEventArgs> ReplyDetailIteration;

        /// <summary>
        /// 在视频动态列表更新时发生.
        /// </summary>
        public event EventHandler<DynamicVideoIterationEventArgs> DynamicVideoIteration;

        /// <summary>
        /// 收到更新消息时发生.
        /// </summary>
        public event EventHandler<UpdateEventArgs> UpdateReceived;

        /// <summary>
        /// 在用户空间搜索视频更新时发生.
        /// </summary>
        public event EventHandler<UserSpaceSearchVideoIterationEventArgs> UserSpaceSearchVideoIteration;

        /// <summary>
        /// 控制器实例.
        /// </summary>
        public static BiliController Instance { get; } = new Lazy<BiliController>(() => new BiliController()).Value;

        /// <summary>
        /// 当前网络是否正常.
        /// </summary>
        public bool IsNetworkAvailable => _networkModule.IsNetworkAvaliable;

        private void RegisterEvents()
        {
            this._authorizeProvider.StateChanged += OnAuthenticationStateChanged;
            this._networkModule.NetworkChanged += OnNetworkChangedAsync;
        }

        private void OnNetworkChangedAsync(object sender, EventArgs e) => NetworkChanged?.Invoke(this, IsNetworkAvailable);

        private void RegisterToolkitServices()
        {
            var serviceCollection = new ServiceCollection()
                .AddSingleton<INumberToolkit, NumberToolkit>()
                .AddSingleton<IAppToolkit, AppToolkit>()
                .AddSingleton<IFileToolkit, FileToolkit>()
                .AddSingleton<IResourceToolkit, ResourceToolkit>()
                .AddSingleton<ISettingsToolkit, SettingsToolkit>()
                .AddSingleton<IMD5Toolkit, MD5Toolkit>()
                .AddSingleton<IFontToolkit, FontToolkit>();

            serviceCollection
                .AddSingleton<IImageAdapter, ImageAdapter>()
                .AddSingleton<IUserAdapter, UserAdapter>()
                .AddSingleton<ICommunityAdapter, CommunityAdapter>()
                .AddSingleton<IVideoAdapter, VideoAdapter>()
                .AddSingleton<IPgcAdapter, PgcAdapter>()
                .AddSingleton<IArticleAdapter, ArticleAdapter>()
                .AddSingleton<ILiveAdapter, LiveAdapter>();

            serviceCollection
                .AddSingleton<INetworkModule, NetworkModule>()
                .AddSingleton<ILoggerModule, LoggerModule>();

            serviceCollection.AddSingleton<IAuthorizeProvider, AuthorizeProvider>()
                .AddSingleton<IHttpProvider, HttpProvider>()
                .AddSingleton<IAccountProvider, AccountProvider>()
                .AddSingleton<IHomeProvider, HomeProvider>()
                .AddSingleton<ILiveProvider, LiveProvider>()
                .AddSingleton<IArticleProvider, ArticleProvider>()
                .AddSingleton<IPgcProvider, PgcProvider>()
                .AddSingleton<IPlayerProvider, PlayerProvider>()
                .AddSingleton<ISearchProvider, SearchProvider>()
                .AddSingleton<ICommunityProvider, CommunityProvider>()
                .AddSingleton<IUpdateProvider, UpdateProvider>();

            _ = new ServiceLocator(serviceCollection);
        }

        private void ThrowWhenNetworkUnavaliable()
        {
            if (!IsNetworkAvailable)
            {
                var ex = new InvalidOperationException("网络连接异常");
                _loggerModule.LogError(ex, true);
                throw ex;
            }
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Richasy.Bili.Controller.Uwp.Interfaces;
using Richasy.Bili.Controller.Uwp.Modules;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Lib.Uwp;
using Richasy.Bili.Locator.Uwp;
using Richasy.Bili.Models.App.Args;
using Richasy.Bili.Models.BiliBili;
using Richasy.Bili.Toolkit.Interfaces;
using Richasy.Bili.Toolkit.Uwp;
using Websocket.Client;
using Windows.Networking.Sockets;

namespace Richasy.Bili.Controller.Uwp
{
    /// <summary>
    /// 应用控制器，连接Lib层与ViewModel层的中间计算层.
    /// </summary>
    public partial class BiliController
    {
        private readonly ISettingsToolkit _settingsToolkit;
        private readonly IFileToolkit _fileToolkit;

        private readonly IAuthorizeProvider _authorizeProvider;
        private readonly IAccountProvider _accountProvider;
        private readonly IPartitionProvider _partitionProvider;
        private readonly IRankProvider _rankProvider;
        private readonly IRecommendProvider _homeProvider;
        private readonly IPopularProvider _popularProvider;
        private readonly ILiveProvider _liveProvider;
        private readonly ISpecialColumnProvider _specialColumnProvider;
        private readonly IPgcProvider _pgcProvider;
        private readonly IPlayerProvider _playerProvider;
        private readonly ISearchProvider _searchProvider;

        private readonly INetworkModule _networkModule;

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
            ServiceLocator.Instance.LoadService(out _settingsToolkit)
                .LoadService(out _fileToolkit)
                .LoadService(out _networkModule)
                .LoadService(out _authorizeProvider)
                .LoadService(out _accountProvider)
                .LoadService(out _partitionProvider)
                .LoadService(out _rankProvider)
                .LoadService(out _homeProvider)
                .LoadService(out _popularProvider)
                .LoadService(out _liveProvider)
                .LoadService(out _specialColumnProvider)
                .LoadService(out _pgcProvider)
                .LoadService(out _playerProvider)
                .LoadService(out _searchProvider);

            InitializeLiveSocket();
            RegisterEvents();
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
        public event EventHandler<MyInfo> AccountChanged;

        /// <summary>
        /// 在网络状态改变时发生，将返回网络可用性.
        /// </summary>
        public event EventHandler<bool> NetworkChanged;

        /// <summary>
        /// 在子分区有新的视频列表传入时发生.
        /// </summary>
        public event EventHandler<PartitionVideoIterationEventArgs> SubPartitionVideoIteration;

        /// <summary>
        /// 在子分区的附加数据发生改变时发生.
        /// </summary>
        public event EventHandler<PartitionAdditionalDataChangedEventArgs> SubPartitionAdditionalDataChanged;

        /// <summary>
        /// 在首页推荐中有新的视频列表传入时发生.
        /// </summary>
        public event EventHandler<RecommendVideoIterationEventArgs> RecommendVideoIteration;

        /// <summary>
        /// 在热门有新的视频列表传入时发生.
        /// </summary>
        public event EventHandler<PopularVideoIterationEventArgs> PopularVideoIteration;

        /// <summary>
        /// 在直播源的附加数据发生改变时发生.
        /// </summary>
        public event EventHandler<LiveFeedAdditionalDataChangedEventArgs> LiveFeedAdditionalDataChanged;

        /// <summary>
        /// 在直播源有新的直播源数据更改时发生.
        /// </summary>
        public event EventHandler<LiveFeedRoomIterationEventArgs> LiveFeedRoomIteration;

        /// <summary>
        /// 在专栏文章的附加数据发生改变时发生.
        /// </summary>
        public event EventHandler<SpecialColumnAdditionalDataChangedEventArgs> SpecialColumnAdditionalDataChanged;

        /// <summary>
        /// 在专栏有新的文章数据更改时发生.
        /// </summary>
        public event EventHandler<SpecialColumnArticleIterationEventArgs> SpecialColumnArticleIteration;

        /// <summary>
        /// 在PGC的附加数据发生改变时发生.
        /// </summary>
        public event EventHandler<PgcModuleAdditionalDataChangedEventArgs> PgcModuleAdditionalDataChanged;

        /// <summary>
        /// 在PGC有新的模块数据更改时发生.
        /// </summary>
        public event EventHandler<PgcModuleIterationEventArgs> PgcModuleIteration;

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
                .AddSingleton<IAppToolkit, AppToolkit>()
                .AddSingleton<IFileToolkit, FileToolkit>()
                .AddSingleton<IResourceToolkit, ResourceToolkit>()
                .AddSingleton<INumberToolkit, NumberToolkit>()
                .AddSingleton<ISettingsToolkit, SettingsToolkit>()
                .AddSingleton<IMD5Toolkit, MD5Toolkit>()
                .AddSingleton<IFontToolkit, FontToolkit>()
                .AddSingleton<INetworkModule, NetworkModule>()
                .AddSingleton<IAuthorizeProvider, AuthorizeProvider>()
                .AddSingleton<IHttpProvider, HttpProvider>()
                .AddSingleton<IAccountProvider, AccountProvider>()
                .AddSingleton<IPartitionProvider, PartitionProvider>()
                .AddSingleton<IRankProvider, RankProvider>()
                .AddSingleton<IRecommendProvider, RecommendProvider>()
                .AddSingleton<IPopularProvider, PopularProvider>()
                .AddSingleton<ILiveProvider, LiveProvider>()
                .AddSingleton<ISpecialColumnProvider, SpecialColumnProvider>()
                .AddSingleton<IPgcProvider, PgcProvider>()
                .AddSingleton<IPlayerProvider, PlayerProvider>()
                .AddSingleton<ISearchProvider, SearchProvider>();
            _ = new ServiceLocator(serviceCollection);
        }

        private void ThrowWhenNetworkUnavaliable()
        {
            if (!IsNetworkAvailable)
            {
                throw new InvalidOperationException("网络连接异常");
            }
        }
    }
}

// Copyright (c) Richasy. All rights reserved.

using System;
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

        private readonly INetworkModule _networkModule;

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
                .LoadService(out _liveProvider);

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

            // this._networkModule.NetworkChanged += OnNetworkChangedAsync;
        }

        private void RegisterToolkitServices()
        {
            var serviceCollection = new ServiceCollection()
                .AddSingleton<IAppToolkit, AppToolkit>()
                .AddSingleton<IFileToolkit, FileToolkit>()
                .AddSingleton<IResourceToolkit, ResourceToolkit>()
                .AddSingleton<INumberToolkit, NumberToolkit>()
                .AddSingleton<ISettingsToolkit, SettingsToolkit>()
                .AddSingleton<IMD5Toolkit, MD5Toolkit>()
                .AddSingleton<INetworkModule, NetworkModule>()
                .AddSingleton<IAuthorizeProvider, AuthorizeProvider>()
                .AddSingleton<IHttpProvider, HttpProvider>()
                .AddSingleton<IAccountProvider, AccountProvider>()
                .AddSingleton<IPartitionProvider, PartitionProvider>()
                .AddSingleton<IRankProvider, RankProvider>()
                .AddSingleton<IRecommendProvider, RecommendProvider>()
                .AddSingleton<IPopularProvider, PopularProvider>()
                .AddSingleton<ILiveProvider, LiveProvider>();
            _ = new ServiceLocator(serviceCollection);
        }
    }
}

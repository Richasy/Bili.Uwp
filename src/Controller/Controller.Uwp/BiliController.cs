// Copyright (c) Richasy. All rights reserved.

using System;
using Microsoft.Extensions.DependencyInjection;
using Richasy.Bili.Controller.Uwp.Interfaces;
using Richasy.Bili.Controller.Uwp.Modules;
using Richasy.Bili.Lib.Interfaces;
using Richasy.Bili.Lib.Uwp;
using Richasy.Bili.Locator.Uwp;
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
        private readonly IAuthorizeProvider _authorizeProvider;
        private readonly IAccountProvider _accountProvider;

        private readonly INetworkModule _networkModule;

        /// <summary>
        /// Initializes a new instance of the <see cref="BiliController"/> class.
        /// </summary>
        internal BiliController()
        {
            RegisterToolkitServices();
            ServiceLocator.Instance.LoadService(out _settingsToolkit)
                .LoadService(out _networkModule)
                .LoadService(out _authorizeProvider)
                .LoadService(out _accountProvider);

            RegisterEvents();
        }

        /// <summary>
        /// Triggered when the user successfully logs in
        /// </summary>
        public event EventHandler Logged;

        /// <summary>
        /// Triggered when the user successfully logs out
        /// </summary>
        public event EventHandler LoggedOut;

        /// <summary>
        /// Triggered when user login fails
        /// </summary>
        public event EventHandler<Exception> LoggedFailed;

        /// <summary>
        /// Triggered when the user changes
        /// </summary>
        public event EventHandler<MyInfo> AccountChanged;

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
                .AddSingleton<IAccountProvider, AccountProvider>();
            _ = new ServiceLocator(serviceCollection);
        }
    }
}

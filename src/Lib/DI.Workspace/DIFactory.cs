// Copyright (c) Richasy. All rights reserved.

using Bili.Adapter;
using Bili.Adapter.Interfaces;
using Bili.DI.Container;
using Bili.Lib;
using Bili.Lib.Interfaces;
using Bili.Models.App.Constants;
using Bili.SignIn.Workspace;
using Bili.Toolkit.Interfaces;
using Bili.Toolkit.Workspace;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Community;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Home;
using Bili.ViewModels.Interfaces.Pgc;
using Bili.ViewModels.Interfaces.Search;
using Bili.ViewModels.Interfaces.Video;
using Bili.ViewModels.Interfaces.Workspace;
using Bili.ViewModels.Workspace;
using Bili.ViewModels.Workspace.Account;
using Bili.ViewModels.Workspace.Community;
using Bili.ViewModels.Workspace.Core;
using Bili.ViewModels.Workspace.Pages;
using Windows.Storage;

namespace DI.Workspace
{
    /// <summary>
    /// 依赖注入工厂.
    /// </summary>
    public static class DIFactory
    {
        /// <summary>
        /// 注册应用所需的依赖服务.
        /// </summary>
        public static void RegisterAppRequiredServices()
        {
            var rootFolder = ApplicationData.Current.LocalFolder;
            var logFolderName = AppConstants.Location.LoggerFolder;
            var fullPath = $"{rootFolder.Path}\\{logFolderName}\\";
            NLog.GlobalDiagnosticsContext.Set("LogPath", fullPath);
            Locator.Instance
                .RegisterSingleton<IResourceToolkit, ResourceToolkit>()
                .RegisterSingleton<INumberToolkit, NumberToolkit>()
                .RegisterSingleton<ISettingsToolkit, SettingsToolkit>()
                .RegisterSingleton<IAppToolkit, AppToolkit>()
                .RegisterSingleton<IMD5Toolkit, MD5Toolkit>()
                .RegisterSingleton<IVideoToolkit, VideoToolkit>()
                .RegisterSingleton<IFileToolkit, FileToolkit>()
                .RegisterSingleton<ITextToolkit, TextToolkit>()

                .RegisterSingleton<IImageAdapter, ImageAdapter>()
                .RegisterSingleton<IUserAdapter, UserAdapter>()
                .RegisterSingleton<ICommunityAdapter, CommunityAdapter>()
                .RegisterSingleton<IVideoAdapter, VideoAdapter>()
                .RegisterSingleton<IPgcAdapter, PgcAdapter>()
                .RegisterSingleton<ILiveAdapter, LiveAdapter>()
                .RegisterSingleton<IArticleAdapter, ArticleAdapter>()
                .RegisterSingleton<ISearchAdapter, SearchAdapter>()
                .RegisterSingleton<IFavoriteAdapter, FavoriteAdapter>()
                .RegisterSingleton<IDynamicAdapter, DynamicAdapter>()
                .RegisterSingleton<ICommentAdapter, CommentAdapter>()
                .RegisterSingleton<IPlayerAdapter, PlayerAdapter>()

                .RegisterSingleton<IAuthorizeProvider, AuthorizeProvider>()
                .RegisterSingleton<IHttpProvider, HttpProvider>()
                .RegisterSingleton<IAccountProvider, AccountProvider>()
                .RegisterSingleton<IHomeProvider, HomeProvider>()
                .RegisterSingleton<ILiveProvider, LiveProvider>()
                .RegisterSingleton<IArticleProvider, ArticleProvider>()
                .RegisterSingleton<IPgcProvider, PgcProvider>()
                .RegisterSingleton<IPlayerProvider, PlayerProvider>()
                .RegisterSingleton<ISearchProvider, SearchProvider>()
                .RegisterSingleton<ICommunityProvider, CommunityProvider>()
                .RegisterSingleton<IUpdateProvider, UpdateProvider>()
                .RegisterSingleton<IFavoriteProvider, FavoriteProvider>()

                .RegisterSingleton<IWorkspaceViewModel, WorkspaceViewModel>()
                .RegisterSingleton<ICallerViewModel, CallerViewModel>()
                .RegisterSingleton<ISettingsViewModel, SettingsViewModel>()
                .RegisterSingleton<IAccountViewModel, AccountViewModel>()
                .RegisterSingleton<ISearchBoxViewModel, SearchBoxViewModel>()
                .RegisterSingleton<IHomePageViewModel, HomePageViewModel>()
                .RegisterSingleton<IPopularPageViewModel, PopularPageViewModel>()
                .RegisterSingleton<IDynamicVideoModuleViewModel, DynamicVideoModuleViewModel>()
                .RegisterSingleton<IHistoryPageViewModel, HistoryPageViewModel>()

                .RegisterTransient<IVideoPartitionViewModel, VideoPartitionViewModel>()
                .RegisterTransient<IUserItemViewModel, UserItemViewModel>()
                .RegisterTransient<IVideoItemViewModel, VideoItemViewModel>()
                .RegisterTransient<IDynamicItemViewModel, DynamicItemViewModel>()
                .RegisterTransient<IEpisodeItemViewModel, EpisodeItemViewModel>()
                .Build();
        }
    }
}

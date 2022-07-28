// Copyright (c) Richasy. All rights reserved.
using Bili.Adapter;
using Bili.Adapter.Interfaces;
using Bili.Lib;
using Bili.Lib.Interfaces;
using Bili.Models.App.Constants;
using Bili.SignIn.Uwp;
using Bili.Toolkit.Interfaces;
using Bili.Toolkit.Uwp;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Article;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Pgc;
using Bili.ViewModels.Interfaces.Video;
using Bili.ViewModels.Uwp.Account;
using Bili.ViewModels.Uwp.Article;
using Bili.ViewModels.Uwp.Common;
using Bili.ViewModels.Uwp.Community;
using Bili.ViewModels.Uwp.Core;
using Bili.ViewModels.Uwp.Home;
using Bili.ViewModels.Uwp.Live;
using Bili.ViewModels.Uwp.Pgc;
using Bili.ViewModels.Uwp.Search;
using Bili.ViewModels.Uwp.Toolbox;
using Bili.ViewModels.Uwp.Video;
using Splat;
using Splat.NLog;
using Windows.Storage;
using Windows.System.Display;
using Windows.UI.Xaml;

namespace Bili.DI.App
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
            Locator.CurrentMutable.UseNLogWithWrappingFullLogger();
            SplatRegistrations.RegisterLazySingleton<IResourceToolkit, ResourceToolkit>();
            SplatRegistrations.RegisterLazySingleton<ISettingsToolkit, SettingsToolkit>();
            SplatRegistrations.RegisterLazySingleton<INumberToolkit, NumberToolkit>();
            SplatRegistrations.RegisterLazySingleton<IAppToolkit, AppToolkit>();
            SplatRegistrations.RegisterLazySingleton<IFileToolkit, FileToolkit>();
            SplatRegistrations.RegisterLazySingleton<IMD5Toolkit, MD5Toolkit>();
            SplatRegistrations.RegisterLazySingleton<IFontToolkit, FontToolkit>();
            SplatRegistrations.RegisterLazySingleton<IVideoToolkit, VideoToolkit>();
            SplatRegistrations.RegisterLazySingleton<ITextToolkit, TextToolkit>();

            SplatRegistrations.RegisterLazySingleton<IImageAdapter, ImageAdapter>();
            SplatRegistrations.RegisterLazySingleton<IUserAdapter, UserAdapter>();
            SplatRegistrations.RegisterLazySingleton<ICommunityAdapter, CommunityAdapter>();
            SplatRegistrations.RegisterLazySingleton<IVideoAdapter, VideoAdapter>();
            SplatRegistrations.RegisterLazySingleton<IPgcAdapter, PgcAdapter>();
            SplatRegistrations.RegisterLazySingleton<ILiveAdapter, LiveAdapter>();
            SplatRegistrations.RegisterLazySingleton<IArticleAdapter, ArticleAdapter>();
            SplatRegistrations.RegisterLazySingleton<ISearchAdapter, SearchAdapter>();
            SplatRegistrations.RegisterLazySingleton<IFavoriteAdapter, FavoriteAdapter>();
            SplatRegistrations.RegisterLazySingleton<IDynamicAdapter, DynamicAdapter>();
            SplatRegistrations.RegisterLazySingleton<ICommentAdapter, CommentAdapter>();
            SplatRegistrations.RegisterLazySingleton<IPlayerAdapter, PlayerAdapter>();

            SplatRegistrations.RegisterLazySingleton<IAuthorizeProvider, AuthorizeProvider>();
            SplatRegistrations.RegisterLazySingleton<IHttpProvider, HttpProvider>();
            SplatRegistrations.RegisterLazySingleton<IAccountProvider, AccountProvider>();
            SplatRegistrations.RegisterLazySingleton<IHomeProvider, HomeProvider>();
            SplatRegistrations.RegisterLazySingleton<ILiveProvider, LiveProvider>();
            SplatRegistrations.RegisterLazySingleton<IArticleProvider, ArticleProvider>();
            SplatRegistrations.RegisterLazySingleton<IPgcProvider, PgcProvider>();
            SplatRegistrations.RegisterLazySingleton<IPlayerProvider, PlayerProvider>();
            SplatRegistrations.RegisterLazySingleton<ISearchProvider, SearchProvider>();
            SplatRegistrations.RegisterLazySingleton<ICommunityProvider, CommunityProvider>();
            SplatRegistrations.RegisterLazySingleton<IUpdateProvider, UpdateProvider>();
            SplatRegistrations.RegisterLazySingleton<IFavoriteProvider, FavoriteProvider>();

            SplatRegistrations.Register<IUserItemViewModel, UserItemViewModel>();
            SplatRegistrations.Register<IVideoItemViewModel, VideoItemViewModel>();
            SplatRegistrations.Register<IEpisodeItemViewModel, EpisodeItemViewModel>();
            SplatRegistrations.Register<IArticleItemViewModel, ArticleItemViewModel>();
            SplatRegistrations.Register<ISeasonItemViewModel, SeasonItemViewModel>();
            SplatRegistrations.Register<IPgcPlaylistViewModel, PgcPlaylistViewModel>();

            SplatRegistrations.RegisterLazySingleton<ICallerViewModel, CallerViewModel>();

            SplatRegistrations.RegisterLazySingleton<AppViewModel>();
            SplatRegistrations.RegisterLazySingleton<NavigationViewModel>();
            SplatRegistrations.RegisterLazySingleton<AccountViewModel>();
            SplatRegistrations.RegisterLazySingleton<RecommendPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<PopularPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<VideoPartitionPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<VideoPartitionDetailPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<RankPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<LiveFeedPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<LivePartitionPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<LivePartitionDetailPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<BangumiPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<DomesticPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<DocumentaryPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<MoviePageViewModel>();
            SplatRegistrations.RegisterLazySingleton<TvPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IndexPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<TimelinePageViewModel>();
            SplatRegistrations.RegisterLazySingleton<ToolboxPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<HelpPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<ArticlePartitionPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<MessagePageViewModel>();
            SplatRegistrations.RegisterLazySingleton<ViewLaterPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<HistoryPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<FansPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<FollowsPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<UserSpaceViewModel>();
            SplatRegistrations.RegisterLazySingleton<SearchBoxViewModel>();
            SplatRegistrations.RegisterLazySingleton<SearchPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<SettingsPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<MyFollowsPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<VideoFavoriteFolderDetailViewModel>();
            SplatRegistrations.RegisterLazySingleton<VideoFavoriteModuleViewModel>();
            SplatRegistrations.RegisterLazySingleton<AnimeFavoriteModuleViewModel>();
            SplatRegistrations.RegisterLazySingleton<CinemaFavoriteModuleViewModel>();
            SplatRegistrations.RegisterLazySingleton<ArticleFavoriteModuleViewModel>();
            SplatRegistrations.RegisterLazySingleton<FavoritePageViewModel>();
            SplatRegistrations.RegisterLazySingleton<DynamicAllModuleViewModel>();
            SplatRegistrations.RegisterLazySingleton<DynamicVideoModuleViewModel>();
            SplatRegistrations.RegisterLazySingleton<DynamicPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<CommentMainModuleViewModel>();
            SplatRegistrations.RegisterLazySingleton<CommentDetailModuleViewModel>();
            SplatRegistrations.RegisterLazySingleton<CommentPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<AvBvConverterViewModel>();
            SplatRegistrations.RegisterLazySingleton<CoverDownloaderViewModel>();
            SplatRegistrations.RegisterLazySingleton<VideoPlayerPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<PgcPlayerPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<LivePlayerPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<XboxAccountPageViewModel>();

            SplatRegistrations.Register<INativePlayerViewModel, NativePlayerViewModel>();
            SplatRegistrations.Register<IFFmpegPlayerViewModel, FFmpegPlayerViewModel>();
            SplatRegistrations.Register<MediaPlayerViewModel>();
            SplatRegistrations.Register<LiveItemViewModel>();
            SplatRegistrations.Register<MessageItemViewModel>();
            SplatRegistrations.Register<VideoFavoriteFolderViewModel>();
            SplatRegistrations.Register<VideoFavoriteFolderGroupViewModel>();
            SplatRegistrations.Register<DynamicItemViewModel>();
            SplatRegistrations.Register<CommentItemViewModel>();
            SplatRegistrations.Register<ToolboxItemViewModel>();
            SplatRegistrations.Register<SubtitleModuleViewModel>();
            SplatRegistrations.Register<DanmakuModuleViewModel>();
            SplatRegistrations.Register<InteractionModuleViewModel>();
            SplatRegistrations.Register<DownloadModuleViewModel>();

            SplatRegistrations.SetupIOC();
        }

        /// <summary>
        /// 注册应用所需的常量.
        /// </summary>
        public static void RegisterAppRequiredConstants()
        {
            SplatRegistrations.RegisterConstant(Window.Current.CoreWindow.Dispatcher);
            SplatRegistrations.RegisterConstant(new DisplayRequest());
            SplatRegistrations.SetupIOC();
        }
    }
}

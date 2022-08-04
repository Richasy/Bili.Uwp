// Copyright (c) Richasy. All rights reserved.

using Bili.Adapter;
using Bili.Adapter.Interfaces;
using Bili.Lib;
using Bili.Lib.Interfaces;
using Bili.Models.App.Constants;
using Bili.SignIn.Uwp;
using Bili.Toolkit.Interfaces;
using Bili.Toolkit.Uwp;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Article;
using Bili.ViewModels.Interfaces.Common;
using Bili.ViewModels.Interfaces.Community;
using Bili.ViewModels.Interfaces.Core;
using Bili.ViewModels.Interfaces.Home;
using Bili.ViewModels.Interfaces.Live;
using Bili.ViewModels.Interfaces.Pgc;
using Bili.ViewModels.Interfaces.Search;
using Bili.ViewModels.Interfaces.Toolbox;
using Bili.ViewModels.Interfaces.Video;
using Bili.ViewModels.Uwp;
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
            SplatRegistrations.Register<ILiveItemViewModel, LiveItemViewModel>();
            SplatRegistrations.Register<IPgcPlaylistViewModel, PgcPlaylistViewModel>();
            SplatRegistrations.Register<IToolboxItemViewModel, ToolboxItemViewModel>();
            SplatRegistrations.Register<IBannerViewModel, BannerViewModel>();
            SplatRegistrations.Register<IMessageItemViewModel, MessageItemViewModel>();
            SplatRegistrations.Register<IMessageHeaderViewModel, MessageHeaderViewModel>();
            SplatRegistrations.Register<IDynamicItemViewModel, DynamicItemViewModel>();
            SplatRegistrations.Register<ICommentItemViewModel, CommentItemViewModel>();
            SplatRegistrations.Register<IFavoriteMetaViewModel, FavoriteMetaViewModel>();
            SplatRegistrations.Register<INumberPartViewModel, NumberPartViewModel>();
            SplatRegistrations.Register<IPlaybackRateItemViewModel, PlaybackRateItemViewModel>();
            SplatRegistrations.Register<IVideoIdentifierSelectableViewModel, VideoIdentifierSelectableViewModel>();
            SplatRegistrations.Register<IVideoFavoriteFolderSelectableViewModel, VideoFavoriteFolderSelectableViewModel>();
            SplatRegistrations.Register<IDownloadModuleViewModel, DownloadModuleViewModel>();
            SplatRegistrations.Register<ISubtitleModuleViewModel, SubtitleModuleViewModel>();
            SplatRegistrations.Register<IDanmakuModuleViewModel, DanmakuModuleViewModel>();
            SplatRegistrations.Register<IInteractionModuleViewModel, InteractionModuleViewModel>();
            SplatRegistrations.Register<IVideoFavoriteFolderViewModel, VideoFavoriteFolderViewModel>();
            SplatRegistrations.Register<IVideoFavoriteFolderGroupViewModel, VideoFavoriteFolderGroupViewModel>();
            SplatRegistrations.Register<ISearchFilterViewModel, SearchFilterViewModel>();
            SplatRegistrations.Register<ISearchModuleItemViewModel, SearchModuleItemViewModel>();

            SplatRegistrations.Register<IIndexFilterViewModel, IndexFilterViewModel>();
            SplatRegistrations.Register<IPgcRankViewModel, PgcRankViewModel>();
            SplatRegistrations.Register<IPgcExtraItemViewModel, PgcExtraItemViewModel>();
            SplatRegistrations.Register<INativePlayerViewModel, NativePlayerViewModel>();
            SplatRegistrations.Register<IFFmpegPlayerViewModel, FFmpegPlayerViewModel>();
            SplatRegistrations.Register<IMediaPlayerViewModel, MediaPlayerViewModel>();

            SplatRegistrations.RegisterLazySingleton<ICallerViewModel, CallerViewModel>();
            SplatRegistrations.RegisterLazySingleton<IRecordViewModel, RecordViewModel>();
            SplatRegistrations.RegisterLazySingleton<IAppViewModel, AppViewModel>();
            SplatRegistrations.RegisterLazySingleton<INavigationViewModel, NavigationViewModel>();
            SplatRegistrations.RegisterLazySingleton<IAccountViewModel, AccountViewModel>();
            SplatRegistrations.RegisterLazySingleton<IVideoFavoriteModuleViewModel, VideoFavoriteModuleViewModel>();
            SplatRegistrations.RegisterLazySingleton<IVideoFavoriteFolderDetailViewModel, VideoFavoriteFolderDetailViewModel>();
            SplatRegistrations.RegisterLazySingleton<IAnimeFavoriteModuleViewModel, AnimeFavoriteModuleViewModel>();
            SplatRegistrations.RegisterLazySingleton<ICinemaFavoriteModuleViewModel, CinemaFavoriteModuleViewModel>();
            SplatRegistrations.RegisterLazySingleton<IArticleFavoriteModuleViewModel, ArticleFavoriteModuleViewModel>();
            SplatRegistrations.RegisterLazySingleton<IUserSpaceViewModel, UserSpaceViewModel>();
            SplatRegistrations.RegisterLazySingleton<IAvBvConverterViewModel, AvBvConverterViewModel>();
            SplatRegistrations.RegisterLazySingleton<ICoverDownloaderViewModel, CoverDownloaderViewModel>();
            SplatRegistrations.RegisterLazySingleton<IDynamicAllModuleViewModel, DynamicAllModuleViewModel>();
            SplatRegistrations.RegisterLazySingleton<IDynamicVideoModuleViewModel, DynamicVideoModuleViewModel>();

            SplatRegistrations.RegisterLazySingleton<IFavoritePageViewModel, FavoritePageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IRecommendPageViewModel, RecommendPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IPopularPageViewModel, PopularPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IRankPageViewModel, RankPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IToolboxPageViewModel, ToolboxPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<ISettingsPageViewModel, SettingsPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IHelpPageViewModel, HelpPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IVideoPartitionPageViewModel, VideoPartitionPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IVideoPartitionDetailPageViewModel, VideoPartitionDetailPageViewModel>();

            SplatRegistrations.RegisterLazySingleton<IIndexPageViewModel, IndexPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IBangumiPageViewModel, BangumiPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IDomesticPageViewModel, DomesticPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IMoviePageViewModel, MoviePageViewModel>();
            SplatRegistrations.RegisterLazySingleton<ITvPageViewModel, TvPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IDocumentaryPageViewModel, DocumentaryPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<ITimelinePageViewModel, TimelinePageViewModel>();

            SplatRegistrations.RegisterLazySingleton<IViewLaterPageViewModel, ViewLaterPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IHistoryPageViewModel, HistoryPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IMyFollowsPageViewModel, MyFollowsPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IXboxAccountPageViewModel, XboxAccountPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IArticlePartitionPageViewModel, ArticlePartitionPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IFollowsPageViewModel, FollowsPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IFansPageViewModel, FansPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IMessagePageViewModel, MessagePageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IDynamicPageViewModel, DynamicPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<ICommentMainModuleViewModel, CommentMainModuleViewModel>();
            SplatRegistrations.RegisterLazySingleton<ICommentDetailModuleViewModel, CommentDetailModuleViewModel>();
            SplatRegistrations.RegisterLazySingleton<ICommentPageViewModel, CommentPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<ILiveFeedPageViewModel, LiveFeedPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<ILivePartitionPageViewModel, LivePartitionPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<ILivePartitionDetailPageViewModel, LivePartitionDetailPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<ISearchBoxViewModel, SearchBoxViewModel>();
            SplatRegistrations.RegisterLazySingleton<ISearchPageViewModel, SearchPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IVideoPlayerPageViewModel, VideoPlayerPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<IPgcPlayerPageViewModel, PgcPlayerPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<ILivePlayerPageViewModel, LivePlayerPageViewModel>();

            SplatRegistrations.SetupIOC();
        }

        /// <summary>
        /// 注册应用所需的常量.
        /// </summary>
        public static void RegisterAppRequiredConstants()
        {
            SplatRegistrations.RegisterConstant(Window.Current.CoreWindow.Dispatcher);
            SplatRegistrations.SetupIOC();
        }
    }
}

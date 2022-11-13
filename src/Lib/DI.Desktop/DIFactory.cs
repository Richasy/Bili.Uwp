// Copyright (c) Richasy. All rights reserved.

using Bili.Adapter;
using Bili.Adapter.Interfaces;
using Bili.DI.Container;
using Bili.Lib;
using Bili.Lib.Interfaces;
using Bili.Models.App.Constants;
using Bili.SignIn.Desktop;
using Bili.Toolkit.Desktop;
using Bili.Toolkit.Interfaces;
using Bili.ViewModels.Desktop;
using Bili.ViewModels.Desktop.Account;
using Bili.ViewModels.Desktop.Article;
using Bili.ViewModels.Desktop.Common;
using Bili.ViewModels.Desktop.Community;
using Bili.ViewModels.Desktop.Core;
using Bili.ViewModels.Desktop.Home;
using Bili.ViewModels.Desktop.Live;
using Bili.ViewModels.Desktop.Pgc;
using Bili.ViewModels.Desktop.Search;
using Bili.ViewModels.Desktop.Toolbox;
using Bili.ViewModels.Desktop.Video;
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
using Windows.Storage;

namespace DI.Desktop
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
            var logger = NLog.LogManager.GetLogger("Richasy.Bili");
            Locator.Instance
                .RegisterSingleton<NLog.ILogger>(logger)
                .RegisterSingleton<IResourceToolkit, ResourceToolkit>()
                .RegisterSingleton<INumberToolkit, NumberToolkit>()
                .RegisterSingleton<ISettingsToolkit, SettingsToolkit>()
                .RegisterSingleton<IAppToolkit, AppToolkit>()
                .RegisterSingleton<IFileToolkit, FileToolkit>()
                .RegisterSingleton<IMD5Toolkit, MD5Toolkit>()
                .RegisterSingleton<IFontToolkit, FontToolkit>()
                .RegisterSingleton<IVideoToolkit, VideoToolkit>()
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

                .RegisterTransient<IUserItemViewModel, UserItemViewModel>()
                .RegisterTransient<IVideoItemViewModel, VideoItemViewModel>()
                .RegisterTransient<IEpisodeItemViewModel, EpisodeItemViewModel>()
                .RegisterTransient<IArticleItemViewModel, ArticleItemViewModel>()
                .RegisterTransient<ISeasonItemViewModel, SeasonItemViewModel>()
                .RegisterTransient<ILiveItemViewModel, LiveItemViewModel>()
                .RegisterTransient<IPgcPlaylistViewModel, PgcPlaylistViewModel>()
                .RegisterTransient<IToolboxItemViewModel, ToolboxItemViewModel>()
                .RegisterTransient<IBannerViewModel, BannerViewModel>()
                .RegisterTransient<IMessageItemViewModel, MessageItemViewModel>()
                .RegisterTransient<IMessageHeaderViewModel, MessageHeaderViewModel>()
                .RegisterTransient<IDynamicItemViewModel, DynamicItemViewModel>()
                .RegisterTransient<ICommentItemViewModel, CommentItemViewModel>()
                .RegisterTransient<IFavoriteMetaViewModel, FavoriteMetaViewModel>()
                .RegisterTransient<INumberPartViewModel, NumberPartViewModel>()
                .RegisterTransient<IPlaybackRateItemViewModel, PlaybackRateItemViewModel>()
                .RegisterTransient<IVideoIdentifierSelectableViewModel, VideoIdentifierSelectableViewModel>()
                .RegisterTransient<IVideoFavoriteFolderSelectableViewModel, VideoFavoriteFolderSelectableViewModel>()
                .RegisterTransient<IDownloadModuleViewModel, DownloadModuleViewModel>()
                .RegisterTransient<ISubtitleModuleViewModel, SubtitleModuleViewModel>()
                .RegisterTransient<IDanmakuModuleViewModel, DanmakuModuleViewModel>()
                .RegisterTransient<IInteractionModuleViewModel, InteractionModuleViewModel>()
                .RegisterTransient<IVideoFavoriteFolderViewModel, VideoFavoriteFolderViewModel>()
                .RegisterTransient<IVideoFavoriteFolderGroupViewModel, VideoFavoriteFolderGroupViewModel>()
                .RegisterTransient<ISearchFilterViewModel, SearchFilterViewModel>()
                .RegisterTransient<ISearchModuleItemViewModel, SearchModuleItemViewModel>()

                .RegisterTransient<IIndexFilterViewModel, IndexFilterViewModel>()
                .RegisterTransient<IPgcRankViewModel, PgcRankViewModel>()
                .RegisterTransient<IPgcExtraItemViewModel, PgcExtraItemViewModel>()
                .RegisterTransient<INativePlayerViewModel, NativePlayerViewModel>()
                .RegisterTransient<IFFmpegPlayerViewModel, FFmpegPlayerViewModel>()
                .RegisterTransient<IMediaPlayerViewModel, MediaPlayerViewModel>()

                .RegisterSingleton<ICallerViewModel, CallerViewModel>()
                .RegisterSingleton<IRecordViewModel, RecordViewModel>()
                .RegisterSingleton<IAppViewModel, AppViewModel>()
                .RegisterSingleton<INavigationViewModel, NavigationViewModel>()
                .RegisterSingleton<IAccountViewModel, AccountViewModel>()
                .RegisterSingleton<IVideoFavoriteModuleViewModel, VideoFavoriteModuleViewModel>()
                .RegisterSingleton<IVideoFavoriteFolderDetailViewModel, VideoFavoriteFolderDetailViewModel>()
                .RegisterSingleton<IAnimeFavoriteModuleViewModel, AnimeFavoriteModuleViewModel>()
                .RegisterSingleton<ICinemaFavoriteModuleViewModel, CinemaFavoriteModuleViewModel>()
                .RegisterSingleton<IArticleFavoriteModuleViewModel, ArticleFavoriteModuleViewModel>()
                .RegisterSingleton<IUserSpaceViewModel, UserSpaceViewModel>()
                .RegisterSingleton<IAvBvConverterViewModel, AvBvConverterViewModel>()
                .RegisterSingleton<ICoverDownloaderViewModel, CoverDownloaderViewModel>()
                .RegisterSingleton<IDynamicAllModuleViewModel, DynamicAllModuleViewModel>()
                .RegisterSingleton<IDynamicVideoModuleViewModel, DynamicVideoModuleViewModel>()

                .RegisterSingleton<IFavoritePageViewModel, FavoritePageViewModel>()
                .RegisterSingleton<IRecommendPageViewModel, RecommendPageViewModel>()
                .RegisterSingleton<IPopularPageViewModel, PopularPageViewModel>()
                .RegisterSingleton<IRankPageViewModel, RankPageViewModel>()
                .RegisterSingleton<IToolboxPageViewModel, ToolboxPageViewModel>()
                .RegisterSingleton<ISettingsPageViewModel, SettingsPageViewModel>()
                .RegisterSingleton<IHelpPageViewModel, HelpPageViewModel>()
                .RegisterSingleton<IVideoPartitionPageViewModel, VideoPartitionPageViewModel>()
                .RegisterSingleton<IVideoPartitionDetailPageViewModel, VideoPartitionDetailPageViewModel>()

                .RegisterSingleton<IIndexPageViewModel, IndexPageViewModel>()
                .RegisterSingleton<IBangumiPageViewModel, BangumiPageViewModel>()
                .RegisterSingleton<IDomesticPageViewModel, DomesticPageViewModel>()
                .RegisterSingleton<IMoviePageViewModel, MoviePageViewModel>()
                .RegisterSingleton<ITvPageViewModel, TvPageViewModel>()
                .RegisterSingleton<IDocumentaryPageViewModel, DocumentaryPageViewModel>()
                .RegisterSingleton<ITimelinePageViewModel, TimelinePageViewModel>()

                .RegisterSingleton<IViewLaterPageViewModel, ViewLaterPageViewModel>()
                .RegisterSingleton<IHistoryPageViewModel, HistoryPageViewModel>()
                .RegisterSingleton<IMyFollowsPageViewModel, MyFollowsPageViewModel>()
                .RegisterSingleton<IXboxAccountPageViewModel, XboxAccountPageViewModel>()
                .RegisterSingleton<IArticlePartitionPageViewModel, ArticlePartitionPageViewModel>()
                .RegisterSingleton<IFollowsPageViewModel, FollowsPageViewModel>()
                .RegisterSingleton<IFansPageViewModel, FansPageViewModel>()
                .RegisterSingleton<IMessagePageViewModel, MessagePageViewModel>()
                .RegisterSingleton<IDynamicPageViewModel, DynamicPageViewModel>()
                .RegisterSingleton<ICommentMainModuleViewModel, CommentMainModuleViewModel>()
                .RegisterSingleton<ICommentDetailModuleViewModel, CommentDetailModuleViewModel>()
                .RegisterSingleton<ICommentPageViewModel, CommentPageViewModel>()
                .RegisterSingleton<ILiveFeedPageViewModel, LiveFeedPageViewModel>()
                .RegisterSingleton<ILivePartitionPageViewModel, LivePartitionPageViewModel>()
                .RegisterSingleton<ILivePartitionDetailPageViewModel, LivePartitionDetailPageViewModel>()
                .RegisterSingleton<ISearchBoxViewModel, SearchBoxViewModel>()
                .RegisterSingleton<ISearchPageViewModel, SearchPageViewModel>()
                .RegisterSingleton<IVideoPlayerPageViewModel, VideoPlayerPageViewModel>()
                .RegisterSingleton<IPgcPlayerPageViewModel, PgcPlayerPageViewModel>()
                .RegisterSingleton<ILivePlayerPageViewModel, LivePlayerPageViewModel>()
                .Build();
        }
    }
}

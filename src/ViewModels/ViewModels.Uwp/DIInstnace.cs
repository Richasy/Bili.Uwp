// Copyright (c) Richasy. All rights reserved.

using Bili.Adapter;
using Bili.Adapter.Interfaces;
using Bili.Lib.Interfaces;
using Bili.Lib.Uwp;
using Bili.Toolkit.Interfaces;
using Bili.Toolkit.Uwp;
using Bili.ViewModels.Interfaces;
using Bili.ViewModels.Uwp.Common;
using Bili.ViewModels.Uwp.Community;
using Bili.ViewModels.Uwp.Core;
using Bili.ViewModels.Uwp.Home;
using Bili.ViewModels.Uwp.Pgc;
using Bili.ViewModels.Uwp.Video;
using Splat;
using Windows.System.Display;
using Windows.UI.Xaml;

namespace Bili.ViewModels.Uwp
{
    /// <summary>
    /// 依赖注入实例.
    /// </summary>
    public static class DIInstnace
    {
        /// <summary>
        /// 注册依赖服务.
        /// </summary>
        public static void RegisterServices()
        {
            SplatRegistrations.RegisterLazySingleton<INumberToolkit, NumberToolkit>();
            SplatRegistrations.RegisterLazySingleton<IAppToolkit, AppToolkit>();
            SplatRegistrations.RegisterLazySingleton<IFileToolkit, FileToolkit>();
            SplatRegistrations.RegisterLazySingleton<IResourceToolkit, ResourceToolkit>();
            SplatRegistrations.RegisterLazySingleton<ISettingsToolkit, SettingsToolkit>();
            SplatRegistrations.RegisterLazySingleton<IMD5Toolkit, MD5Toolkit>();
            SplatRegistrations.RegisterLazySingleton<IFontToolkit, FontToolkit>();

            SplatRegistrations.RegisterLazySingleton<IImageAdapter, ImageAdapter>();
            SplatRegistrations.RegisterLazySingleton<IUserAdapter, UserAdapter>();
            SplatRegistrations.RegisterLazySingleton<ICommunityAdapter, CommunityAdapter>();
            SplatRegistrations.RegisterLazySingleton<IVideoAdapter, VideoAdapter>();
            SplatRegistrations.RegisterLazySingleton<IPgcAdapter, PgcAdapter>();
            SplatRegistrations.RegisterLazySingleton<ILiveAdapter, LiveAdapter>();

            SplatRegistrations.RegisterLazySingleton<IAuthorizeProvider, AuthorizeProvider>();
            SplatRegistrations.RegisterLazySingleton<IHttpProvider, HttpProvider>();
            SplatRegistrations.RegisterLazySingleton<IAccountProvider, AccountProvider>();
            SplatRegistrations.RegisterLazySingleton<IPartitionProvider, PartitionProvider>();
            SplatRegistrations.RegisterLazySingleton<IRecommendProvider, RecommendProvider>();
            SplatRegistrations.RegisterLazySingleton<IPopularProvider, PopularProvider>();
            SplatRegistrations.RegisterLazySingleton<ILiveProvider, LiveProvider>();
            SplatRegistrations.RegisterLazySingleton<ISpecialColumnProvider, SpecialColumnProvider>();
            SplatRegistrations.RegisterLazySingleton<IPgcProvider, PgcProvider>();
            SplatRegistrations.RegisterLazySingleton<IPlayerProvider, PlayerProvider>();
            SplatRegistrations.RegisterLazySingleton<ISearchProvider, SearchProvider>();
            SplatRegistrations.RegisterLazySingleton<ICommunityProvider, CommunityProvider>();
            SplatRegistrations.RegisterLazySingleton<IUpdateProvider, UpdateProvider>();

            SplatRegistrations.RegisterLazySingleton<AppViewModel>();
            SplatRegistrations.RegisterLazySingleton<AccountViewModel>();
            SplatRegistrations.RegisterLazySingleton<NavigationViewModel>();
            SplatRegistrations.RegisterLazySingleton<RecommendPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<PopularPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<PartitionPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<PartitionDetailPageViewModel>();
            SplatRegistrations.RegisterLazySingleton<RankPageViewModel>();
            SplatRegistrations.Register<VideoItemViewModel>();
            SplatRegistrations.Register<EpisodeItemViewModel>();
            SplatRegistrations.SetupIOC();
        }

        /// <summary>
        /// 注册常量.
        /// </summary>
        public static void RegisterConstants()
        {
            SplatRegistrations.RegisterConstant(Window.Current.CoreWindow.Dispatcher);
            SplatRegistrations.RegisterConstant(new DisplayRequest());
            SplatRegistrations.SetupIOC();
        }
    }
}

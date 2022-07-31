// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Uwp.Pgc;
using Splat;

namespace Bili.App.Pages.Base
{
    /// <summary>
    /// 收藏页面的基类.
    /// </summary>
    public class FavoritePageBase : AppPage<IFavoritePageViewModel>
    {
        /// <summary>
        /// 动漫收藏模块.
        /// </summary>
        protected AnimeFavoriteModuleViewModel AnimeFavoriteModule { get; } = Locator.Current.GetService<AnimeFavoriteModuleViewModel>();

        /// <summary>
        /// 影视收藏模块.
        /// </summary>
        protected CinemaFavoriteModuleViewModel CinemaFavoriteModule { get; } = Locator.Current.GetService<CinemaFavoriteModuleViewModel>();
    }
}

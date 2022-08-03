// Copyright (c) Richasy. All rights reserved.

using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Pgc;
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
        protected IAnimeFavoriteModuleViewModel AnimeFavoriteModule { get; } = Locator.Current.GetService<IAnimeFavoriteModuleViewModel>();

        /// <summary>
        /// 影视收藏模块.
        /// </summary>
        protected ICinemaFavoriteModuleViewModel CinemaFavoriteModule { get; } = Locator.Current.GetService<ICinemaFavoriteModuleViewModel>();
    }
}

// Copyright (c) Richasy. All rights reserved.

using Bili.DI.Container;
using Bili.ViewModels.Interfaces.Account;
using Bili.ViewModels.Interfaces.Pgc;

namespace Bili.Desktop.App.Pages.Base
{
    /// <summary>
    /// 收藏页面的基类.
    /// </summary>
    public class FavoritePageBase : AppPage<IFavoritePageViewModel>
    {
        /// <summary>
        /// 动漫收藏模块.
        /// </summary>
        protected IAnimeFavoriteModuleViewModel AnimeFavoriteModule { get; } = Locator.Instance.GetService<IAnimeFavoriteModuleViewModel>();

        /// <summary>
        /// 影视收藏模块.
        /// </summary>
        protected ICinemaFavoriteModuleViewModel CinemaFavoriteModule { get; } = Locator.Instance.GetService<ICinemaFavoriteModuleViewModel>();
    }
}
